using System.Collections.Generic;
using System.Linq;

namespace RIB.Extensions.Metadata.Models
{
    public record PhysicalComplexQuantity : PhysicalQuantity
    {
        public List<PhysicalQuantity> HasQuantities { get; set; }
        public string Usage { get; set; }
        public string Quality { get; set; }
        public string Discrimination { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            // foreach (var property in base.PropertyValues())
            //     yield return property;
            if (!string.IsNullOrWhiteSpace(Usage))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Usage,
                    name: $"{Name}:Usage",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Quality))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Quality,
                    name: $"{Name}:Quality",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Discrimination))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Discrimination,
                    name: $"{Name}:Discrimination",
                    className: Class);
            if (HasQuantities?.Count > 0)
                foreach (var property in HasQuantities.SelectMany(p => p.PropertyValues()))
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = !string.IsNullOrWhiteSpace(Name) ? $"{Name}:{property.Name}" : property.Name;
                        yield return property;
                    }

        }
    }
}