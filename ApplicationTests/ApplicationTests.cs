using Application.Dto;
using Application.Exceptions;
using Application.Extensions;
using Application.Services.Implementations;
using DataAccess;
using DataAccess.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;


namespace ApplicationTests;

public class ApplicationTests : IClassFixture<DataBaseConfiguration>
{
    private DataBaseConfiguration _configuration;
    private ServiceProvider _serviceProvider;

    public ApplicationTests(DataBaseConfiguration configuration)
    {
        _configuration = configuration;
        _serviceProvider = _configuration.ServiceProvider;
    }

    [Fact]
    async void CreateSupervisorAndWorker()
    {
        DataBaseContext? dataBaseContext = _serviceProvider.GetService<DataBaseContext>();
        if (dataBaseContext is null)
            throw new NullReferenceException();
        var supervisorRegisterService = new SupervisorRegisterService(dataBaseContext);
        var workerRegisterService = new WorkerRegisterService(dataBaseContext);
        SupervisorDto supervisorDto =
            await supervisorRegisterService.CreateSupervisorAsync("Vlad", CancellationToken.None);
        await workerRegisterService.CreateWorkerAsync("Paul", supervisorDto.Id, CancellationToken.None);
        Assert.Equal(2, dataBaseContext.Employees.Count());
        dataBaseContext.Employees.RemoveRange(dataBaseContext.Employees);
        await dataBaseContext.SaveChangesAsync();
    }

    [Fact]
    async void CreateMessageSourceAndMessage()
    {
        DataBaseContext? dataBaseContext = _serviceProvider.GetService<DataBaseContext>();
        if (dataBaseContext is null)
            throw new NullReferenceException();
        var messageSourceService = new MessageSourceService(dataBaseContext);
        var messageService = new MessageService(dataBaseContext);
        MessageSourceDto messageSourceAsync =
            await messageSourceService.CreateMessageSourceAsync("Email", CancellationToken.None);
        MessageDto messageDto =
            await messageService.CreateMessageAsync(messageSourceAsync.Id, "Hello World!", CancellationToken.None);
        MessageSource? messageSource =
            dataBaseContext.MessageSources.SingleOrDefault(ms => ms.Id == messageSourceAsync.Id);
        if (messageSource is null)
            throw new NullReferenceException();
        Assert.Equal(1, dataBaseContext.Messages.Count());
        Assert.Equal(1, dataBaseContext.MessageSources.Count());
        Assert.Equal(1, messageSource.Messages.Count);
        dataBaseContext.MessageSources.RemoveRange(dataBaseContext.MessageSources);
        dataBaseContext.Messages.RemoveRange(dataBaseContext.Messages);
        await dataBaseContext.SaveChangesAsync();
    }

    [Fact]
    async void Login()
    {
        DataBaseContext? dataBaseContext = _serviceProvider.GetService<DataBaseContext>();
        if (dataBaseContext is null)
            throw new NullReferenceException();
        var supervisorRegisterService = new SupervisorRegisterService(dataBaseContext);
        var accountService = new AccountService(dataBaseContext);
        SupervisorDto supervisorDto =
            await supervisorRegisterService.CreateSupervisorAsync("Anton", CancellationToken.None);
        await accountService.CreateAccountAsync("T", "123", supervisorDto.Id, CancellationToken.None);
        var authenticationService = new AuthenticationService(dataBaseContext);
        AccountDto accountDto =
            await authenticationService.CheckAuthenticationAsync("T", "123", CancellationToken.None);
        Employee employee = await dataBaseContext.Employees.GetEntityAsync(supervisorDto.Id, CancellationToken.None);
        if (employee is not Supervisor supervisor)
            throw new EmployeeNotFoundException();
        Assert.Equal(true, supervisor.HaveAccess);
        dataBaseContext.Employees.RemoveRange(dataBaseContext.Employees);
        dataBaseContext.Accounts.RemoveRange(dataBaseContext.Accounts);
        await dataBaseContext.SaveChangesAsync();
    }

    [Fact]
    async Task Logout()
    {
        DataBaseContext? dataBaseContext = _serviceProvider.GetService<DataBaseContext>();
        if (dataBaseContext is null)
            throw new NullReferenceException();
        var supervisorRegisterService = new SupervisorRegisterService(dataBaseContext);
        var accountService = new AccountService(dataBaseContext);
        var logoutService = new LogoutService(dataBaseContext);
        SupervisorDto supervisorDto =
            await supervisorRegisterService.CreateSupervisorAsync("Anton", CancellationToken.None);
        await accountService.CreateAccountAsync("T", "123", supervisorDto.Id, CancellationToken.None);
        var authenticationService = new AuthenticationService(dataBaseContext);
        AccountDto accountDto =
            await authenticationService.CheckAuthenticationAsync("T", "123", CancellationToken.None);
        Employee employee = await dataBaseContext.Employees.GetEntityAsync(supervisorDto.Id, CancellationToken.None); 
        if (employee is not Supervisor supervisor)
            throw new EmployeeNotFoundException();
        Assert.Equal(true, supervisor.HaveAccess);
        logoutService.Logout("T", CancellationToken.None);
        Assert.Equal(false, employee.HaveAccess);
        dataBaseContext.Employees.RemoveRange(dataBaseContext.Employees);
        dataBaseContext.Accounts.RemoveRange(dataBaseContext.Accounts);
        await dataBaseContext.SaveChangesAsync();
    }

    [Fact]
    async Task ConnectMessageSourceAndWorker()
    {
        DataBaseContext? dataBaseContext = _serviceProvider.GetService<DataBaseContext>();
        if (dataBaseContext is null)
            throw new NullReferenceException();
        var supervisorRegisterService = new SupervisorRegisterService(dataBaseContext);
        var workerRegisterService = new WorkerRegisterService(dataBaseContext);
        var messageSourceService = new MessageSourceService(dataBaseContext);
        var addMessageSourceToWorkerService = new AddMessageSourceToWorkerService(dataBaseContext);
        SupervisorDto supervisorDto =
            await supervisorRegisterService.CreateSupervisorAsync("Vlad", CancellationToken.None);
        WorkerDto workerDto = await workerRegisterService.CreateWorkerAsync("Paul", supervisorDto.Id, CancellationToken.None);
        MessageSourceDto messageSourceDto =
            await messageSourceService.CreateMessageSourceAsync("Email", CancellationToken.None);
        await addMessageSourceToWorkerService.AddMessageSourceToWorkerAsync(workerDto.Id, messageSourceDto.Id, CancellationToken.None);
        Employee employee = await dataBaseContext.Employees.GetEntityAsync(workerDto.Id, CancellationToken.None); 
        if (employee is not Worker worker)
            throw new EmployeeNotFoundException();
        Assert.Equal(1, worker.MessageSources.Count);
        dataBaseContext.Employees.RemoveRange(dataBaseContext.Employees);
        dataBaseContext.MessageSources.RemoveRange(dataBaseContext.MessageSources);
        await dataBaseContext.SaveChangesAsync();
    }

    [Fact]
    async Task CreateReport()
    {
        DataBaseContext? dataBaseContext = _serviceProvider.GetService<DataBaseContext>();
        if (dataBaseContext is null)
            throw new NullReferenceException();
        var supervisorRegisterService = new SupervisorRegisterService(dataBaseContext);
        var workerRegisterService = new WorkerRegisterService(dataBaseContext);
        var authenticationService = new AuthenticationService(dataBaseContext);
        var accountService = new AccountService(dataBaseContext);
        var messageSourceService = new MessageSourceService(dataBaseContext);
        var addMessageSourceToWorkerService = new AddMessageSourceToWorkerService(dataBaseContext);
        var messageService = new MessageService(dataBaseContext);
        var messageLoadingService = new MessageLoadingService(dataBaseContext);
        var handleMessageService = new HandleMessagesService(dataBaseContext);
        var reportService = new ReportService(dataBaseContext);
        SupervisorDto supervisorDto =
            await supervisorRegisterService.CreateSupervisorAsync("Vlad", CancellationToken.None);
        WorkerDto workerDto = await workerRegisterService.CreateWorkerAsync("Paul", supervisorDto.Id, CancellationToken.None);
        MessageSourceDto messageSourceDto =
            await messageSourceService.CreateMessageSourceAsync("Email", CancellationToken.None);
        await accountService.CreateAccountAsync("A", "123", supervisorDto.Id, CancellationToken.None);
        await accountService.CreateAccountAsync("B", "123", workerDto.Id, CancellationToken.None);
        await authenticationService.CheckAuthenticationAsync("A", "123", CancellationToken.None);
        await authenticationService.CheckAuthenticationAsync("B", "123", CancellationToken.None);
        await addMessageSourceToWorkerService.AddMessageSourceToWorkerAsync(workerDto.Id, messageSourceDto.Id, CancellationToken.None);
        await messageService.CreateMessageAsync(messageSourceDto.Id, "Hello World!", CancellationToken.None);
        await messageLoadingService.LoadMessagesFromMessageSourceAsync(messageSourceDto.Id, CancellationToken.None);
        await handleMessageService.HandleMessagesAsync(workerDto.Id, CancellationToken.None);
        await reportService.CreateReportAsync(supervisorDto.Id, 2, CancellationToken.None);
        Assert.Equal(1, dataBaseContext.Reports.Count());
        dataBaseContext.Employees.RemoveRange(dataBaseContext.Employees);
        dataBaseContext.MessageSources.RemoveRange(dataBaseContext.MessageSources);
        dataBaseContext.Messages.RemoveRange(dataBaseContext.Messages);
        dataBaseContext.Reports.RemoveRange(dataBaseContext.Reports);
        await dataBaseContext.SaveChangesAsync();
    }
}