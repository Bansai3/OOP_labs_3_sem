namespace Presentation.Models;

public record class CreateMessageSourceConnectionToWorker(Guid MessageSourceId, Guid WorkerId);
