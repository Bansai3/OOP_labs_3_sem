using Application.Extensions;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationTests;

public class DataBaseConfiguration
{
    public DataBaseConfiguration()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddApplication();
        serviceCollection.AddDbContext<DataBaseContext>(x => x.UseLazyLoadingProxies().UseSqlite("Data Source = DataBase.db"));
        ServiceProvider = serviceCollection.BuildServiceProvider();
        DataBaseContext dataBaseContext = ServiceProvider.GetRequiredService<DataBaseContext>();
        dataBaseContext.Database.EnsureCreated();
    }
    
    public ServiceProvider ServiceProvider { get; private set; }
}