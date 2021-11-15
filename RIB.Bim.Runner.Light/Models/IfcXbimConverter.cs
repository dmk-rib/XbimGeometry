using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;
using RIB.Extensions;
using Xbim.Common.Exceptions;
using Xbim.Ifc;
using Xbim.IO;
using Xbim.IO.Esent;

namespace RIB.Bim.Runner.Light.Models
{
    public class IfcXbimConverter
    {
        private readonly ILogger _logger;

        private static readonly double Minutes20 = TimeSpan.FromMinutes(20).TotalMilliseconds;

        public void Convert(string inputFile, string outputFile)
        {
            try
            {
                _logger.LogInformation($"Starting conversion of {inputFile}");
                var watch = Stopwatch.StartNew();
                var modelType = IfcStore.ModelProviderFactory.CreateProvider() is EsentModelProvider
                    ? XbimModelType.EsentModel
                    : XbimModelType.MemoryModel;
                using (var content = File.OpenRead(inputFile))
                using (var model = content.OpenIfcStore(StorageType.Ifc, modelType,
                    _logger != null ? msg => _logger.LogInformation(msg) : default(Action<string>)))
                {
                    //switch (options.TessellationQuality)
                    //{
                    //    case Quality.High:
                    model.ModelFactors.DeflectionAngle = 7.0 * Math.PI / 45.0;
                    model.ModelFactors.DeflectionTolerance = 5.0 * model.ModelFactors.OneMilliMetre;
                    //        break;
                    //    case Quality.Normal:
                    //        model.ModelFactors.DeflectionAngle = 2.0 * Math.PI / 9.0;
                    //        model.ModelFactors.DeflectionTolerance = 20.0 * model.ModelFactors.OneMilliMetre;
                    //        break;
                    //    case Quality.Low:
                    //        model.ModelFactors.DeflectionAngle = Math.PI / 3.0;
                    //        model.ModelFactors.DeflectionTolerance = 100.0 * model.ModelFactors.OneMilliMetre;
                    //        break;
                    //}
                    var parseTime = watch.ElapsedMilliseconds;
                    model.CreateContext(_logger != null ? (msg, per) => _logger.LogInformation($"{per} {msg}") : null, true);
                    var wexBimFilename = Path.ChangeExtension(outputFile, "wexBIM");
                    using (var wexBiMfile = File.Create(wexBimFilename))
                    {
                        using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
                        {
                            model.SaveAsWexBim(wexBimBinaryWriter);
                            wexBimBinaryWriter.Close();
                        }
                        wexBiMfile.Close();
                    }
                    
                    watch.Stop();
                    var outputSize = new FileInfo(wexBimFilename).Length;
                    var inputSize = new FileInfo(inputFile).Length;
                    _logger.LogInformation(
                        $"File {inputFile} is successfully converted. " +
                        $"Xbim is generated in {parseTime * 0.001} s, " +
                        $"Xbim is saved in {(watch.ElapsedMilliseconds - parseTime) * 0.001} s, " +
                        $"Total time {watch.ElapsedMilliseconds * 0.001} s. " +
                        $"Output file {wexBimFilename} size: {outputSize / (1024f * 1024f):F} Mb. " +
                        $"Input file size: {inputSize / (1024f * 1024f):F} Mb.");
                    if (outputSize > inputSize)
                        _logger.LogWarning($"KPI Warning: the output file {wexBimFilename} is bigger then input.");
                    else if (outputSize * 2 > inputSize)
                        _logger.LogWarning($"KPI Warning: Pay attention to the size {outputSize / (1024f * 1024f):F} of the output file {wexBimFilename} size.");
                    if (watch.ElapsedMilliseconds > Minutes20)
                        _logger.LogWarning($"KPI Warning: file {inputFile} conversion time {watch.ElapsedMilliseconds * 0.001} is too long.");
                }
            }
            catch (OperationCanceledException canEx)
            {
                _logger.LogInformation($"Pipe Info: File conversion {inputFile} was canceled.\n{canEx}");
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case XbimException xbEx:
                        _logger.LogError($"Pipe Error: One or more problems converting {inputFile}\n{xbEx}");
                        break;
                    case AccessViolationException avEx:
                        _logger.LogError($"Pipe Error: Critical error converting {inputFile}.\n{avEx}");
                        break;
                    default:
                        _logger.LogError($"Pipe Error: Error converting {inputFile}.\n{ex}");
                        break;
                }
                _logger.LogCritical($"Critical Error: {ex}");
            }
        } 

        public IfcXbimConverter(
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<IfcXbimConverter>();
        }
    }
}
