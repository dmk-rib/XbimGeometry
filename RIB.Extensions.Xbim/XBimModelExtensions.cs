using RIB.Extensions.Metadata;
using RIB.Extensions.Metadata.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xbim.Common;
using Xbim.Common.Geometry;
using Xbim.Common.Step21;
using Xbim.Common.XbimExtensions;
using Xbim.Ifc4.Interfaces;

namespace RIB.Extensions
{
    public record Model<TNode> where TNode : new()
    {
        public double MeterFactor { get; set; }
        public List<TNode> Nodes { get; set; }
    }

    public record Spatial
    {
        public string GlobalId { get; set; }
        public string ParentGlobalId { get; set; }
        public ushort NodeLevel { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public SpatialTag Tag { get; set; }
        public double? ElevationMeter { get; set; }
        public ICollection<IfcProperty> Properties { get; set; }
    }

    public record SpatialTag
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
    }

    public enum SpatialLevel
    {
        BuildingStorey = 8,
        Space = 16,
        Products = 32
    }

    public static class XBimModelExtensions
    {
        private class IfcObjectDefinitionWrapper
        {
            internal IPersistEntity Object { get; }
            internal IIfcGroup Group { get; }

            public IfcObjectDefinitionWrapper(IPersistEntity obj, IIfcGroup group)
            {
                Object = obj;
                Group = group;
            }
        }
        private static IfcObjectDefinitionWrapper Wrapper(this IPersistEntity obj, IIfcGroup ifcGroup) 
            => new IfcObjectDefinitionWrapper(obj, ifcGroup);
        private static IfcObjectDefinitionWrapper Wrapper(this IPersistEntity obj) 
            => new IfcObjectDefinitionWrapper(obj, null);

        private static IEnumerable<IfcObjectDefinitionWrapper> AppendWrapped(IDictionary<IIfcGroup, IPersistEntity[]> left, IEnumerable<IPersistEntity> right)
        {
            foreach (var l in left.SelectMany(o => o.Value.Select(d => Wrapper(d, o.Key))))
                yield return l;
            foreach (var r in right)
                yield return Wrapper(r);
        }

        public static Model<Spatial> ToSpatialModel(this IModel model, SpatialLevel depth) 
            => ToSpatialModel(model, depth, (s, n, p) => s);
        public static Model<TResult> ToSpatialModel<TResult>(
            this IModel model, 
            SpatialLevel depth, 
            Func<Spatial, IPersistEntity, IPersistEntity, TResult> select)
            where TResult : new()
        {
            var propertyMap = model.Instances.OfType<IIfcRelDefinesByProperties>()
                .SelectMany(p =>
                    p.RelatedObjects.Select(ro =>
                    (
                        Object: (IPersistEntity)ro,
                        PropertySets: p.RelatingPropertyDefinition.PropertySetDefinitions
                    )))
                .GroupBy(p => p.Object, p => p.PropertySets)
                .ToDictionary(p => p.Key, p => new HashSet<IIfcPropertySetDefinition>(p.SelectMany(s => s)));

            var locations = new List<TResult>(propertyMap.Count);
            ushort levelNumber = 0;
            var level = new Dictionary<IPersistEntity, IEnumerable<IfcObjectDefinitionWrapper>> { { new NullObjectDefinition(), model.Instances.OfType<IIfcProject>().Select(Wrapper) } };
            while (level.Any())
            {
                var nextLevel = new Dictionary<IPersistEntity, IEnumerable<IfcObjectDefinitionWrapper>>();
                foreach (var generation in level)
                {
                    var parent = generation.Key;
                    foreach (var child in generation.Value)
                    {
                        var ifcChild = child.Object; 
                        nextLevel[ifcChild] = ifcChild is IIfcProject
                            ? AppendWrapped(model.Zones(), ifcChild.Decompose())
                            : ifcChild.Decompose().Select(Wrapper);
                        IfcLocation locational = null;
                        var propertySet = propertyMap.TryGetValue(ifcChild, out var set) ? set : null;
                        switch (ifcChild)
                        {
                            case IIfcProject project:
                                locational = project.ToProject();
                                break;
                            case IIfcSite site:
                                locational = site.ToSite();
                                break;
                            case IIfcBuilding building:
                                locational = building.ToBuilding();
                                break;
                            case IIfcBuildingStorey floor when depth >= SpatialLevel.BuildingStorey:
                                locational = floor.ToBuildingStorey();
                                break;
                            case IIfcSpace space when depth >= SpatialLevel.Space:
                                locational = space.ToSpace();
                                break;
                            case IIfcProduct product when depth >= SpatialLevel.Products:
                                locational = product.ToProduct();
                                break;
                        }
                        var properties = IfcPropertyExtensions
                            .Merge(propertySet?.SelectMany(s => s?.PropertySet()?.PropertyValues()),
                                   locational?.PropertyValues());
                        if (locational != null)
                        {
                            var elevation = ifcChild.GetElevation() ?? parent?.GetElevation();
                            var result = select(new Spatial
                            {
                                Class = locational.Class,
                                GlobalId = locational.GlobalId,
                                ParentGlobalId = parent is IIfcRoot root ? root?.GlobalId : null,
                                NodeLevel = levelNumber,
                                Name = locational.Name,
                                LongName = locational.LongName,
                                Description = locational.Description,
                                Tag = child.Group == null ? null : new SpatialTag
                                {
                                    Class = child.Group.ExpressType?.ToString().ValidateString(),
                                    Description = child.Group.Description,
                                    Name = child.Group.Name
                                },
                                ElevationMeter = elevation != null
                                    ? model.ModelFactors.LengthToMetresConversionFactor * elevation
                                    : default,
                                Properties = properties?.Count > 0 ? properties : null
                            }, ifcChild, parent);

                            if (result != null)
                                locations.Add(result);
                        }
                    }
                }
                level = nextLevel;
                ++levelNumber;
            }
            var plan = new Model<TResult>
            {
                MeterFactor = model.ModelFactors.LengthToMetresConversionFactor,
                Nodes = locations?.Count > 0 ? locations : null
            };
            return plan;
        }
        public static IfcSurfaceSide GetSurfaceSide(this IModel model, params XbimShapeInstance[] shapeInstances)
        {
            var surfaceStyle = shapeInstances.Select(i => i.StyleLabel)
                .Distinct()
                .Select(s => model.Instances[s])
                .OfType<IIfcSurfaceStyle>()
                .FirstOrDefault();
            return surfaceStyle?.Side ?? IfcSurfaceSide.POSITIVE;
        }
        public static XbimShapeTriangulation ReadShapeTriangulation(this IXbimShapeGeometryData xShape)
        {
            var shapeTriangulation = new BinaryReader(new MemoryStream(xShape.ShapeData))
                .ReadShapeTriangulation();
            return shapeTriangulation;
        }
        public static int GetKey(this IXbimShapeInstanceData instance) =>
            instance.StyleLabel <= 0 ? -instance.IfcTypeId : instance.StyleLabel;
        public static List<short> IfcTypeIgnoreList(this IModel model)
        {
            var toIgnore = new List<short>();
            toIgnore.Add(model.Metadata.ExpressTypeId("IFCOPENINGELEMENT"));
            toIgnore.Add(model.Metadata.ExpressTypeId("IFCPROJECTIONELEMENT"));
            toIgnore.Add(model.Metadata.ExpressTypeId("IFCGRID"));
            toIgnore.Add(model.Metadata.ExpressTypeId("IFCSPACE"));
            if (model.SchemaVersion is XbimSchemaVersion.Ifc4 or XbimSchemaVersion.Ifc4x1)
            {
                toIgnore.Add(model.Metadata.ExpressTypeId("IFCVOIDINGFEATURE"));
                toIgnore.Add(model.Metadata.ExpressTypeId("IFCSURFACEFEATURE"));
            }
            return toIgnore;
        }
    }
}
