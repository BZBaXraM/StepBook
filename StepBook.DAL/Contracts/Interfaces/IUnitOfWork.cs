namespace StepBook.DAL.Contracts.Interfaces;

/// <summary>
/// The unit of work
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// The user repository
    /// </summary>
    IUserRepository UserRepository { get; }

    /// <summary>
    /// The like repository
    /// </summary>
    ILikesRepository LikeRepository { get; }

    /// <summary>
    /// The message repository
    /// </summary>
    IMessageRepository MessageRepository { get; }

    /// <summary>
    /// Save all changes
    /// </summary>
    /// <returns></returns>
    Task<bool> Complete();

    /// <summary>
    /// Check if there are changes
    /// </summary>
    /// <returns></returns>
    bool HasChanges();
}