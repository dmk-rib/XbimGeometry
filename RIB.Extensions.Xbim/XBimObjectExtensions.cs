using System;
using System.Collections.Generic;
using System.Linq;
using RIB.Extensions.Metadata;
using RIB.Extensions.Metadata.Models;
using Xbim.Common;
using Xbim.Ifc2x3.Interfaces;
using Xbim.Ifc4.Interfaces;
using IIfcBuilding = Xbim.Ifc4.Interfaces.IIfcBuilding;
using IIfcBuildingStorey = Xbim.Ifc4.Interfaces.IIfcBuildingStorey;
using IIfcDoorLiningProperties = Xbim.Ifc4.Interfaces.IIfcDoorLiningProperties;
using IIfcDoorPanelProperties = Xbim.Ifc4.Interfaces.IIfcDoorPanelProperties;
using IIfcElement = Xbim.Ifc4.Interfaces.IIfcElement;
using IIfcElementQuantity = Xbim.Ifc4.Interfaces.IIfcElementQuantity;
using IIfcGroup = Xbim.Ifc4.Interfaces.IIfcGroup;
using IIfcLocalPlacement = Xbim.Ifc4.Interfaces.IIfcLocalPlacement;
using IIfcObject = Xbim.Ifc4.Interfaces.IIfcObject;
using IIfcObjectDefinition = Xbim.Ifc4.Interfaces.IIfcObjectDefinition;
using IIfcObjectPlacement = Xbim.Ifc4.Interfaces.IIfcObjectPlacement;
using IIfcPermeableCoveringProperties = Xbim.Ifc4.Interfaces.IIfcPermeableCoveringProperties;
using IIfcProduct = Xbim.Ifc4.Interfaces.IIfcProduct;
using IIfcProject = Xbim.Ifc4.Interfaces.IIfcProject;
using IIfcPropertySet = Xbim.Ifc4.Interfaces.IIfcPropertySet;
using IIfcPropertySetDefinition = Xbim.Ifc4.Interfaces.IIfcPropertySetDefinition;
using IIfcPropertySingleValue = Xbim.Ifc4.Interfaces.IIfcPropertySingleValue;
using IIfcQuantityArea = Xbim.Ifc4.Interfaces.IIfcQuantityArea;
using IIfcQuantityVolume = Xbim.Ifc4.Interfaces.IIfcQuantityVolume;
using IIfcReinforcementDefinitionProperties = Xbim.Ifc4.Interfaces.IIfcReinforcementDefinitionProperties;
using IIfcSite = Xbim.Ifc4.Interfaces.IIfcSite;
using IIfcSpace = Xbim.Ifc4.Interfaces.IIfcSpace;
using IIfcSpatialStructureElement = Xbim.Ifc4.Interfaces.IIfcSpatialStructureElement;
using IIfcValue = Xbim.Ifc4.Interfaces.IIfcValue;
using IIfcWindowLiningProperties = Xbim.Ifc4.Interfaces.IIfcWindowLiningProperties;
using IIfcWindowPanelProperties = Xbim.Ifc4.Interfaces.IIfcWindowPanelProperties;

namespace RIB.Extensions
{
    public static class XBimObjectExtensions
    {
        private static IEnumerable<T> IfEmpty<T>(this IEnumerable<T> left, IEnumerable<T> right) => !left.Any() ? right : left;
        private static IIfcValue GetProperty(IIfcObject product, string name)
        {
            //get all relations which can define property and quantity sets
            return product.IsDefinedBy
                //Only consider property sets in this case.
                //Search across all property and quantity sets. You might also want to search in a specific property set
                .SelectMany(r => r.RelatingPropertyDefinition.PropertySetDefinitions)
                //Get all properties from all property sets
                //Only consider property sets in this case.
                .OfType<IIfcPropertySet>()
                //lets only consider single value properties. There are also enumerated properties, 
                //table properties, reference properties, complex properties and other
                //Get all properties from all property sets
                .SelectMany(pSet => pSet.HasProperties)
                //lets make the name comparison more fuzzy. This might not be the best practise
                //lets only consider single value properties. There are also enumerated properties, 
                //table properties, reference properties, complex properties and other
                .OfType<IIfcPropertySingleValue>()
                //only take the first. In reality you should handle this more carefully.
                .FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase) ||
                                     p.Name.ToString().ToLower().Contains(name.ToLower()))?.NominalValue;
        }

        public static IIfcValue GetArea(this IIfcObject product)
        {
            //try to get the value from quantities first
            //get all relations which can define property and quantity sets
            var area = product.IsDefinedBy
                //Search across all property and quantity sets. 
                //You might also want to search in a specific quantity set by name
                .SelectMany(r => r.RelatingPropertyDefinition.PropertySetDefinitions)
                //Only consider quantity sets in this case.
                .OfType<IIfcElementQuantity>()
                //Get all quantities from all quantity sets
                .SelectMany(qSet => qSet.Quantities)
                //We are only interested in areas 
                .OfType<IIfcQuantityArea>()
                //We will take the first one. There might obviously be more than one area properties
                //so you might want to check the name. But we will keep it simple for this example.
                .FirstOrDefault()?
                .AreaValue;
            //try to get the value from properties
            return area ?? GetProperty(product, "Area");
        }
        public static IIfcValue GetVolume(this IIfcProduct product)
        {
            var volume = product.IsDefinedBy
                .SelectMany(r => r.RelatingPropertyDefinition.PropertySetDefinitions)
                .OfType<IIfcElementQuantity>()
                .SelectMany(qSet => qSet.Quantities)
                .OfType<IIfcQuantityVolume>()
                .FirstOrDefault()?.VolumeValue;
            return volume ?? GetProperty(product, "Volume");
        }
        public static IEnumerable<IIfcLocalPlacement> GetLocalPlacements(this IIfcElement element)
        {
            // TODO It seems this localPlacement is centered in the hole. Possibly because the hole is usually a rectangle.
            // TODO To draw the window precidely in the wall we might need to find wall: Opening -> VoidsElements -> RelatingBuildingElement
            // According to http://www.buildingsmart-tech.org/ifc/IFC2x3/TC1/html/ifcproductextension/lexical/ifcopeningelement.htm the 
            // Opening.PlacementRelTo is the same localPlacement as Opening -> VoidsElements -> RelatingBuildingElement
            var voids = element.FillsVoids
                .Select(v => v.RelatingOpeningElement?.ObjectPlacement)
                .Where(o => o != null)
                .ToList();
            var objectPlacements = voids.Count > 0
                ? voids
                : new List<IIfcObjectPlacement> { element.ObjectPlacement };
            var axis = objectPlacements.SelectMany(pl => pl.ReferencedByPlacements);
            return axis;
        }
        public static IEnumerable<PropertySetDefinition> GetPropertySets(this IIfcObject ifcObject)
        {
            //Only consider property sets in this case.
            var sets = ifcObject.IsDefinedBy
                //Search across all property and quantity sets. You might also want to search in a specific property set
                .SelectMany(r => r.RelatingPropertyDefinition.PropertySetDefinitions);
            foreach (var set in sets)
                yield return set.PropertySet();
        }
        public static PropertySetDefinition PropertySet(this IIfcPropertySetDefinition set)
        {
            switch (set)
            {
                case IIfcElectricalBaseProperties ebp:
                    return ebp.ToElectricalBaseProperties();
                case IIfcEnergyProperties ep:
                    return ep.ToEnergyProperties();
                case IIfcFluidFlowProperties ffp:
                    return ffp.ToFluidFlowProperties();
                case IIfcSoundProperties sp:
                    return sp.ToSoundProperties();
                case IIfcSoundValue sv:
                    return sv.ToSoundValue();
                case IIfcSpaceThermalLoadProperties stlp:
                    return stlp.ToSpaceThermalLoadProperties();
                case IIfcServiceLifeFactor slf:
                    return slf.ToServiceLifeFactor();
                case IIfcPropertySet ps:
                    return ps.ToPropertySet();
                case IIfcElementQuantity eq:
                    return eq.ToElementQuantity();
                case IIfcQuantitySet qs:
                    return qs.ToQuantitySet();
                case IIfcPermeableCoveringProperties pcp:
                    return pcp.ToPermeableCoveringProperties();
                case IIfcDoorLiningProperties dlp:
                    return dlp.ToDoorLiningProperties();
                case IIfcDoorPanelProperties dpp:
                    return dpp.ToDoorPanelProperties();
                case IIfcWindowLiningProperties wlp:
                    return wlp.ToWindowLiningProperties();
                case IIfcWindowPanelProperties wpp:
                    return wpp.ToWindowPanelProperties();
                case IIfcReinforcementDefinitionProperties rdp:
                    return rdp.ToReinforcementDefinitionProperties();
                default:
                    return set.ToPropertySetDefinition();
                    //case IIfcPreDefinedPropertySet pdp:
                    //case IIfcPropertySetDefinition psd:
                    //    yield return new BimSet
                    //    {
                    //        Location = set.MakePropertySet<BimLocation>()
                    //    };
                    //    break;
            }
        }
        public static Dictionary<IIfcGroup, IPersistEntity[]> Zones(this IModel model)
        {
            var zones = model.Instances.OfType<IIfcGroup>()
                .SelectMany(z => z.IsGroupedBy)
                .ToDictionary(sb => sb.RelatingGroup, sb => sb.RelatedObjects.OfType<IPersistEntity>().ToArray());
            return zones;
        }
        public static IEnumerable<IPersistEntity> Decompose(this IPersistEntity o)
        {
            if (o is IIfcSpatialStructureElement spatialElement)
            {
                foreach (var node in spatialElement
                    .ContainsElements
                    .SelectMany(rel => rel.RelatedElements))
                    yield return node;
            }
            if (o is IIfcElement element)
            {
                if (element.FillsVoids != null)
                    foreach (var opening in element.FillsVoids.Select(v => v.RelatingOpeningElement))
                        yield return opening;
                if (element.HasOpenings != null)
                    foreach (var opening in element.HasOpenings.Select(v => v.RelatedOpeningElement))
                        yield return opening;

                // if (element.HasProjections != null)
                //     foreach (var addition in element.HasProjections?.Select(p => p.RelatedFeatureElement))
                //         yield return addition;
                // if (element.HasCoverings != null)
                //     foreach (var covering in element.HasCoverings?.SelectMany(c => c.RelatedCoverings))
                //         yield return covering;

            }
            if (o is IIfcObjectDefinition definition)
                foreach (var node in definition.IsDecomposedBy
                    .SelectMany(r => r.RelatedObjects))
                    yield return node;
        }
        public static IEnumerable<IPersistEntity> Decompose(this IPersistEntity o, IModel m) =>
            o switch
            {
                IIfcProject => o.Decompose()
                    .IfEmpty(m.Instances.OfType<IIfcSite>())
                    .IfEmpty(m.Instances.OfType<IIfcBuilding>())
                    .IfEmpty(m.Instances.OfType<IIfcBuildingStorey>())
                    .IfEmpty(m.Instances.OfType<IIfcSpace>()),
                IIfcSite => o.Decompose()
                    .IfEmpty(m.Instances.OfType<IIfcBuilding>())
                    .IfEmpty(m.Instances.OfType<IIfcBuildingStorey>())
                    .IfEmpty(m.Instances.OfType<IIfcSpace>()),
                IIfcBuilding => o.Decompose()
                    .IfEmpty(m.Instances.OfType<IIfcBuildingStorey>())
                    .IfEmpty(m.Instances.OfType<IIfcSpace>()),
                IIfcBuildingStorey => o.Decompose()
                    .IfEmpty(m.Instances.OfType<IIfcSpace>()),
                _ => o.Decompose()
            };
        public static IEnumerable<IPersistEntity> Hierarchy(this IPersistEntity o) => Hierarchy(o, (p, c) => p);
        public static IEnumerable<TResult> Hierarchy<TResult>(this IPersistEntity o,
            Func<IPersistEntity, IEnumerable<IPersistEntity>, TResult> onSelect)
        {
            var level = new Dictionary<IPersistEntity, IEnumerable<IPersistEntity>> { { o, o.Decompose() } };
            while (level.Any())
            {
                var nextLevel = new Dictionary<IPersistEntity, IEnumerable<IPersistEntity>>();
                foreach (var generation in level)
                {
                    foreach (var child in generation.Value)
                        nextLevel[child] = child.Decompose();
                    yield return onSelect(generation.Key, generation.Value);
                }
                level = nextLevel;
            }
        }
        public static double? GetElevation(this IPersistEntity o, double factor = 1)
        {
            switch (o)
            {
                case IIfcProject project:
                    return null;
                case IIfcSite site:
                    var siteElevation = site.RefElevation?.ToValue();
                    return siteElevation != null
                        ? Convert.ToDouble(siteElevation) * factor
                        : default(double?);
                case IIfcBuilding building:
                    var buildingElevation = building.ElevationOfTerrain?.ToValue() ?? building.ElevationOfRefHeight?.ToValue();
                    return buildingElevation != null
                        ? Convert.ToDouble(buildingElevation) * factor
                        : default(double?);
                case IIfcBuildingStorey floor:
                    var floorElevation = floor.Elevation?.ToValue();
                    return floorElevation != null
                        ? Convert.ToDouble(floorElevation) * factor
                        : default(double?);
                case IIfcSpace space:
                    var spaceElevation = space.ElevationWithFlooring?.ToValue();
                    return spaceElevation != null
                        ? Convert.ToDouble(spaceElevation) * factor
                        : default(double?);
                case IIfcProduct product:
                default:
                    return null;
            }
        }
    }
}