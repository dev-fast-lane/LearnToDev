namespace LearnToDev.Api.Services;

//The possible outcomes of a service call. The controller turns them into status codes
public enum ServiceStatus
{
    Success,
    NotFound,
    Conflict
}

//A receipt from the service: what happened, the value if it went well, and why if it didn't.
//T is the type of the value, e.g. HousingDto
public record ServiceResult<T>(ServiceStatus Status, T? Value = default, string? Message = null)
{
    public static ServiceResult<T> Ok(T value) => new(ServiceStatus.Success, value);

    public static ServiceResult<T> NotFound() => new(ServiceStatus.NotFound);

    public static ServiceResult<T> Conflict(string message) => new(ServiceStatus.Conflict, default, message);
}