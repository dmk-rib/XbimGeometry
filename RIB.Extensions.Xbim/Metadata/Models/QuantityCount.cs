using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record QuantityCount : PhysicalQuantity
    {
        public object CountValue { get; set; }
        public string Formula { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(Name) && CountValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: CountValue,
                    name: Name,
                    className: Class);
            // if (!string.IsNullOrWhiteSpace(Description))
            //     yield return TypeMapper.ToDisplayPropertyValue(
            //         value: Description,
            //         name: $"{Name}:Description",
            //         className: Class);
            if (!string.IsNullOrWhiteSpace(Formula))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Formula,
                    name: $"{Name}:Formula",
                    className: Class);
        }
    }
}