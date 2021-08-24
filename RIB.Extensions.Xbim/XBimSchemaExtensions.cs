using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using Xbim.Common.Step21;
using Xbim.IO;
using Xbim.IO.Memory;
using Xbim.IO.Xml;

namespace RIB.Extensions
{
    public static class XBimSchemaExtensions
    {
        public static StorageType GetStorageType(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return StorageType.Invalid;
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var actualFormat = StorageType.Invalid;
            if (extension == ".ifczip")
            {
                actualFormat = StorageType.IfcZip;
                actualFormat |= StorageType.Ifc; //the default
            }
            else if (extension == ".ifcxml")
                actualFormat = StorageType.IfcXml;
            else if (extension == ".xbim")
                actualFormat = StorageType.Xbim;
            else if (extension == ".ifc")
                actualFormat = StorageType.Ifc; //the default
            else
                actualFormat = StorageType.Ifc; //the default
            return actualFormat;
        }
        public static XbimSchemaVersion GetSchemaVersion(
            Stream fileStream, 
            StorageType storageType)
        {
            switch (storageType)
            {
                // need to get the header for each step file storage type
                //if it is a zip, xml or ifc text
                case StorageType.Ifc or StorageType.Stp:
                    return MemoryModel.GetStepFileXbimSchemaVersion(fileStream);
                case StorageType.IfcZip or StorageType.StpZip or StorageType.Zip:
                    try
                    {
                        using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                        {
                            var entry = archive.Entries.FirstOrDefault(z => z.Name.IsStepTextFile() || z.Name.IsStepXmlFile());
                            if (entry == null) 
                                throw new FileLoadException("File does not contain a valid model");
                            using (var reader = entry.Open())
                            {
                                if (entry.Name.IsStepTextFile())
                                    return MemoryModel.GetStepFileXbimSchemaVersion(reader);
                                if (entry.Name.IsStepXmlFile())
                                    using (var xml = XmlReader.Create(reader))
                                    {
                                        var schema = XbimXmlReader4.ReadSchemaVersion(xml);
                                        return schema switch
                                        {
                                            XmlSchemaVersion.Ifc2x3 => XbimSchemaVersion.Ifc2X3,
                                            XmlSchemaVersion.Ifc4Add2
                                                or XmlSchemaVersion.Ifc4Add1
                                                or XmlSchemaVersion.Ifc4 => XbimSchemaVersion.Ifc4,
                                            _ => XbimSchemaVersion.Unsupported,
                                        };
                                    }
                                throw new FileLoadException("File does not contain a valid model");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new FileLoadException("File is an invalid zip format", e);
                    }
                case StorageType.IfcXml:
                {
                    XmlSchemaVersion schema;
                    using (var reader = XmlReader.Create(fileStream)) 
                        schema = XbimXmlReader4.ReadSchemaVersion(reader);
                    switch (schema)
                    {
                        case XmlSchemaVersion.Ifc2x3:
                            return XbimSchemaVersion.Ifc2X3;
                        case XmlSchemaVersion.Ifc4Add1:
                        case XmlSchemaVersion.Ifc4Add2:
                        case XmlSchemaVersion.Ifc4:
                            return XbimSchemaVersion.Ifc4;
                        case XmlSchemaVersion.Unknown:
                        default:
                            return XbimSchemaVersion.Unsupported;
                    }
                }
            }
            throw new FileLoadException("File is an invalid model format");
        }
    }
}