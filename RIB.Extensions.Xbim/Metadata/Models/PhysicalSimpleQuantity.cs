using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record PhysicalSimpleQuantity : PhysicalQuantity
    {
        public string Unit { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            // foreach (var property in base.PropertyValues())
            //     yield return property;
            if (!string.IsNullOrWhiteSpace(Unit))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Unit,
                    name: $"{Name}:Unit",
                    className: Class);
        }
    }
}