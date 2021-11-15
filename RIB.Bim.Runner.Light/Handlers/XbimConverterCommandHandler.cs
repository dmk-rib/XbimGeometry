using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using RIB.Bim.Runner.Light.Models;
using Xbim.Common;

namespace RIB.Bim.Runner.Handlers
{
    public class XbimConverterCommandHandler
    {
        private readonly ILogger _logger;
        private readonly IfcXbimConverter _builder;

        public Command Create()
        {
            var cmd = new Command("convert", "Creates Xbim model from IFC file");

            cmd.AddArgument(new Argument<string>("input"));
            cmd.AddArgument(new Argument<string>("output", () => string.Empty));
            //cmd.AddArgument(new Argument<IEnumerable<BundleTarget>>("platforms", () => new[] { BundleTarget.iOS, BundleTarget.Android, BundleTarget.WebGL, BundleTarget.StandaloneWindows64 }));

            cmd.Handler = CommandHandler.Create((string input, string output) =>
            {
                output = string.IsNullOrEmpty(output)
                    ? Path.GetDirectoryName(input) ?? Directory.GetCurrentDirectory()
                    : Path.HasExtension(output)
                        ? Path.GetDirectoryName(output) ?? Directory.GetCurrentDirectory()
                        : output;
                if (!Directory.Exists(output))
                    Directory.CreateDirectory(output);

                if (File.Exists(input))
                {
                    _builder.Convert(input, Path.Combine(output, Path.GetFileName(input)));
                }
                else
                if (Directory.Exists(input))
                {
                    var allFiles = Directory.GetFiles(input, "*.ifc", SearchOption.AllDirectories)
                        .OrderBy(f => new FileInfo(f).Length)
                        .ToArray();
                    for (int i = 0; i < allFiles.Length; i++)
                    {
                        var file = allFiles[i];
                        _logger.LogInformation($"\nBegin {i + 1}/{allFiles.Length} file {file}.\n");
                        _builder.Convert(file, Path.Combine(output, Path.GetFileName(file)));
                        _logger.LogInformation($"\nEnd {i + 1}/{allFiles.Length} file.\n");
                    }
                }
            });
            return cmd;
        }

        
        public XbimConverterCommandHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<XbimConverterCommandHandler>();
            _builder = new IfcXbimConverter(loggerFactory);
            XbimLogging.LoggerFactory = loggerFactory;
        }
    }
}