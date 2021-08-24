using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xbim.Common;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.IO;
using Xbim.ModelGeometry.Scene;

namespace RIB.Extensions
{
    internal class LoggerProgress 
    {
        private int _lastProgress;
        private readonly int _step;
        private readonly string _prefix;
        private readonly Action<string> _log;
        private readonly Action<string, double> _report;

        public LoggerProgress(Action<string> log = null, Action<string, double> report = null,  int step = 10, string prefix = "")
        {
            _step = step;
            _prefix = prefix;
            _log = log;
            _report = report;
        }

        public void Log(int progress, object state)
        {
            if (progress - _lastProgress >= _step)
            {
                _log?.Invoke($"{_prefix} {progress:D2}% parsed. Stage {state}");
                _lastProgress = progress;
            }
        }
        public ReportProgressDelegate Progress()
        {
            return _report == null ? null : (i, o) =>
            {
                if (o != null)
                    _report.Invoke(o.ToString(), i / 100.0);
            };
        }
    }

    public static class XbimStoreExtensions
    {
        public static Xbim3DModelContext CreateContext(this IModel model, 
            Action<string, double> report = null, 
            bool singleThread = true)
        {
            var context = new Xbim3DModelContext(model);
            if (singleThread)
                context.MaxThreads = 1;
            var hasContext = context.CreateContext(new LoggerProgress(report: report).Progress(), adjustWcs: true);
            if (!hasContext)
                throw new InvalidDataContractException("Can't create IFC geometry context");
            return context;
        }

        public static IfcStore OpenIfcStore(this string input,
            Action<string> log = null)
        {
            var model = IfcStore.Open(input,
                accessMode: XbimDBAccess.Read,
                progDelegate: log != null ? new LoggerProgress(log, step: 10).Log : null);
            return model;
        }
        public static IfcStore OpenIfcStore(this Stream input,
            StorageType dataType,
            XbimModelType modelType,
            Action<string> log = null)
        {
            var schemaVersion = XbimSchemaVersion.Unsupported;
            var position = input.Position;
            if (dataType is StorageType.IfcZip or StorageType.StpZip or StorageType.Zip)
                using (var memoryStream = new MemoryStream())
                {
                    input.CopyTo(memoryStream);
                    schemaVersion = XBimSchemaExtensions.GetSchemaVersion(memoryStream, dataType);
                }
            else
                schemaVersion = XBimSchemaExtensions.GetSchemaVersion(input, dataType);
            input.Position = position;
            var model = IfcStore.Open(input, dataType, schemaVersion, modelType, accessMode: XbimDBAccess.Read,
                progDelegate: log != null ? new LoggerProgress(log, step: 10).Log : null);
            return model;
        }
        public static async Task<IfcStore> OpenIfcStoreAsync(this Stream input,
            StorageType dataType,
            XbimModelType modelType,
            Action<string> log = null)
        {
            var schemaVersion = XbimSchemaVersion.Unsupported;
            var position = input.Position;
            if (dataType is StorageType.IfcZip or StorageType.StpZip or StorageType.Zip)
                await using (var memoryStream = new MemoryStream())
                {
                    await input.CopyToAsync(memoryStream);
                    schemaVersion = XBimSchemaExtensions.GetSchemaVersion(memoryStream, dataType);
                }
            else
                schemaVersion = XBimSchemaExtensions.GetSchemaVersion(input, dataType);
            input.Position = position;
            var model = IfcStore.Open(input, dataType, schemaVersion, modelType, 
                accessMode: XbimDBAccess.Read,
                progDelegate: log != null ? new LoggerProgress(log, step: 10).Log : null); 
            return model;
        }
        
    }
}