namespace TempHumidityBackend.Types;

/// <summary>
/// A monad which wraps the results of an operation.
/// </summary>
/// <typeparam name="TResult">The type of result to wrap.</typeparam>
public sealed class CBORDecodeResult<TResult> where TResult : class
{
    public bool HasValue { get; init; }
    public string? ErrorMessage { get; init; }
    public TResult? Value { get; init; }

    /// <summary>
    /// Returns a new <see cref="CBORDecodeResult"/> instance wrapping a successful result.
    /// </summary>
    /// <param name="data">The data to wrap and return.</param>
    /// <returns>A <see cref="CBORDecodeResult"/> containing the results of a successful operation.</returns>
    public static CBORDecodeResult<TResult> Success(TResult data) =>
        new() { HasValue = true, ErrorMessage = string.Empty, Value = data };

    /// <summary>
    /// Returns a new <see cref="CBORDecodeResult"/> instance wrapping an error message.
    /// </summary>
    /// <param name="errorMessage">The error message to wrap and return.</param>
    /// <returns>A <see cref="CBORDecodeResult"/> containing the error message of an unsuccessful operation.</returns>
    public static CBORDecodeResult<TResult> Error(string errorMessage) =>
        new() { HasValue = false, ErrorMessage = errorMessage };
}
