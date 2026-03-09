using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OfferBuilder.Application.Interfaces;
using OfferBuilder.Application.ViewModels;
using OfferBuilder.Infrastructure.Data;
using OfferBuilder.Infrastructure.Repositories;
using OfferBuilder.Infrastructure.Services;
using Serilog;

namespace OfferBuilder.UI;

public partial class App : System.Windows.Application
{
    public static ServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day).CreateLogger();
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "offerbuilder.db");
        var connectionString = $"Data Source={dbPath}";
        DatabaseInitializer.Initialize(connectionString);
        DemoSeeder.Seed(connectionString);

        var sc = new ServiceCollection();
        sc.AddSingleton<IOfferRepository>(_ => new OfferRepository(connectionString));
        sc.AddSingleton<ITemplateRepository>(_ => new TemplateRepository(connectionString));
        sc.AddSingleton<ICatalogRepository>(_ => new CatalogRepository(connectionString));
        sc.AddSingleton<IPdfRenderService, PdfRenderService>();
        sc.AddSingleton<IPreviewRenderService, PreviewRenderService>();
        sc.AddSingleton<MainViewModel>();
        Services = sc.BuildServiceProvider();

        base.OnStartup(e);
    }
}
