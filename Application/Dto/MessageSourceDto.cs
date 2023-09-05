namespace Application.Dto;

public record class MessageSourceDto(ICollection<MessageDto> Messages, ICollection<WorkerDto> Workers, Guid Id, string Name);