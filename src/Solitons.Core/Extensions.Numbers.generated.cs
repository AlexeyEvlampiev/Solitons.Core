namespace Solitons
{
	using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

	public static partial class Extensions
	{ 
		/// <summary>
		/// Determines and returns the greater value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Int32.</param>
		/// <param name="threshold">The System.Int32 number to compare with the current instance.</param>
		/// <returns>
		/// The greater value between the current instance and the provided System.Int32 value.
		/// </returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int32 Min(this System.Int32 self, System.Int32 threshold) => Math.Max(self, threshold);

		/// <summary>
		/// Determines and returns the smaller value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Int32.</param>
		/// <param name="threshold">The System.Int32 number to compare with the current instance.</param>
		/// <returns>
		/// The lesser value between the current instance and the provided System.Int32 value.
		/// </returns>
		public static System.Int32 Max(this System.Int32 self, System.Int32 threshold) => Math.Min(self, threshold);
	 
		/// <summary>
		/// Determines and returns the greater value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Byte.</param>
		/// <param name="threshold">The System.Byte number to compare with the current instance.</param>
		/// <returns>
		/// The greater value between the current instance and the provided System.Byte value.
		/// </returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Byte Min(this System.Byte self, System.Byte threshold) => Math.Max(self, threshold);

		/// <summary>
		/// Determines and returns the smaller value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Byte.</param>
		/// <param name="threshold">The System.Byte number to compare with the current instance.</param>
		/// <returns>
		/// The lesser value between the current instance and the provided System.Byte value.
		/// </returns>
		public static System.Byte Max(this System.Byte self, System.Byte threshold) => Math.Min(self, threshold);
	 
		/// <summary>
		/// Determines and returns the greater value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Int64.</param>
		/// <param name="threshold">The System.Int64 number to compare with the current instance.</param>
		/// <returns>
		/// The greater value between the current instance and the provided System.Int64 value.
		/// </returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int64 Min(this System.Int64 self, System.Int64 threshold) => Math.Max(self, threshold);

		/// <summary>
		/// Determines and returns the smaller value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Int64.</param>
		/// <param name="threshold">The System.Int64 number to compare with the current instance.</param>
		/// <returns>
		/// The lesser value between the current instance and the provided System.Int64 value.
		/// </returns>
		public static System.Int64 Max(this System.Int64 self, System.Int64 threshold) => Math.Min(self, threshold);
	 
		/// <summary>
		/// Determines and returns the greater value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Decimal.</param>
		/// <param name="threshold">The System.Decimal number to compare with the current instance.</param>
		/// <returns>
		/// The greater value between the current instance and the provided System.Decimal value.
		/// </returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Decimal Min(this System.Decimal self, System.Decimal threshold) => Math.Max(self, threshold);

		/// <summary>
		/// Determines and returns the smaller value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Decimal.</param>
		/// <param name="threshold">The System.Decimal number to compare with the current instance.</param>
		/// <returns>
		/// The lesser value between the current instance and the provided System.Decimal value.
		/// </returns>
		public static System.Decimal Max(this System.Decimal self, System.Decimal threshold) => Math.Min(self, threshold);
	 
		/// <summary>
		/// Determines and returns the greater value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Single.</param>
		/// <param name="threshold">The System.Single number to compare with the current instance.</param>
		/// <returns>
		/// The greater value between the current instance and the provided System.Single value.
		/// </returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Single Min(this System.Single self, System.Single threshold) => Math.Max(self, threshold);

		/// <summary>
		/// Determines and returns the smaller value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Single.</param>
		/// <param name="threshold">The System.Single number to compare with the current instance.</param>
		/// <returns>
		/// The lesser value between the current instance and the provided System.Single value.
		/// </returns>
		public static System.Single Max(this System.Single self, System.Single threshold) => Math.Min(self, threshold);
	 
		/// <summary>
		/// Determines and returns the greater value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Double.</param>
		/// <param name="threshold">The System.Double number to compare with the current instance.</param>
		/// <returns>
		/// The greater value between the current instance and the provided System.Double value.
		/// </returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Double Min(this System.Double self, System.Double threshold) => Math.Max(self, threshold);

		/// <summary>
		/// Determines and returns the smaller value between the current instance and the provided parameter.
		/// </summary>
		/// <param name="self">The current instance of System.Double.</param>
		/// <param name="threshold">The System.Double number to compare with the current instance.</param>
		/// <returns>
		/// The lesser value between the current instance and the provided System.Double value.
		/// </returns>
		public static System.Double Max(this System.Double self, System.Double threshold) => Math.Min(self, threshold);
	 
	}


	public static partial class Extensions
	{ 				

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBetween(this System.Int32 self, System.Int32 min, System.Int32 max) => (self >= min && self <= max);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int32 ThrowIfOutOfRange(this System.Int32 self, System.Int32 min, System.Int32 max, Func<Exception> errorFactory)
		{
			if(self < min || self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int32 ThrowIfArgumentOutOfRange(this System.Int32 self, System.Int32 min, System.Int32 max, string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min || self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int32 ThrowIfLessThan(this System.Int32 self, System.Int32 min, Func<Exception> errorFactory)
		{
			if(self < min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int32 ThrowIfLessOrEqual(this System.Int32 self, System.Int32 min, Func<Exception> errorFactory)
		{
			if(self <= min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int32 ThrowIfArgumentLessThan(this System.Int32 self, System.Int32 min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int32 ThrowIfArgumentLessOrEqual(this System.Int32 self, System.Int32 min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self <= min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int32 ThrowIfGreaterThan(this System.Int32 self, System.Int32 max, Func<Exception> errorFactory)
		{
			if(self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int32 ThrowIfGreaterOrEqual(this System.Int32 self, System.Int32 max, Func<Exception> errorFactory)
		{
			if(self >= max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int32 ThrowIfArgumentGreaterThan(this System.Int32 self, System.Int32 max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int32 ThrowIfArgumentGreaterOrEqual(this System.Int32 self, System.Int32 max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		} 				

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBetween(this System.Byte self, System.Byte min, System.Byte max) => (self >= min && self <= max);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Byte ThrowIfOutOfRange(this System.Byte self, System.Byte min, System.Byte max, Func<Exception> errorFactory)
		{
			if(self < min || self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Byte ThrowIfArgumentOutOfRange(this System.Byte self, System.Byte min, System.Byte max, string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min || self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Byte ThrowIfLessThan(this System.Byte self, System.Byte min, Func<Exception> errorFactory)
		{
			if(self < min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Byte ThrowIfLessOrEqual(this System.Byte self, System.Byte min, Func<Exception> errorFactory)
		{
			if(self <= min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Byte ThrowIfArgumentLessThan(this System.Byte self, System.Byte min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Byte ThrowIfArgumentLessOrEqual(this System.Byte self, System.Byte min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self <= min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Byte ThrowIfGreaterThan(this System.Byte self, System.Byte max, Func<Exception> errorFactory)
		{
			if(self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Byte ThrowIfGreaterOrEqual(this System.Byte self, System.Byte max, Func<Exception> errorFactory)
		{
			if(self >= max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Byte ThrowIfArgumentGreaterThan(this System.Byte self, System.Byte max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Byte ThrowIfArgumentGreaterOrEqual(this System.Byte self, System.Byte max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		} 				

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBetween(this System.Int64 self, System.Int64 min, System.Int64 max) => (self >= min && self <= max);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int64 ThrowIfOutOfRange(this System.Int64 self, System.Int64 min, System.Int64 max, Func<Exception> errorFactory)
		{
			if(self < min || self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int64 ThrowIfArgumentOutOfRange(this System.Int64 self, System.Int64 min, System.Int64 max, string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min || self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int64 ThrowIfLessThan(this System.Int64 self, System.Int64 min, Func<Exception> errorFactory)
		{
			if(self < min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int64 ThrowIfLessOrEqual(this System.Int64 self, System.Int64 min, Func<Exception> errorFactory)
		{
			if(self <= min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int64 ThrowIfArgumentLessThan(this System.Int64 self, System.Int64 min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int64 ThrowIfArgumentLessOrEqual(this System.Int64 self, System.Int64 min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self <= min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int64 ThrowIfGreaterThan(this System.Int64 self, System.Int64 max, Func<Exception> errorFactory)
		{
			if(self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int64 ThrowIfGreaterOrEqual(this System.Int64 self, System.Int64 max, Func<Exception> errorFactory)
		{
			if(self >= max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int64 ThrowIfArgumentGreaterThan(this System.Int64 self, System.Int64 max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Int64 ThrowIfArgumentGreaterOrEqual(this System.Int64 self, System.Int64 max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		} 				

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBetween(this System.Decimal self, System.Decimal min, System.Decimal max) => (self >= min && self <= max);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Decimal ThrowIfOutOfRange(this System.Decimal self, System.Decimal min, System.Decimal max, Func<Exception> errorFactory)
		{
			if(self < min || self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Decimal ThrowIfArgumentOutOfRange(this System.Decimal self, System.Decimal min, System.Decimal max, string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min || self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Decimal ThrowIfLessThan(this System.Decimal self, System.Decimal min, Func<Exception> errorFactory)
		{
			if(self < min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Decimal ThrowIfLessOrEqual(this System.Decimal self, System.Decimal min, Func<Exception> errorFactory)
		{
			if(self <= min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Decimal ThrowIfArgumentLessThan(this System.Decimal self, System.Decimal min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Decimal ThrowIfArgumentLessOrEqual(this System.Decimal self, System.Decimal min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self <= min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Decimal ThrowIfGreaterThan(this System.Decimal self, System.Decimal max, Func<Exception> errorFactory)
		{
			if(self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Decimal ThrowIfGreaterOrEqual(this System.Decimal self, System.Decimal max, Func<Exception> errorFactory)
		{
			if(self >= max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Decimal ThrowIfArgumentGreaterThan(this System.Decimal self, System.Decimal max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Decimal ThrowIfArgumentGreaterOrEqual(this System.Decimal self, System.Decimal max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		} 				

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBetween(this System.Single self, System.Single min, System.Single max) => (self >= min && self <= max);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Single ThrowIfOutOfRange(this System.Single self, System.Single min, System.Single max, Func<Exception> errorFactory)
		{
			if(self < min || self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Single ThrowIfArgumentOutOfRange(this System.Single self, System.Single min, System.Single max, string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min || self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Single ThrowIfLessThan(this System.Single self, System.Single min, Func<Exception> errorFactory)
		{
			if(self < min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Single ThrowIfLessOrEqual(this System.Single self, System.Single min, Func<Exception> errorFactory)
		{
			if(self <= min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Single ThrowIfArgumentLessThan(this System.Single self, System.Single min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Single ThrowIfArgumentLessOrEqual(this System.Single self, System.Single min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self <= min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Single ThrowIfGreaterThan(this System.Single self, System.Single max, Func<Exception> errorFactory)
		{
			if(self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Single ThrowIfGreaterOrEqual(this System.Single self, System.Single max, Func<Exception> errorFactory)
		{
			if(self >= max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Single ThrowIfArgumentGreaterThan(this System.Single self, System.Single max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Single ThrowIfArgumentGreaterOrEqual(this System.Single self, System.Single max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		} 				

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBetween(this System.Double self, System.Double min, System.Double max) => (self >= min && self <= max);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Double ThrowIfOutOfRange(this System.Double self, System.Double min, System.Double max, Func<Exception> errorFactory)
		{
			if(self < min || self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Double ThrowIfArgumentOutOfRange(this System.Double self, System.Double min, System.Double max, string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min || self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Double ThrowIfLessThan(this System.Double self, System.Double min, Func<Exception> errorFactory)
		{
			if(self < min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Double ThrowIfLessOrEqual(this System.Double self, System.Double min, Func<Exception> errorFactory)
		{
			if(self <= min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Double ThrowIfArgumentLessThan(this System.Double self, System.Double min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Double ThrowIfArgumentLessOrEqual(this System.Double self, System.Double min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self <= min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Double ThrowIfGreaterThan(this System.Double self, System.Double max, Func<Exception> errorFactory)
		{
			if(self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Double ThrowIfGreaterOrEqual(this System.Double self, System.Double max, Func<Exception> errorFactory)
		{
			if(self >= max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Double ThrowIfArgumentGreaterThan(this System.Double self, System.Double max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Double ThrowIfArgumentGreaterOrEqual(this System.Double self, System.Double max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		} 				

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBetween(this System.TimeSpan self, System.TimeSpan min, System.TimeSpan max) => (self >= min && self <= max);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.TimeSpan ThrowIfOutOfRange(this System.TimeSpan self, System.TimeSpan min, System.TimeSpan max, Func<Exception> errorFactory)
		{
			if(self < min || self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.TimeSpan ThrowIfArgumentOutOfRange(this System.TimeSpan self, System.TimeSpan min, System.TimeSpan max, string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min || self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.TimeSpan ThrowIfLessThan(this System.TimeSpan self, System.TimeSpan min, Func<Exception> errorFactory)
		{
			if(self < min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.TimeSpan ThrowIfLessOrEqual(this System.TimeSpan self, System.TimeSpan min, Func<Exception> errorFactory)
		{
			if(self <= min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.TimeSpan ThrowIfArgumentLessThan(this System.TimeSpan self, System.TimeSpan min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.TimeSpan ThrowIfArgumentLessOrEqual(this System.TimeSpan self, System.TimeSpan min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self <= min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.TimeSpan ThrowIfGreaterThan(this System.TimeSpan self, System.TimeSpan max, Func<Exception> errorFactory)
		{
			if(self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.TimeSpan ThrowIfGreaterOrEqual(this System.TimeSpan self, System.TimeSpan max, Func<Exception> errorFactory)
		{
			if(self >= max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.TimeSpan ThrowIfArgumentGreaterThan(this System.TimeSpan self, System.TimeSpan max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.TimeSpan ThrowIfArgumentGreaterOrEqual(this System.TimeSpan self, System.TimeSpan max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		} 				

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBetween(this System.DateTime self, System.DateTime min, System.DateTime max) => (self >= min && self <= max);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTime ThrowIfOutOfRange(this System.DateTime self, System.DateTime min, System.DateTime max, Func<Exception> errorFactory)
		{
			if(self < min || self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTime ThrowIfArgumentOutOfRange(this System.DateTime self, System.DateTime min, System.DateTime max, string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min || self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTime ThrowIfLessThan(this System.DateTime self, System.DateTime min, Func<Exception> errorFactory)
		{
			if(self < min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTime ThrowIfLessOrEqual(this System.DateTime self, System.DateTime min, Func<Exception> errorFactory)
		{
			if(self <= min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTime ThrowIfArgumentLessThan(this System.DateTime self, System.DateTime min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTime ThrowIfArgumentLessOrEqual(this System.DateTime self, System.DateTime min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self <= min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTime ThrowIfGreaterThan(this System.DateTime self, System.DateTime max, Func<Exception> errorFactory)
		{
			if(self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTime ThrowIfGreaterOrEqual(this System.DateTime self, System.DateTime max, Func<Exception> errorFactory)
		{
			if(self >= max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTime ThrowIfArgumentGreaterThan(this System.DateTime self, System.DateTime max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTime ThrowIfArgumentGreaterOrEqual(this System.DateTime self, System.DateTime max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		} 				

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBetween(this System.DateTimeOffset self, System.DateTimeOffset min, System.DateTimeOffset max) => (self >= min && self <= max);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTimeOffset ThrowIfOutOfRange(this System.DateTimeOffset self, System.DateTimeOffset min, System.DateTimeOffset max, Func<Exception> errorFactory)
		{
			if(self < min || self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTimeOffset ThrowIfArgumentOutOfRange(this System.DateTimeOffset self, System.DateTimeOffset min, System.DateTimeOffset max, string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min || self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTimeOffset ThrowIfLessThan(this System.DateTimeOffset self, System.DateTimeOffset min, Func<Exception> errorFactory)
		{
			if(self < min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTimeOffset ThrowIfLessOrEqual(this System.DateTimeOffset self, System.DateTimeOffset min, Func<Exception> errorFactory)
		{
			if(self <= min)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTimeOffset ThrowIfArgumentLessThan(this System.DateTimeOffset self, System.DateTimeOffset min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self < min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="min"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTimeOffset ThrowIfArgumentLessOrEqual(this System.DateTimeOffset self, System.DateTimeOffset min,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self <= min)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTimeOffset ThrowIfGreaterThan(this System.DateTimeOffset self, System.DateTimeOffset max, Func<Exception> errorFactory)
		{
			if(self > max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="errorFactory"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTimeOffset ThrowIfGreaterOrEqual(this System.DateTimeOffset self, System.DateTimeOffset max, Func<Exception> errorFactory)
		{
			if(self >= max)throw errorFactory?.Invoke() ?? new ArgumentOutOfRangeException(nameof(self));
			return self;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns><paramref name="self"/></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTimeOffset ThrowIfArgumentGreaterThan(this System.DateTimeOffset self, System.DateTimeOffset max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="max"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.DateTimeOffset ThrowIfArgumentGreaterOrEqual(this System.DateTimeOffset self, System.DateTimeOffset max,  string paramName, string message = null)
		{
			if(string.IsNullOrWhiteSpace(paramName))paramName = nameof(self);
			if(self > max)
			{
				throw string.IsNullOrWhiteSpace(message) 
					? new ArgumentOutOfRangeException(paramName)
					: new ArgumentOutOfRangeException(message, paramName);
			}
			return self;
		} 		
	}
}