using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class RefreshTokenCleanupService : IRefreshTokenCleanupService
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<RefreshTokenCleanupService> _logger;

    public RefreshTokenCleanupService(
        IApplicationDbContext context,
        ILogger<RefreshTokenCleanupService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> CleanupAsync(CancellationToken cancellationToken)
    {
        var tokensToDelete = await _context.RefreshTokens
            .Where(x => x.IsRevoked || x.ExpiresAt <= DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        if (tokensToDelete.Count == 0)
        {
            _logger.LogInformation("Refresh token cleanup finished. No tokens to delete.");
            return 0;
        }

        _context.RefreshTokens.RemoveRange(tokensToDelete);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Refresh token cleanup finished. DeletedCount={DeletedCount}",
            tokensToDelete.Count);

        return tokensToDelete.Count;
    }
}