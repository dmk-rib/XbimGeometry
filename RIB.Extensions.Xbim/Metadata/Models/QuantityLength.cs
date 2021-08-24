using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record QuantityLength : PhysicalQuantity
    {
        public string Formula { get; set; }
        public object LengthValue { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(Name) && LengthValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LengthValue,
                    name: Name,
                    className: Class);
            // if (!string.IsNullOrWhiteSpace(Description))
            //     yield return TypeMapper.ToDisplayPropertyValue(
            //         value: Description,
            //         name: "Description",
            //         className: Class);
            if (!string.IsNullOrWhiteSpace(Formula))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Formula,
                    name: $"{Name}:Formula",
                    className: Class);
        }
    }
}