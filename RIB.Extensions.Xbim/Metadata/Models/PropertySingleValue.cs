using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record PropertySingleValue : Property
    {
        public object NominalValue { get; set; }
        public string Unit { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (NominalValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: NominalValue,
                    name: Name,
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Unit))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Unit,
                    name: $"{Name}:Unit",
                    className: Class);
            // if (!string.IsNullOrWhiteSpace(Description))
            //     yield return TypeMapper.ToDisplayPropertyValue(
            //         value: Description,
            //         name: $"{Name}:Description",
            //         className: Class);
        }
    }
}