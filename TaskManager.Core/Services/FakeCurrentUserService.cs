using TaskManager.Interfaces;

namespace TaskManager.Services;

public class FakeCurrentUserService : ICurrentUserService
{
    public int UserId { get; } = 1;
}