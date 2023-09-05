namespace Application.Dto;

public record class SupervisorDto(ICollection<WorkerDto> Workers, Guid Id, string Name);