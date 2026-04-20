using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Repositories;

public interface IApplicationDbContext
{
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Notification> Notifications { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
