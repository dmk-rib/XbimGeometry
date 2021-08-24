using System.Linq;
using RIB.Extensions.Metadata.Models;
using Xbim.Common;
using Xbim.Ifc;
using Xbim.Ifc2x3.Interfaces;
using Xbim.Ifc4.Interfaces;
using IIfcBuilding = Xbim.Ifc4.Interfaces.IIfcBuilding;
using IIfcBuildingStorey = Xbim.Ifc4.Interfaces.IIfcBuildingStorey;
using IIfcDoor = Xbim.Ifc4.Interfaces.IIfcDoor;
using IIfcDoorLiningProperties = Xbim.Ifc4.Interfaces.IIfcDoorLiningProperties;
using IIfcDoorPanelProperties = Xbim.Ifc4.Interfaces.IIfcDoorPanelProperties;
using IIfcDoorStyle = Xbim.Ifc4.Interfaces.IIfcDoorStyle;
using IIfcElementQuantity = Xbim.Ifc4.Interfaces.IIfcElementQuantity;
using IIfcPermeableCoveringProperties = Xbim.Ifc4.Interfaces.IIfcPermeableCoveringProperties;
using IIfcProject = Xbim.Ifc4.Interfaces.IIfcProject;
using IIfcPropertySet = Xbim.Ifc4.Interfaces.IIfcPropertySet;
using IIfcPropertySetDefinition = Xbim.Ifc4.Interfaces.IIfcPropertySetDefinition;
using IIfcReinforcementDefinitionProperties = Xbim.Ifc4.Interfaces.IIfcReinforcementDefinitionProperties;
using IIfcRoot = Xbim.Ifc4.Interfaces.IIfcRoot;
using IIfcObject = Xbim.Ifc4.Interfaces.IIfcObject;
using IIfcSite = Xbim.Ifc4.Interfaces.IIfcSite;
using IIfcSpace = Xbim.Ifc4.Interfaces.IIfcSpace;
using IIfcWindow = Xbim.Ifc4.Interfaces.IIfcWindow;
using IIfcWindowLiningProperties = Xbim.Ifc4.Interfaces.IIfcWindowLiningProperties;
using IIfcWindowPanelProperties = Xbim.Ifc4.Interfaces.IIfcWindowPanelProperties;
using IIfcWindowStyle = Xbim.Ifc4.Interfaces.IIfcWindowStyle;

namespace RIB.Extensions.Metadata
{
    public static class PropertySetDefinitionExtensions
    {
        private static TSet ToPropertySetDefinitionIfc3<TSet>(
            this Xbim.Ifc2x3.Interfaces.IIfcRoot bim)
            where TSet : IfcLocation, new() =>
            bim is IIfcPropertySetDefinition bim4 ? ToPropertySetDefinition<TSet>(bim4) :
            bim == null ? null : new TSet
            {
                GlobalId = TypeMapper.ValidateString(bim.GlobalId),
                Description = TypeMapper.ValidateString(bim.Description),
                Class = bim.ExpressType?.ToString().ValidateString(),
                Name = TypeMapper.ValidateString(bim.Name)
            };
        private static TSet ToPropertySetDefinition<TSet>(this IIfcRoot bim)
            where TSet : IfcLocation, new() =>
            bim == null
                ? null
                : new TSet
                {
                    GlobalId = TypeMapper.ValidateString(bim.GlobalId),
                    Description = TypeMapper.ValidateString(bim.Description),
                    Class = bim.ExpressType?.ToString().ValidateString(),
                    Name = TypeMapper.ValidateString(bim.Name),
                    ObjectType = bim is IIfcObject obj ? TypeMapper.ValidateString(obj.ObjectType) : null,
                };

        public static PropertySetDefinition ToPropertySetDefinition(this IIfcRoot bim) => 
            bim.ToPropertySetDefinition<PropertySetDefinition>();
        public static ReinforcementDefinitionProperties ToReinforcementDefinitionProperties(
            this IIfcReinforcementDefinitionProperties bim)
        {
            var set = bim.ToPropertySetDefinition<ReinforcementDefinitionProperties>();
            if (set == null)
                return null;
            set.DefinitionType = TypeMapper.ValidateString(bim.DefinitionType);
            set.ReinforcementSectionDefinitions =
                bim.ReinforcementSectionDefinitions?
                    .Select(p => p.ToSectionReinforcementProperties())
                    .ValidateCollection();
            return set;
        }
        public static WindowPanelProperties ToWindowPanelProperties(
            this IIfcWindowPanelProperties bim)
        {
            var set = bim.ToPropertySetDefinition<WindowPanelProperties>();
            if (set == null)
                return null;
            set.OperationType = bim.OperationType.ToString().ValidateString();
            set.PanelPosition = bim.PanelPosition.ToString().ValidateString();
            set.FrameDepth = bim.FrameDepth?.ToValue();
            set.FrameThickness = bim.FrameThickness?.ToValue();
            set.ShapeAspectStyle = bim.ShapeAspectStyle?.ToShapeAspect();
            return set;
        }
        public static WindowLiningProperties ToWindowLiningProperties(
            this IIfcWindowLiningProperties bim)
        {
            var set = bim.ToPropertySetDefinition<WindowLiningProperties>();
            if (set == null)
                return null;
            set.LiningDepth = bim.LiningDepth?.ToValue();
            set.LiningThickness = bim.LiningThickness?.ToValue();
            set.TransomThickness = bim.TransomThickness?.ToValue();
            set.MullionThickness = bim.MullionThickness?.ToValue();
            set.FirstTransomOffset = bim.FirstTransomOffset?.ToValue();
            set.SecondTransomOffset = bim.SecondTransomOffset?.ToValue();
            set.FirstMullionOffset = bim.FirstMullionOffset?.ToValue();
            set.SecondMullionOffset = bim.SecondMullionOffset?.ToValue();
            set.ShapeAspectStyle = bim.ShapeAspectStyle?.ToShapeAspect();
            set.LiningOffset = bim.LiningOffset?.ToValue();
            set.LiningToPanelOffsetX = bim.LiningToPanelOffsetX?.ToValue();
            set.LiningToPanelOffsetY = bim.LiningToPanelOffsetY?.ToValue();
            return set;
        }
        public static DoorPanelProperties ToDoorPanelProperties(
            this IIfcDoorPanelProperties bim)
        {
            var set = bim.ToPropertySetDefinition<DoorPanelProperties>();
            if (set == null)
                return null;
            set.ShapeAspectStyle = bim.ShapeAspectStyle?.ToShapeAspect();
            set.PanelDepth = bim.PanelDepth?.ToValue();
            set.PanelWidth = bim.PanelWidth?.ToValue();
            set.PanelOperation = bim.PanelOperation.ToString().ValidateString();
            set.PanelPosition = bim.PanelPosition.ToString().ValidateString();
            return set;
        }
        public static DoorLiningProperties ToDoorLiningProperties(
            this IIfcDoorLiningProperties bim)
        {
            var set = bim.ToPropertySetDefinition<DoorLiningProperties>();
            if (set == null)
                return null;
            set.LiningDepth = bim.LiningDepth?.ToValue();
            set.LiningThickness = bim.LiningThickness?.ToValue();
            set.ThresholdDepth = bim.ThresholdDepth?.ToValue();
            set.ThresholdThickness = bim.ThresholdThickness?.ToValue();
            set.TransomThickness = bim.TransomThickness?.ToValue();
            set.TransomOffset = bim.TransomOffset?.ToValue();
            set.LiningOffset = bim.LiningOffset?.ToValue();
            set.ThresholdOffset = bim.ThresholdOffset?.ToValue();
            set.CasingThickness = bim.CasingThickness?.ToValue();
            set.CasingDepth = bim.CasingDepth?.ToValue();
            set.ShapeAspectStyle = bim.ShapeAspectStyle?.ToShapeAspect();
            set.LiningToPanelOffsetX = bim.LiningToPanelOffsetX?.ToValue();
            set.LiningToPanelOffsetY = bim.LiningToPanelOffsetY?.ToValue();
            return set;
        }
        public static PermeableCoveringProperties ToPermeableCoveringProperties(
            this IIfcPermeableCoveringProperties bim)
        {
            var set = bim.ToPropertySetDefinition<PermeableCoveringProperties>();
            if (set == null)
                return null;
            set.OperationType = bim.OperationType.ToString().ValidateString();
            set.PanelPosition = bim.PanelPosition.ToString().ValidateString();
            set.FrameDepth = bim.FrameDepth?.ToValue();
            set.FrameThickness = bim.FrameThickness?.ToValue();
            set.ShapeAspectStyle = bim.ShapeAspectStyle?.ToShapeAspect();
            return set;
        }
        public static QuantitySet ToQuantitySet(
            this IIfcQuantitySet bim)
        {
            var set = bim.ToPropertySetDefinition<QuantitySet>();
            return set;
        }
        public static ElementQuantity ToElementQuantity(
            this IIfcElementQuantity bim)
        {
            var set = bim.ToPropertySetDefinition<ElementQuantity>();
            if (set == null)
                return null;
            set.MethodOfMeasurement = TypeMapper.ValidateString(bim.MethodOfMeasurement);
            set.Quantities = bim.Quantities?.Select(p => p.ToQuantity()).ValidateCollection();
            return set;
        }
        public static PropertySet ToPropertySet(
            this IIfcPropertySet bim)
        {
            var set = bim.ToPropertySetDefinition<PropertySet>();
            if (set == null)
                return null;
            set.HasProperties = bim.HasProperties?.Select(p => p.ToProperty()).ValidateCollection();
            return set;
        }
        public static ServiceLifeFactor ToServiceLifeFactor(
            this IIfcServiceLifeFactor bim)
        {
            var set = bim.ToPropertySetDefinitionIfc3<ServiceLifeFactor>();
            if (set == null)
                return null;
            set.LowerValue = bim.LowerValue?.ToValue();
            set.MostUsedValue = bim.MostUsedValue?.ToValue();
            set.UpperValue = bim.UpperValue?.ToValue();
            set.PredefinedType = bim.PredefinedType.ToString().ValidateString();
            return set;
        }
        public static SpaceThermalLoadProperties ToSpaceThermalLoadProperties(
            this IIfcSpaceThermalLoadProperties bim)
        {
            var set = bim.ToPropertySetDefinitionIfc3<SpaceThermalLoadProperties>();
            if (set == null)
                return null;
            set.ApplicableValueRatio = bim.ApplicableValueRatio?.ToIfc4()?.ToValue();
            set.ThermalLoadSource = bim.ThermalLoadSource.ToString().ValidateString();
            set.PropertySource = bim.PropertySource.ToString().ValidateString();
            set.SourceDescription = TypeMapper.ValidateString(bim.SourceDescription);
            set.MaximumValue = bim.MaximumValue.ToIfc4()?.ToValue();
            set.MinimumValue = bim.MinimumValue?.ToIfc4()?.ToValue();
            set.ThermalLoadTimeSeriesValues = bim.ThermalLoadTimeSeriesValues?.ToTimeSeries();
            set.UserDefinedThermalLoadSource = TypeMapper.ValidateString(bim.UserDefinedThermalLoadSource);
            set.UserDefinedPropertySource = TypeMapper.ValidateString(bim.UserDefinedPropertySource);
            set.ThermalLoadType = bim.ThermalLoadType.ToString().ValidateString();
            return set;
        }
        public static SoundValue ToSoundValue(
            this IIfcSoundValue bim)
        {
            var set = bim.ToPropertySetDefinitionIfc3<SoundValue>();
            if (set == null)
                return null;
            set.SoundLevelTimeSeries = bim.SoundLevelTimeSeries?.ToTimeSeries();
            set.Frequency = bim.Frequency.ToIfc4()?.ToValue();
            set.SoundLevelSingleValue = bim.SoundLevelSingleValue?.ToValue();
            return set;
        }
        public static SoundProperties ToSoundProperties(
            this IIfcSoundProperties bim)
        {
            var set = bim.ToPropertySetDefinitionIfc3<SoundProperties>();
            if (set == null)
                return null;
            set.IsAttenuating = bim.IsAttenuating.ToIfc4()?.ToValue();
            set.SoundScale = bim.SoundScale?.ToString().ValidateString();
            set.SoundValues = bim.SoundValues?.Select(s => s.ToSoundValue()).ValidateCollection();
            return set;
        }
        public static FluidFlowProperties ToFluidFlowProperties(
            this IIfcFluidFlowProperties bim)
        {
            var set = bim.ToPropertySetDefinitionIfc3<FluidFlowProperties>();
            if (set == null)
                return null;
            set.PropertySource = bim.PropertySource.ToString().ValidateString();
            set.FlowConditionTimeSeries = bim.FlowConditionTimeSeries?.ToTimeSeries();
            set.VelocityTimeSeries = bim.VelocityTimeSeries?.ToTimeSeries();
            set.FlowrateTimeSeries = bim.FlowrateTimeSeries?.ToTimeSeries();
            set.Fluid = TypeMapper.ValidateString(bim.Fluid?.Name);
            set.PressureTimeSeries = bim.PressureTimeSeries?.ToTimeSeries();
            set.UserDefinedPropertySource = TypeMapper.ValidateString(bim.UserDefinedPropertySource);
            set.TemperatureSingleValue = bim.TemperatureSingleValue?.ToIfc4().ToValue();
            set.WetBulbTemperatureSingleValue = bim.WetBulbTemperatureSingleValue?.ToIfc4().ToValue();
            set.WetBulbTemperatureTimeSeries = bim.WetBulbTemperatureTimeSeries?.ToTimeSeries();
            set.TemperatureTimeSeries = bim.TemperatureTimeSeries?.ToTimeSeries();
            set.FlowrateSingleValue = bim.FlowrateSingleValue?.ToValue();
            set.FlowConditionSingleValue = bim.FlowConditionSingleValue?.ToIfc4()?.ToValue();
            set.VelocitySingleValue = bim.VelocitySingleValue?.ToIfc4()?.ToValue();
            set.PressureSingleValue = bim.PressureSingleValue?.ToIfc4()?.ToValue();
            return set;
        }
        public static EnergyProperties ToEnergyProperties(
            this IIfcEnergyProperties bim)
        {
            var set = bim.ToPropertySetDefinitionIfc3<EnergyProperties>();
            if (set == null)
                return null;
            set.EnergySequence = bim.EnergySequence?.ToString().ValidateString();
            set.UserDefinedEnergySequence = TypeMapper.ValidateString(bim.UserDefinedEnergySequence);
            return set;
        }
        public static ElectricalBaseProperties ToElectricalBaseProperties(
            this IIfcElectricalBaseProperties bim)
        {
            var set = bim.ToPropertySetDefinitionIfc3<ElectricalBaseProperties>();
            if (set == null)
                return null;
            set.ElectricCurrentType = bim.ElectricCurrentType?.ToString().ValidateString();
            set.InputVoltage = bim.InputVoltage.ToIfc4()?.ToValue();
            set.InputFrequency = bim.InputFrequency.ToIfc4()?.ToValue();
            set.FullLoadCurrent = bim.FullLoadCurrent?.ToIfc4()?.ToValue();
            set.MinimumCircuitCurrent = bim.MinimumCircuitCurrent?.ToIfc4()?.ToValue();
            set.MaximumPowerInput = bim.MaximumPowerInput?.ToIfc4()?.ToValue();
            set.RatedPowerInput = bim.RatedPowerInput?.ToIfc4()?.ToValue();
            set.InputPhase = bim.InputPhase;
            return set;
        }

        public static Site ToSite(this IIfcSite ifcSite)
        {
            var set = ifcSite.ToPropertySetDefinition<Site>();
            if (set == null)
                return null;
            set.LongName = ifcSite.LongName;
            set.LandTitleNumber = TypeMapper.ValidateString(ifcSite.LandTitleNumber);
            set.RefElevation = ifcSite.RefElevation?.ToValue();
            set.RefLatitude = ifcSite.RefLatitude?.AsDouble;
            set.RefLongitude = ifcSite.RefLongitude?.AsDouble;
            set.SiteAddress = ifcSite.SiteAddress?.ToPostalAddress();
            return set;
        }
        public static Building ToBuilding(this IIfcBuilding ifcBuilding)
        {
            var set = ifcBuilding.ToPropertySetDefinition<Building>();
            if (set == null)
                return null;
            set.LongName = ifcBuilding.LongName;
            set.ElevationOfTerrain = ifcBuilding.ElevationOfTerrain?.ToValue();
            set.ElevationOfRefHeight = ifcBuilding.ElevationOfRefHeight?.ToValue();
            set.BuildingAddress = ifcBuilding.BuildingAddress?.ToPostalAddress();
            return set;
            
        }
        public static BuildingStorey ToBuildingStorey(this IIfcBuildingStorey ifcBuildingStorey)
        {
            var set = ifcBuildingStorey.ToPropertySetDefinition<BuildingStorey>();
            if (set == null)
                return null;
            set.Elevation = ifcBuildingStorey.Elevation?.ToValue() as double?;
            set.LongName = ifcBuildingStorey.LongName;
            set.GrossFloorArea = ifcBuildingStorey.GrossFloorArea?.ToValue();
            set.TotalHeight = ifcBuildingStorey.TotalHeight?.ToValue();
            return set;
        }
        public static Space ToSpace(this IIfcSpace ifcSpace)
        {
            var set = ifcSpace.ToPropertySetDefinition<Space>();
            if (set == null)
                return null; 
            set.LongName = ifcSpace.LongName;
            set.ElevationWithFlooring = ifcSpace.ElevationWithFlooring?.ToValue();
            set.PredefinedType = ifcSpace.PredefinedType?.ToString().ValidateString();
            return set;
        }
        public static Project ToProject(this IIfcProject bim)
        {
            var set = bim?.ToPropertySetDefinition<Project>();
            if (set == null)
                return null;
            set.LongName = bim.LongName;
            return set;
        }

        public static IfcModel ToModel(this IfcStore model)
        {
            return new()
            {
                MetersFactor = model.ModelFactors.LengthToMetresConversionFactor,
                FileName = model.FileName
            };
        }
        public static Product ToProduct(this IIfcRoot bim)
        {
            var set = bim?.ToPropertySetDefinition<Product>();
            if (set == null)
                return null;
            set.LongName = bim is IIfcSpatialElement spatial ? spatial.LongName : null;
            return set;
        }
        public static Window ToWindow(this IIfcWindow bim)
        {
            var set = bim?.ToPropertySetDefinition<Window>();
            if (set == null)
                return null;
            set.PartitioningType = bim.PartitioningType?.ToString() ?? bim.IsTypedBy
                .Select(t => t.RelatingType)
                .OfType<IIfcWindowStyle>()
                .Select(t => t.OperationType.ToString())
                .FirstOrDefault();
            set.PredefinedType = bim.PredefinedType?.ToString();
            set.UserDefinedPartitioningType = TypeMapper.ValidateString(bim.UserDefinedPartitioningType);
            return set;
        }
        public static Door ToDoor(this IIfcDoor bim)
        {
            var set = bim?.ToPropertySetDefinition<Door>();
            if (set == null)
                return null;
            set.OperationType = bim.OperationType?.ToString() ?? bim.IsTypedBy
                .Select(t => t.RelatingType)
                .OfType<IIfcDoorStyle>()
                .Select(t => t.OperationType.ToString())
                .FirstOrDefault();
            set.PredefinedType = bim.PredefinedType?.ToString();
            set.UserDefinedOperationType = TypeMapper.ValidateString(bim.UserDefinedOperationType);
            return set;
        }
    }
}