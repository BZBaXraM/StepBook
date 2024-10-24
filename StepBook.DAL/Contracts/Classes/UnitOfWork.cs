using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StepBook.API.Contracts.Interfaces;
using StepBook.DAL.Data;
using StepBook.DAL.Repositories.Interfaces;

namespace StepBook.DAL.Contracts.Classes;

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
        try
        {
            return await context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            Debug.WriteLine($"Postgres error: {pgEx.SqlState}, {pgEx.MessageText}");
            throw new Exception("Database update error occurred", ex);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unexpected error: {ex.Message}");
            throw new Exception("An error occurred while saving changes", ex);
        }
    }


    public bool HasChanges()
    {
        try
        {
            return context.ChangeTracker.HasChanges();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            throw new Exception("An error occurred while checking for changes", e);
        }
    }
}