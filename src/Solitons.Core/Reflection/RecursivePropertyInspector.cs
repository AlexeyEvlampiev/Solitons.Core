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
    public sealed class RecursivePropertyInspector : IDisposable
    {
        private readonly IPropertyInspector[] _inspectors;
        private readonly EventLoopScheduler _scheduler;
        private readonly Dictionary<Type, PropertyInfo[]> _properties = new();
        private readonly Dictionary<PropertyInfo, ParameterInfo[]> _indexParameters = new();

        #region ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="inspectors"></param>
        [DebuggerNonUserCode]
        private RecursivePropertyInspector(
            EventLoopScheduler scheduler,
            IPropertyInspector[] inspectors)
        {
            _scheduler = scheduler;
            _inspectors = inspectors;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="inspectors"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static RecursivePropertyInspector Create(
            EventLoopScheduler scheduler,
            IEnumerable<IPropertyInspector> inspectors)
        {
            if (scheduler == null) throw new ArgumentNullException(nameof(scheduler));
            if (inspectors == null) throw new ArgumentNullException(nameof(inspectors));
            return new RecursivePropertyInspector(
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
        public static RecursivePropertyInspector Create(IEnumerable<IPropertyInspector> inspectors)
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
        public static RecursivePropertyInspector Create(
            EventLoopScheduler scheduler,
            IPropertyInspector inspector)
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
        public static RecursivePropertyInspector Create(
            EventLoopScheduler scheduler,
            params IPropertyInspector[] inspectors)
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
        public static RecursivePropertyInspector Create(
            IPropertyInspector inspector)
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
        public static RecursivePropertyInspector Create(
            params IPropertyInspector[] inspectors)
        {
            if (inspectors == null) throw new ArgumentNullException(nameof(inspectors));
            return Create(inspectors.AsEnumerable());
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

        void Inspect(object target, Func<object, bool> shouldInspect)
        {
            if(target is null)return;
            if(false == shouldInspect.Invoke(target))return;
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

                Array.ForEach(_inspectors, inspector=> inspector.Inspect(target, property));
                if (indexParameters.Length > 0) continue;
                if (property.GetMethod == null) continue;
                var value = property.GetValue(target);
                Inspect(value, shouldInspect);
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
