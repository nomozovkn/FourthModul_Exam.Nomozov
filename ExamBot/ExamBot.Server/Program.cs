using ExamBot.Bll.Services;
using ExamBot.Dal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExamBot.Server;

internal class Program
{
    static async Task Main(string[] args)
    {
        var projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(projectDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddScoped<IUserService, UserService>();
        
        serviceCollection.AddSingleton<BotListenerService>();
        serviceCollection.AddSingleton<MainContext>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var botListenerService = serviceProvider.GetRequiredService<BotListenerService>();
        await botListenerService.StartBot();

        Console.ReadKey();
    }
}
