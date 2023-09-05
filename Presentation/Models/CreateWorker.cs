namespace Presentation.Models;

public record class CreateWorker(string Name, Guid SupervisorId, string Login, string Password);