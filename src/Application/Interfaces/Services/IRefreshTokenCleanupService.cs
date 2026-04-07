namespace Application.Interfaces.Services;

public interface IRefreshTokenCleanupService
{
    Task<int> CleanupAsync(CancellationToken cancellationToken);
}