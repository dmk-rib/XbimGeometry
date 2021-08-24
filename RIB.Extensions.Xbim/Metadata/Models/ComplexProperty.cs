using System.Collections.Generic;
using System.Linq;

namespace RIB.Extensions.Metadata.Models
{
    public record ComplexProperty : Property
    {
        public string UsageName { get; set; }
        public List<Property> HasProperties { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(UsageName))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: UsageName,
                    name: $"{Name}:Usage_Name",
                    className: Class);
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
}