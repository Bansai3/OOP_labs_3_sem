using Microsoft.EntityFrameworkCore;
using Application.Exceptions;
using DataAccess.Models;

namespace Application.Extensions;

public static class DbSetExtensions
{
    public static async Task<T> GetEntityAsync<T>(this DbSet<T> set, Guid id, CancellationToken cancellationToken)
        where T : class
    {
        T? entity = await set.FindAsync(new object[] { id }, cancellationToken);

        if (entity is null)
            throw new EntityNotFoundException($"Entity with id {id} does not exist!");

        return entity;
    }
}