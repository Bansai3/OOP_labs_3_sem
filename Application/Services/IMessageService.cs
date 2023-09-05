using Application.Dto;
using DataAccess.Models;

namespace Application.Services;

public interface IMessageService
{
    Task<MessageDto> CreateMessageAsync(Guid messageSourceId, string text, CancellationToken cancellationToken);
}