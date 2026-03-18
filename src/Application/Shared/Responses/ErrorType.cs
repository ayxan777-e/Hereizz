namespace Application.Shared.Responses;

public enum ErrorType
{
    None = 0,
    Validation = 1,
    NotFound = 2,
    BusinessRule = 3,
    Conflict = 4,
    Unauthorized = 5,
    BadRequest = 6
}