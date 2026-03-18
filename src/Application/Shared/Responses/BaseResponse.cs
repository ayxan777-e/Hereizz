namespace Application.Shared.Responses;

public class BaseResponse<T>
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public List<string>? Errors { get; set; }
    public ErrorType ErrorType { get; set; } = ErrorType.None;


    public static BaseResponse<T> Ok(T data, string message = "Success")
    {
        return new BaseResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            ErrorType= ErrorType.None
        };
    }

    public static BaseResponse<T> Fail(string message, List<string> errors, ErrorType errorType = ErrorType.Validation)
         => new BaseResponse<T>
         {
             Success = false,
             Message = message,
             Errors = errors,
             ErrorType = errorType
         };
}

public class BaseResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<string>? Errors { get; set; }
    public ErrorType ErrorType { get; set; } = ErrorType.None;
    public static BaseResponse Ok(string message = "Success")
    {
        return new BaseResponse
        {
            Success = true,
            Message = message,
            ErrorType= ErrorType.None
        };
    }

    public static BaseResponse Fail(string message, List<string> errors, ErrorType errorType = ErrorType.Validation)
     => new BaseResponse
     {
         Success = false,
         Message = message,
         Errors = errors,
         ErrorType = errorType
     };
}