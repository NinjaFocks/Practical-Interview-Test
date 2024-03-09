using Microsoft.Extensions.DependencyInjection;
using Rdessoy_MCMS_Practical_Interview_Test.Data;
using System.Windows;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rdessoy_MCMS_Practical_Interview_Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        private readonly string _connectionString = "Data Source=\"C:\\Users\\rdess\\Downloads\\MCMS Practical Test (C#)\\MCMS Practical Test\\hashes.sqlite\"";

        public App()
        {
            ServiceCollection services = new ServiceCollection();

            ConfigureDataServices(services);

            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureDataServices(ServiceCollection services)
        {
            services.AddDbContext<HashDbContext>(options =>
            {
                options.UseSqlite(_connectionString);
            });

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
