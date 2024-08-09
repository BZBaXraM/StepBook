using StepBook.API.Contracts.Interfaces;
using StepBook.API.Repositories.Interfaces;

namespace StepBook.API.Contracts.Classes;

public class UnitOfWork(
    StepContext context,
    IUserRepository userRepository,
    ILikesRepository likesRepository,
    IMessageRepository messageRepository) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;
    public ILikesRepository LikeRepository => likesRepository;
    public IMessageRepository MessageRepository => messageRepository;

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}