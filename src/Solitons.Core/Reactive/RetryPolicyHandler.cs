using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Reactive;

/// <summary>
/// Represents the method that will handle the retry policy.
/// </summary>
/// <param name="args">The arguments associated with the retry policy.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether to retry or not.</returns>
public delegate Task<bool> RetryPolicyHandler(RetryPolicyArgs args);
