using System.Collections.Generic;
using System.Linq;

namespace RIB.Extensions.Metadata.Models
{
    public record PropertySetDefinition : IfcLocation
    {
        public override IEnumerable<IfcProperty> PropertyValues()
        {
            yield break;
        }

        //public string Name { get; set; }
    }

    public record ReinforcementDefinitionProperties : PropertySetDefinition
    {
        public string DefinitionType { get; set; }
        public List<SectionReinforcementProperties> ReinforcementSectionDefinitions { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (DefinitionType != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: DefinitionType,
                    name: "Type",
                    className: Class);
            if (ReinforcementSectionDefinitions?.Count > 0)
                foreach (var property in ReinforcementSectionDefinitions
                    .SelectMany(cs => cs.PropertyValues()))
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Reinforcement_Section_Definitions:{property.Name}";
                        yield return property;
                    }
        }
    }

    public record WindowPanelProperties : PropertySetDefinition
    {
        public string OperationType { get; set; }
        public string PanelPosition { get; set; }
        public object FrameDepth { get; set; }
        public object FrameThickness { get; set; }
        public ShapeAspect ShapeAspectStyle { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(OperationType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: OperationType,
                    name: "Operation_Type",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(PanelPosition))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PanelPosition,
                    name: "Panel_Position",
                    className: Class);
            if (FrameDepth != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FrameDepth,
                    name: "Frame_Depth",
                    className: Class);
            if (FrameThickness != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FrameThickness,
                    name: "Frame_Thickness",
                    className: Class);
            if (ShapeAspectStyle != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ShapeAspectStyle,
                    name: "Shape_AspectStyle",
                    className: Class);
        }
    }

    public record WindowLiningProperties : PropertySetDefinition
    {
        public object LiningDepth { get; set; }
        public object LiningThickness { get; set; }
        public object TransomThickness { get; set; }
        public object MullionThickness { get; set; }
        public object FirstTransomOffset { get; set; }
        public object SecondTransomOffset { get; set; }
        public object FirstMullionOffset { get; set; }
        public object SecondMullionOffset { get; set; }
        public object LiningOffset { get; set; }
        public object LiningToPanelOffsetX { get; set; }
        public object LiningToPanelOffsetY { get; set; }
        public ShapeAspect ShapeAspectStyle { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (LiningDepth != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LiningDepth,
                    name: "Lining_Depth",
                    className: Class);
            if (LiningThickness != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LiningThickness,
                    name: "Lining_Thickness",
                    className: Class);
            if (TransomThickness != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: TransomThickness,
                    name: "Transom_Thickness",
                    className: Class);
            if (MullionThickness != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: MullionThickness,
                    name: "Mullion_Thickness",
                    className: Class);
            if (FirstTransomOffset != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FirstTransomOffset,
                    name: "First_Transom_Offset",
                    className: Class);
            if (SecondTransomOffset != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: SecondTransomOffset,
                    name: "Second_Transom_Offset",
                    className: Class);
            if (FirstMullionOffset != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FirstMullionOffset,
                    name: "First_Mullion_Offset",
                    className: Class);
            if (SecondMullionOffset != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: SecondMullionOffset,
                    name: "Second_Mullion_Offset",
                    className: Class);
            if (LiningOffset != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LiningOffset,
                    name: "Lining_Offset",
                    className: Class);
            if (LiningToPanelOffsetX != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LiningToPanelOffsetX,
                    name: "Lining_To_Panel_Offset_X",
                    className: Class);
            if (LiningToPanelOffsetY != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LiningToPanelOffsetY,
                    name: "Lining_To_Panel_Offset_Y",
                    className: Class);
            if (ShapeAspectStyle != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ShapeAspectStyle,
                    name: "Shape_AspectStyle",
                    className: Class);
        }
    }

    public record DoorPanelProperties : PropertySetDefinition
    {
        public object PanelDepth { get; set; }
        public object PanelWidth { get; set; }
        public string PanelOperation { get; set; }
        public string PanelPosition { get; set; }
        public ShapeAspect ShapeAspectStyle { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (PanelDepth != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PanelDepth,
                    name: "Panel_Depth",
                    className: Class);
            if (PanelWidth != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PanelWidth,
                    name: "Panel_Width",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(PanelOperation))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PanelOperation,
                    name: "Panel_Operation",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(PanelPosition))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PanelPosition,
                    name: "Panel_Position",
                    className: Class);
            if (ShapeAspectStyle != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ShapeAspectStyle,
                    name: "Shape_AspectStyle",
                    className: Class);
        }
    }

    public record DoorLiningProperties : PropertySetDefinition
    {
        public object LiningDepth { get; set; }
        public object LiningThickness { get; set; }
        public object LiningOffset { get; set; }
        public object LiningToPanelOffsetX { get; set; }
        public object LiningToPanelOffsetY { get; set; }
        public object ThresholdDepth { get; set; }
        public object ThresholdThickness { get; set; }
        public object ThresholdOffset { get; set; }
        public object TransomThickness { get; set; }
        public object TransomOffset { get; set; }
        public object CasingThickness { get; set; }
        public object CasingDepth { get; set; }

        
        public ShapeAspect ShapeAspectStyle { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (LiningDepth != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LiningDepth,
                    name: "Lining_Depth",
                    className: Class);
            if (LiningThickness != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LiningThickness,
                    name: "Lining_Thickness",
                    className: Class);
            if (LiningOffset != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LiningOffset,
                    name: "Lining_Offset",
                    className: Class);
            if (LiningToPanelOffsetX != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LiningToPanelOffsetX,
                    name: "Lining_To_Panel_Offset_X",
                    className: Class);
            if (LiningToPanelOffsetY != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LiningToPanelOffsetY,
                    name: "Lining_To_Panel_Offset_Y",
                    className: Class);
            if (ThresholdDepth != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ThresholdDepth,
                    name: "Threshold_Depth",
                    className: Class);
            if (ThresholdThickness != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ThresholdThickness,
                    name: "Threshold_Thickness",
                    className: Class);
            if (ThresholdOffset != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ThresholdOffset,
                    name: "Threshold_Offset",
                    className: Class);
            if (TransomThickness != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: TransomThickness,
                    name: "Transom_Thickness",
                    className: Class);
            if (TransomOffset != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: TransomOffset,
                    name: "Transom_Offset",
                    className: Class);
            if (CasingThickness != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: CasingThickness,
                    name: "Casing_Thickness",
                    className: Class);
            if (CasingDepth != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: CasingDepth,
                    name: "Casing_Depth",
                    className: Class);
            if (ShapeAspectStyle != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ShapeAspectStyle,
                    name: "Shape_AspectStyle",
                    className: Class);
        }
    }

    public record PermeableCoveringProperties : PropertySetDefinition
    {
        public string OperationType { get; set; }
        public string PanelPosition { get; set; }
        public object FrameDepth { get; set; }
        public object FrameThickness { get; set; }
        public ShapeAspect ShapeAspectStyle { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(OperationType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: OperationType,
                    name: "Operation_Type",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(PanelPosition))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PanelPosition,
                    name: "Panel_Position",
                    className: Class);
            if (FrameDepth != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FrameDepth,
                    name: "Frame_Depth",
                    className: Class);
            if (FrameThickness != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FrameThickness,
                    name: "Frame_Thickness",
                    className: Class);
            if (ShapeAspectStyle != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ShapeAspectStyle,
                    name: "Shape_AspectStyle",
                    className: Class);
        }
    }

    public record QuantitySet : PropertySetDefinition 
    {

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            yield break;
        }
    }

    public record ElementQuantity : QuantitySet
    {
        public string MethodOfMeasurement { get; set; }
        public List<PhysicalQuantity> Quantities { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            foreach (var property in base.PropertyValues())
                yield return property;
            if (!string.IsNullOrWhiteSpace(MethodOfMeasurement))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: MethodOfMeasurement,
                    name: "Method_Of_Measurement",
                    className: Class);
            if (Quantities?.Count > 0)
                foreach (var property in Quantities.SelectMany(p => p.PropertyValues()))
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = !string.IsNullOrWhiteSpace(Name) ? $"{Name}:{property.Name}" : property.Name;
                        yield return property;
                    }
        }
    }

    public record PropertySet : PropertySetDefinition
    {
        public List<Property> HasProperties { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (HasProperties?.Count > 0)
                foreach (var property in HasProperties.SelectMany(p => p.PropertyValues()))
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = !string.IsNullOrWhiteSpace(Name) ? $"{Name}:{property.Name}" : property.Name;
                        yield return property;
                    }
        }
    }

    public record ServiceLifeFactor : PropertySetDefinition
    {
        public object LowerValue { get; set; }
        public object UpperValue { get; set; }
        public object MostUsedValue { get; set; }
        public string PredefinedType { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (LowerValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LowerValue,
                    name: "Lower_Value",
                    className: Class);
            if (UpperValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: UpperValue,
                    name: "Upper_Value",
                    className: Class);
            if (MostUsedValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: MostUsedValue,
                    name: "Most_Used_Value",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(PredefinedType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PredefinedType,
                    name: "Type",
                    className: Class);
        }
    }

    public record SpaceThermalLoadProperties : PropertySetDefinition
    {
        public object ApplicableValueRatio { get; set; }
        public string ThermalLoadSource { get; set; }
        public string PropertySource { get; set; }
        public string SourceDescription { get; set; }
        public object MaximumValue { get; set; }
        public object MinimumValue { get; set; }
        public string UserDefinedThermalLoadSource { get; set; }
        public string UserDefinedPropertySource { get; set; }
        public string ThermalLoadType { get; set; }
        public TimeSeries ThermalLoadTimeSeriesValues { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (ApplicableValueRatio != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ApplicableValueRatio,
                    name: "Applicable_Value_Ratio",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(ThermalLoadSource))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ThermalLoadSource,
                    name: "Thermal_Load_Source",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(PropertySource))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PropertySource,
                    name: "Property_Source",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(SourceDescription))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: SourceDescription,
                    name: "Source_Description",
                    className: Class);
            if (MaximumValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: MaximumValue,
                    name: "Maximum_Value",
                    className: Class);
            if (MinimumValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: MinimumValue,
                    name: "Minimum_Value",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(UserDefinedThermalLoadSource))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: UserDefinedThermalLoadSource,
                    name: "User_Defined_Thermal_Load_Source",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(UserDefinedPropertySource))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: UserDefinedPropertySource,
                    name: "User_Defined_Property_Source",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(ThermalLoadType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ThermalLoadType,
                    name: "Thermal_Load_Type",
                    className: Class);
            if (ThermalLoadTimeSeriesValues != null)
                foreach (var property in ThermalLoadTimeSeriesValues.PropertyValues())
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Thermal_Load_Time_Series_Values:{property.Name}";
                    }
        }
    }

    public record SoundValue : PropertySetDefinition
    {
        public object Frequency { get; set; }
        public object SoundLevelSingleValue { get; set; }
        public TimeSeries SoundLevelTimeSeries { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (Frequency != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Frequency,
                    name: "Frequency",
                    className: Class);
            if (SoundLevelSingleValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: SoundLevelSingleValue,
                    name: "Sound_Level_Single_Value",
                    className: Class);
            if (SoundLevelTimeSeries != null)
                foreach (var property in SoundLevelTimeSeries.PropertyValues())
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Sound_Level_Time_Series:{property.Name}";
                    }
        }
    }

    public record SoundProperties : PropertySetDefinition
    {
        public object IsAttenuating { get; set; }
        public string SoundScale { get; set; }
        public List<SoundValue> SoundValues { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (IsAttenuating != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: IsAttenuating,
                    name: "Is_Attenuating",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(SoundScale))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: SoundScale,
                    name: "Sound_Scale",
                    className: Class);
            if (SoundValues?.Count > 0)
                foreach (var property in SoundValues.SelectMany(p =>p.PropertyValues()))
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Sound_Values:{property.Name}";
                    }
        }
    }

    public record FluidFlowProperties : PropertySetDefinition
    {
        public string Fluid { get; set; }
        public object TemperatureSingleValue { get; set; }
        public object WetBulbTemperatureSingleValue { get; set; }
        public object FlowrateSingleValue { get; set; }
        public object FlowConditionSingleValue { get; set; }
        public object VelocitySingleValue { get; set; }
        public object PressureSingleValue { get; set; }
        public string PropertySource { get; set; }
        public string UserDefinedPropertySource { get; set; }

        public TimeSeries TemperatureTimeSeries { get; set; }
        public TimeSeries WetBulbTemperatureTimeSeries { get; set; }
        public TimeSeries FlowrateTimeSeries { get; set; }
        public TimeSeries FlowConditionTimeSeries { get; set; }
        public TimeSeries VelocityTimeSeries { get; set; }
        public TimeSeries PressureTimeSeries { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(Fluid))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Fluid,
                    name: "Fluid",
                    className: Class);
            if (TemperatureSingleValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: TemperatureSingleValue,
                    name: "Temperature",
                    className: Class);
            if (WetBulbTemperatureSingleValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: WetBulbTemperatureSingleValue,
                    name: "Wet_Bulb_Temperature",
                    className: Class);
            if (FlowrateSingleValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FlowrateSingleValue,
                    name: "Flow-rate",
                    className: Class);
            if (FlowConditionSingleValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FlowConditionSingleValue,
                    name: "Flow_Condition",
                    className: Class);
            if (VelocitySingleValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: VelocitySingleValue,
                    name: "Velocity",
                    className: Class);
            if (PressureSingleValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PressureSingleValue,
                    name: "Pressure",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(PropertySource))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PropertySource,
                    name: "Property_Source",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(UserDefinedPropertySource))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: UserDefinedPropertySource,
                    name: "User_Defined_Property_Source",
                    className: Class);
            if (TemperatureTimeSeries != null)
                foreach (var property in TemperatureTimeSeries.PropertyValues())
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Temperature_Time_Series:{property.Name}";
                    }
            if (WetBulbTemperatureTimeSeries != null)
                foreach (var property in WetBulbTemperatureTimeSeries.PropertyValues())
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Wet_Bulb_Temperature_Time_Series:{property.Name}";
                    }
            if (FlowrateTimeSeries != null)
                foreach (var property in FlowrateTimeSeries.PropertyValues())
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Flowrate_Time_Series:{property.Name}";
                    }
            if (FlowConditionTimeSeries != null)
                foreach (var property in FlowConditionTimeSeries.PropertyValues())
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Flow_Condition_Time_Series:{property.Name}";
                    }
            if (VelocityTimeSeries != null)
                foreach (var property in VelocityTimeSeries.PropertyValues())
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Velocity_Time_Series:{property.Name}";
                    }   
            if (PressureTimeSeries != null)
                foreach (var property in PressureTimeSeries.PropertyValues())
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Pressure_Time_Series:{property.Name}";
                    }
        }
    }

    public record EnergyProperties : PropertySetDefinition
    {
        public string EnergySequence { get; set; }
        public string UserDefinedEnergySequence { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(EnergySequence))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: EnergySequence,
                    name: "Energy_Sequence",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(UserDefinedEnergySequence))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: UserDefinedEnergySequence,
                    name: "User_Defined_Energy_Sequence",
                    className: Class);
        }
    }

    public record ElectricalBaseProperties : PropertySetDefinition
    {
        public string ElectricCurrentType { get; set; }
        public object InputVoltage { get; set; }
        public object InputFrequency { get; set; }
        public object FullLoadCurrent { get; set; }
        public object MinimumCircuitCurrent { get; set; }
        public object MaximumPowerInput { get; set; }
        public object RatedPowerInput { get; set; }
        public long InputPhase { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            yield return TypeMapper.ToDisplayPropertyValue(
                value: InputPhase,
                name: "Input_Phase",
                className: Class);
            if (RatedPowerInput != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: RatedPowerInput,
                    name: "Rated_Power_Input",
                    className: Class);
            if (MaximumPowerInput != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: MaximumPowerInput,
                    name: "Maximum_Power_Input",
                    className: Class);
            if (MinimumCircuitCurrent != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: MinimumCircuitCurrent,
                    name: "Minimum_Circuit_Current",
                    className: Class);
            if (FullLoadCurrent != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FullLoadCurrent,
                    name: "Full_Load_Current",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(ElectricCurrentType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ElectricCurrentType,
                    name: "Electric_Current_Type",
                    className: Class);
            if (InputVoltage != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: InputVoltage,
                    name: "Input_Voltage",
                    className: Class);
            if (InputFrequency != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: InputFrequency,
                    name: "Input_Frequency",
                    className: Class);
        }
    }
}