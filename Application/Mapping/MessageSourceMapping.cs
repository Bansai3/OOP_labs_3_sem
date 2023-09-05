using Application.Dto;
using DataAccess.Models;

namespace Application.Mapping;

public static class MessageSourceMapping
{
    public static MessageSourceDto AsDto(this MessageSource messageSource) =>
        new MessageSourceDto(messageSource.Messages.Select(x => x.AsDto()).ToArray(),
            messageSource.Workers.Select(x => x.AsDto()).ToArray(), messageSource.Id, messageSource.Title);
}