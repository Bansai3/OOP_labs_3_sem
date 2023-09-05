namespace Application.Dto;

public record class WorkerDto(ICollection<MessageSourceDto> MessageSources, Guid SupervisorId, Guid Id, string Name);
