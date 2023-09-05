namespace Application.Dto;

public record class MessageHandlerDto(Guid WorkerId, ICollection<MessageDto> Messages);