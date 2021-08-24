using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record PropertyReferenceValue : Property
    {
        public string UsageName { get; set; }
        public string PropertyReference { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            // foreach (var property in base.PropertyValues())
            //     yield return property;
            if (!string.IsNullOrWhiteSpace(UsageName))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: UsageName,
                    name: $"{Name}:Usage_Name",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(PropertyReference))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PropertyReference,
                    name: $"{Name}:Reference",
                    className: Class);
        }
    }
}