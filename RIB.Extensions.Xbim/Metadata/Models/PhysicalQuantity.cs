using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record PhysicalQuantity : IfcClass
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(Name))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Name,
                    name: "Name",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Description))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Description,
                    name: "Description",
                    className: Class);
        }
    }
}