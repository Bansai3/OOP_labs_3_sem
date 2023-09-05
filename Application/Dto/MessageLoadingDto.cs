namespace Application.Dto;

public record class MessageLoadingDto(Guid MessageSourceId, ICollection<MessageDto> Messages);