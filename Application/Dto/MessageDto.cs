using DataAccess.Models;

namespace Application.Dto;

public record class MessageDto(Guid Id, string MessageText, Guid MessageSourceId, State State, DateTime Date);