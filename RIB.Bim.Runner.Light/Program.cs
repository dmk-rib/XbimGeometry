using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Runtime.ExceptionServices;
using System.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using RIB.Bim.Runner.Handlers;

namespace RIB.Bim.Runner
{
    class Program
    {
        private static ServiceProvider _serviceProvider;
        private static IConfiguration _configurations;

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        static int Main(string[] args)
        {
            // args = new[] { "convert", @"C:\projects\IFCwithErrors", @"C:\projects\IFCwithErrors_Out" };
            // args = new[] { "convert", @"C:\projects\IFCwithErrors_int_real", @"C:\projects\IFCwithErrors_int_real_out" };
            // args = new[] { "convert", @"C:\projects\IFCwithErrors_polyloop", @"C:\projects\IFCwithErrors_polyloop_out" };
            // args = new[] { "convert", @"C:\projects\IFCwithErrors_pins\default_fallback_values", @"C:\projects\IFCwithErrors_pins\default_fallback_values_Out" };
            // args = new[] { "convert", @"C:\projects\IFCwithErrors_pins\seh_exception_occ", @"C:\projects\IFCwithErrors_pins\seh_exception_occ_out" };
            // args = new[] { "convert", @"C:\projects\IFCwithErrors_normals", @"C:\projects\IFCwithErrors_normals_Out" };
            // args = new[] { "convert", @"C:\projects\IFCwithErrors_cases", @"C:\projects\IFCwithErrors_cases_Out" };
            // args = new[] { "convert", @"C:\projects\IFCwithErrors_samples\01_IFCRELCONTAINEDINSPATIALSTRUCTURE", @"C:\projects\IFCwithErrors_Out" };
            // args = new[] { "convert", @"C:\projects\IFCwithErrors_samples\05_IFCMATERIALPROFILE", @"C:\projects\IFCwithErrors_Out" };
            _configurations = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, _configurations);

            _serviceProvider = serviceCollection.BuildServiceProvider();
            var cmd = new CommandLineBuilder()
                .AddCommand(_serviceProvider.GetRequiredService<XbimConverterCommandHandler>().Create())
                .UseDefaults()
                .Build();

            return cmd.Invoke(args);
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(configure =>
            {
                configure.ClearProviders();
                configure.AddSimpleConsole(options =>
                {
                    options.ColorBehavior = LoggerColorBehavior.Enabled;
                    options.UseUtcTimestamp = true;
                });
                configure.AddConfiguration(_configurations.GetSection("Logging"));
                configure.AddLog4Net("log4net.config");
            });
            services.AddTransient<XbimConverterCommandHandler>();
        }
    }
}
