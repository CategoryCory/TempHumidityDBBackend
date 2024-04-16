namespace TempHumidityBackend;

public sealed class CBORDecodeResult<TResult> where TResult : class
{
    public bool HasValue { get; init; }
    public string? ErrorMessage { get; init; }
    public TResult? Value { get; init; }

    public static CBORDecodeResult<TResult> Success(TResult data) =>
        new() { HasValue = true, ErrorMessage = string.Empty, Value = data };
    
    public static CBORDecodeResult<TResult> Error(string errorMessage) =>
        new() { HasValue = false, ErrorMessage = errorMessage };
}
