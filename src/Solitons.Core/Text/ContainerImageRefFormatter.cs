using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Solitons.Text
{
    /// <summary>
    /// 
    /// </summary>
    public class ContainerImageRefFormatter
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string? _registry;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string? _tag;

        /// <summary>
        /// Gets the container image registry name
        /// </summary>
        public string? Registry
        {
            get => _registry;
            init
            {
                if (value.IsPrintable())
                {
                    value = Regex.Replace(value!, @"^\s+|(?:\s*/\s*)$", String.Empty);
                    if (IsValidRegistry(value))
                    {
                        _registry = value;
                    }
                    else
                    {
                        throw new InvalidOperationException($"'{value}' is not a valid container image registry.");
                    }
                }
                else
                {
                    _registry = null;
                }
            }
        }


        public string? Tag
        {
            get => _tag;
            init
            {
                if (value.IsPrintable())
                {
                    value = Regex.Replace(value!, @"^(?:\s*:\s*)|\s+$", String.Empty);
                    if (IsValidTag(value))
                    {
                        _tag = value;
                    }
                    else
                    {
                        throw new InvalidOperationException($"'{value}' is not a valid container image registry.");
                    }
                }
                else
                {
                    _tag = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string Format(string imageName)
        {
            imageName = imageName.Trim();
            if (IsValidImage(imageName) == false)
                throw new ArgumentException();
            var builder = new StringBuilder();
            if (Registry.IsPrintable())
            {
                builder.Append(Registry).Append('/');
            }

            builder.Append(imageName);
            if (Tag.IsPrintable())
            {
                builder.Append(':').Append(Tag);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool IsValidImage(string value) =>
            Regex.IsMatch(value, @"(?xis-m)^\w{1,150}(?:\.\w{1,150}){1,5}$");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool IsValidTag(string value) => Regex.IsMatch(value, @"^\w+$");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool IsValidRegistry(string value) => 
            Regex.IsMatch(value, @"(?xis-m)^\w{1,150}(?:\.\w{1,150}){1,10}$");
    }
}
