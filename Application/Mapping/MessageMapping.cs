using Application.Dto;
using DataAccess.Models;

namespace Application.Mapping;

public static class MessageMapping
{
    public static MessageDto AsDto(this Message message) =>
        new MessageDto(message.Id, message.MessageText, message.MessageSource.Id, message.State, message.Date);
}