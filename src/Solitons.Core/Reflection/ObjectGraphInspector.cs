using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Collections;

namespace Solitons.Reflection
{

    /// <summary>
    /// 
    /// </summary>
    public sealed class ObjectGraphInspector : IDisposable
    {
        private readonly IReadOnlyList<IObjectPropertyInspector> _propertyInspectors;
        private readonly EventLoopScheduler _scheduler;
        private readonly Dictionary<Type, PropertyInfo[]> _properties = new();
        private readonly Dictionary<PropertyInfo, ParameterInfo[]> _indexParameters = new();

        #region ctor

        /// <summary>
        /// 
        /// </summary>
        public ObjectGraphInspector()
        {
            _scheduler = new EventLoopScheduler();
            _propertyInspectors = new List<IObjectPropertyInspector>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ObjectGraphInspector(EventLoopScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _propertyInspectors = new List<IObjectPropertyInspector>();
        }

        private ObjectGraphInspector(ObjectGraphInspector other, IEnumerable<IObjectPropertyInspector> addedPropertyInspectors)
        {
            _propertyInspectors = new List<IObjectPropertyInspector>(other._propertyInspectors
                .Union(addedPropertyInspectors)
                .Distinct());
            _scheduler = other._scheduler;
            _properties = other._properties;
            _indexParameters = other._indexParameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="inspectors"></param>
        [DebuggerNonUserCode]
        private ObjectGraphInspector(
            EventLoopScheduler scheduler,
            IObjectPropertyInspector[] inspectors)
        {
            _scheduler = scheduler;
            _propertyInspectors = inspectors.Distinct().ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="inspectors"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static ObjectGraphInspector Create(
            EventLoopScheduler scheduler,
            IEnumerable<IObjectPropertyInspector> inspectors)
        {
            if (scheduler == null) throw new ArgumentNullException(nameof(scheduler));
            if (inspectors == null) throw new ArgumentNullException(nameof(inspectors));
            return new ObjectGraphInspector(
                scheduler, 
                inspectors
                    .SkipNulls()
                    .Distinct()
                    .ToArray()
                    .ThrowIfNullOrEmptyArgument(nameof(inspectors)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectors"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static ObjectGraphInspector Create(IEnumerable<IObjectPropertyInspector> inspectors)
        {
            var scheduler = new EventLoopScheduler();
            try
            {
                return Create(scheduler, inspectors);
            }
            catch (Exception)
            {
                scheduler.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="inspector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static ObjectGraphInspector Create(
            EventLoopScheduler scheduler,
            IObjectPropertyInspector inspector)
        {
            if (inspector == null) throw new ArgumentNullException(nameof(inspector));
            return Create(scheduler, FluentEnumerable.Yield(inspector));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="inspectors"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static ObjectGraphInspector Create(
            EventLoopScheduler scheduler,
            params IObjectPropertyInspector[] inspectors)
        {
            if (inspectors == null) throw new ArgumentNullException(nameof(inspectors));
            return Create(scheduler, inspectors.AsEnumerable());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static ObjectGraphInspector Create(
            IObjectPropertyInspector inspector)
        {
            if (inspector == null) throw new ArgumentNullException(nameof(inspector));
            return Create(FluentEnumerable.Yield(inspector));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectors"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static ObjectGraphInspector Create(
            params IObjectPropertyInspector[] inspectors)
        {
            if (inspectors == null) throw new ArgumentNullException(nameof(inspectors));
            return Create(inspectors.AsEnumerable());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ObjectGraphInspector WithPropertyInspector(IObjectPropertyInspector inspector)
        {
            if (inspector == null) throw new ArgumentNullException(nameof(inspector));
            return new ObjectGraphInspector(this, new[] { inspector });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectors"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ObjectGraphInspector WithPropertyInspector(IEnumerable<IObjectPropertyInspector> inspectors)
        {
            if (inspectors == null) throw new ArgumentNullException(nameof(inspectors));
            return new ObjectGraphInspector(this, inspectors);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectors"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ObjectGraphInspector WithPropertyInspector(params IObjectPropertyInspector[] inspectors)
        {
            if (inspectors == null) throw new ArgumentNullException(nameof(inspectors));
            if (inspectors.Length == 0) return this;
            return new ObjectGraphInspector(this, inspectors);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task InspectAsync(object obj, CancellationToken cancellation = default)
        {
            if(obj is null)return;

            var inspected = new HashSet<object>();

            await Observable
                .Return(obj)
                .ObserveOn(_scheduler)
                .Do(_ => Inspect(obj, inspected.Add))
                .ToTask(cancellation);
        }

        void Inspect(object target, Func<object, bool> inspectionRequired)
        {
            if(target is null)return;
            if(false == inspectionRequired.Invoke(target))return;
            var properties = _properties.GetOrAdd(target.GetType(), ()=> target.GetType().GetProperties());
            foreach (var property in properties)
            {
                if (false == _indexParameters.TryGetValue(property, out var indexParameters))
                {
                    indexParameters = property.GetIndexParameters();
                    indexParameters = indexParameters.Length > 0
                        ? indexParameters
                        : Array.Empty<ParameterInfo>();
                    _indexParameters.Add(property, indexParameters);
                }
                _propertyInspectors.ForEach(i=> i.Inspect(target, property));
                if (indexParameters.Length > 0) continue;
                if (property.GetMethod == null) continue;
                var value = property.GetValue(target);
                Inspect(value, inspectionRequired);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void IDisposable.Dispose()
        {
            _scheduler.Dispose();
        }
    }
}
