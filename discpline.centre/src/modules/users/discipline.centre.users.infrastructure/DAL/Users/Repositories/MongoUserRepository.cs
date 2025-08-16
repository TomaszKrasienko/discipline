using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.DAL;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Repositories;
using discipline.centre.users.infrastructure.DAL.Documents;
using discipline.centre.users.infrastructure.DAL.Users.Documents;
using MongoDB.Driver;

namespace discipline.centre.users.infrastructure.DAL.Users.Repositories;

internal sealed class MongoUserRepository(
    UsersMongoContext context) : BaseRepository<UserDocument>(context), IReadWriteUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var document = user.ToDocumet();
        await AddAsync(document, cancellationToken);
    } 
    
    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var document = user.ToDocumet();
        
        await context.GetCollection<UserDocument>()
            .FindOneAndReplaceAsync(
                x => x.Id == document.Id,
                document,
                cancellationToken: cancellationToken);
    } 

    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => (await GetAsync(x => x.Id == id.Value.ToString(), cancellationToken))?.ToEntity();

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => (await GetAsync(x => x.Email == email, cancellationToken))?.ToEntity();
    
    public Task<bool> DoesEmailExistAsync(string email, CancellationToken cancellationToken = default)
        => AnyAsync(x => x.Email == email, cancellationToken);
}

