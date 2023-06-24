using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Solitons.Text
{
    /// <summary>
    /// Represents a base class for generating text templates.
    /// </summary>
    public abstract class RuntimeTextTemplate
    {
        #region Fields
        private List<string>? _errors;
        private List<string>? _warnings;
        private readonly List<int> _indentLengths = new();
        private bool _endsWithNewline;
        private IFormatProvider _formatProvider = System.Globalization.CultureInfo.InvariantCulture;
        private Dictionary<string, object> _session = new();
        private readonly object _syncObject = new();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeTextTemplate"/> class.
        /// </summary>
        protected RuntimeTextTemplate()
        {
            CurrentIndent = String.Empty;
            GenerationEnvironment = new StringBuilder();
            ToStringHelper = new ToStringInstanceHelper(this);
        }

        /// <summary>
        /// Implicitly converts an instance of the <see cref="RuntimeTextTemplate"/> class to a string.
        /// </summary>
        /// <param name="template">The <see cref="RuntimeTextTemplate"/> instance to be converted.</param>
        public static implicit operator string(RuntimeTextTemplate template) => template.ToString();

        /// <summary>
        /// When overridden in a derived class, transforms the text template.
        /// </summary>
        public abstract string TransformText();

        /// <summary>
        /// When overridden in a derived class, performs additional transformations on the generated text.
        /// </summary>
        /// <param name="text">The generated text.</param>
        protected virtual string PostTransformText(string text) => text;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sealed override string ToString()
        {
            lock (_syncObject)
            {
                _warnings = null;
                _errors = null;
                GenerationEnvironment.Clear();
                _indentLengths.Clear();
                CurrentIndent = string.Empty;
                var text = TransformText();
                return PostTransformText(text);
            }
        }

        /// <summary>
        /// Gets or sets format provider to be used by ToStringWithCulture method.
        /// </summary>
        public IFormatProvider FormatProvider
        {
            get => _formatProvider ?? System.Globalization.CultureInfo.InvariantCulture;
            set => _formatProvider = value;
        }


        /// <summary>
        /// When overridden in a derived class, converts an object within an expression block to a string using the specified format provider.
        /// </summary>
        /// <param name="objectToConvert">The object to convert.</param>
        protected virtual string ToStringWithCulture(object objectToConvert)
        {
            if ((objectToConvert == null))
            {
                throw new ArgumentNullException(nameof(objectToConvert));
            }
            if (objectToConvert is bool boolean) return boolean.ToString().ToLower();
            var t = objectToConvert.GetType();
            var method = t.GetMethod(nameof(ToString), new[] {
                typeof(IFormatProvider)});
            if ((method == null))
            {
                return objectToConvert.ToString() ?? string.Empty;
            }

            return method
                .Invoke(objectToConvert, new object[]
                {
                    FormatProvider
                })
                .Convert(_ => _?.ToString() ?? string.Empty);
        }


        #region Properties
        /// <summary>
        /// Gets the string builder that generation-time code is using to assemble generated output.
        /// </summary>
        protected StringBuilder GenerationEnvironment { get; }

        /// <summary>
        /// Gets the errors generated during the transformation process.
        /// </summary>
        public IEnumerable<string> Errors => _errors?.AsEnumerable() ?? Enumerable.Empty<string>();

        /// <summary>
        /// Gets the warnings generated during the transformation process.
        /// </summary>
        public IEnumerable<string> Warnings => _warnings?.AsEnumerable() ?? Enumerable.Empty<string>();


        /// <summary>
        /// Gets the current indentation string.
        /// </summary>
        public string CurrentIndent { get; private set; }

        /// <summary>
        /// Gets or sets the session information.
        /// </summary>
        public virtual Dictionary<string, object> Session
        {
            get => _session ??= new Dictionary<string, object>();
            set => _session = value;
        }

        #endregion

        #region Transform-time helpers

        /// <summary>
        /// Writes text directly into the generated output.
        /// </summary>
        /// <param name="textToAppend">The text to append.</param>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((GenerationEnvironment.Length == 0)
                        || _endsWithNewline))
            {
                GenerationEnvironment.Append(CurrentIndent);
                _endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(Environment.NewLine, StringComparison.CurrentCulture))
            {
                _endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((CurrentIndent.Length == 0))
            {
                GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(Environment.NewLine, (Environment.NewLine + CurrentIndent));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (_endsWithNewline)
            {
                GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - CurrentIndent.Length));
            }
            else
            {
                GenerationEnvironment.Append(textToAppend);
            }
        }

        /// <summary>
        /// Writes a line of text directly into the generated output.
        /// </summary>
        /// <param name="textToAppend">The text to append.</param>
        public void WriteLine(string textToAppend)
        {
            Write(textToAppend);
            GenerationEnvironment.AppendLine();
            _endsWithNewline = true;
        }

        /// <summary>
        /// Writes formatted text directly into the generated output.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void Write(string format, params object[] args)
        {
            Write(string.Format(System.Globalization.CultureInfo.CurrentCulture, format, args));
        }

        /// <summary>
        /// Writes a line of formatted text directly into the generated output.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        public void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(System.Globalization.CultureInfo.CurrentCulture, format, args));
        }

        /// <summary>
        /// Raises an error with the specified message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public void Error(string message)
        {
            LazyInitializer.EnsureInitialized(ref _errors, () => new List<string>());
            _errors.Add(message);
        }

        /// <summary>
        /// Raises a warning with the specified message.
        /// </summary>
        /// <param name="message">The warning message.</param>
        public void Warning(string message)
        {
            LazyInitializer.EnsureInitialized(ref _warnings, () => new List<string>());
            _warnings.Add(message);
        }

        /// <summary>
        /// Increases the current indentation level.
        /// </summary>
        /// <param name="indent">The string to use for indentation.</param>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new ArgumentNullException(nameof(indent));
            }
            
            CurrentIndent = (CurrentIndent + indent);
            _indentLengths.Add(indent.Length);
        }

        /// <summary>
        /// Decreases the current indentation level.
        /// </summary>
        /// <returns>The removed indentation string.</returns>
        public string PopIndent()
        {
            var returnValue = "";
            if ((_indentLengths.Count > 0))
            {
                var indentLength = _indentLengths[^1];
                _indentLengths.RemoveAt((_indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = CurrentIndent.Substring((CurrentIndent.Length - indentLength));
                    CurrentIndent = CurrentIndent.Remove((CurrentIndent.Length - indentLength));
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Removes any current indentation.
        /// </summary>
        public void ClearIndent()
        {
            _indentLengths.Clear();
            CurrentIndent = "";
        }
        #endregion

        #region ToString Helpers

        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        protected sealed class ToStringInstanceHelper
        {
            private readonly RuntimeTextTemplate _rtt;

            internal ToStringInstanceHelper(RuntimeTextTemplate rtt)
            {
                Debug.Assert(rtt != null);
                _rtt = rtt;
            }

            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public IFormatProvider FormatProvider
            {
                get => _rtt.FormatProvider;
                set => _rtt.FormatProvider = value;
            }

            /// <summary>
            /// Converts an object within an expression block to a culture-oriented string representation.
            /// </summary>
            /// <param name="objectToConvert">The object to convert.</param>
            /// <returns>The string representation of the object.</returns>
            [DebuggerStepThrough]
            public string ToStringWithCulture(object objectToConvert) => _rtt.ToStringWithCulture(objectToConvert);

        }
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string.
        /// </summary>
        protected ToStringInstanceHelper ToStringHelper { get; } 

        #endregion
    }
}
