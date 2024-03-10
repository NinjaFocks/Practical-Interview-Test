using Microsoft.Extensions.DependencyInjection;
using Rdessoy_MCMS_Practical_Interview_Test.Data;
using System.Windows;

namespace Rdessoy_MCMS_Practical_Interview_Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        //private readonly string _connectionString = "";

        public App()
        {
            ServiceCollection services = new ServiceCollection();

            ConfigureServices(services);

            serviceProvider = services.BuildServiceProvider();            
        }

        private void ConfigureServices(ServiceCollection services)
        {
            //no longer used with the DbContext
            //services.AddDbContext<HashDbContext>(options =>
            //{
            //    options.UseSqlite(_connectionString);
            //});

            services.AddScoped<IDataSource, DataSource>();

            services.AddSingleton<MainWindow>();            
        }

        public void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
