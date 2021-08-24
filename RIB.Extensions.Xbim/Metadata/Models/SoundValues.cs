using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record SoundValues : IfcClass
    {
        public TimeSeries SoundLevelTimeSeries { get; set; }
        public double Frequency { get; set; }
        public object SoundLevelSingleValue { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (SoundLevelSingleValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: SoundLevelSingleValue,
                    name: "Sound_Level",
                    className: Class);
            yield return TypeMapper.ToDisplayPropertyValue(
                value: Frequency,
                name: "Frequency",
                className: Class);
            foreach (var property in SoundLevelTimeSeries.PropertyValues())
                if (property != null)
                {
                    property.GroupClass = Class;
                    property.Name = $"Sound_Level_Series:{property.Name}";
                    yield return property;
                }
        }
    }
}