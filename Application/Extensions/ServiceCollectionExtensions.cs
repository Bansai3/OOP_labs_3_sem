using Application.Services;
using Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IAccountService, AccountService>();
        collection.AddScoped<IAddMessageSourceToWorkerService, AddMessageSourceToWorkerService>();
        collection.AddScoped<IAuthenticationService, AuthenticationService>();
        collection.AddScoped<IHandleMessagesService, HandleMessagesService>();
        collection.AddScoped<IMessageLoadingService, MessageLoadingService>();
        collection.AddScoped<IMessageService, MessageService>();
        collection.AddScoped<IMessageSourceService, MessageSourceService>();
        collection.AddScoped<IReportService, ReportService>();
        collection.AddScoped<ISupervisorRegisterService, SupervisorRegisterService>();
        collection.AddScoped<IWorkerRegisterService, WorkerRegisterService>();
        collection.AddScoped<ISameUsersCheckService, SameUsersCheckService>();
        collection.AddScoped<ILogoutService, LogoutService>();
        
        return collection;
    }
}