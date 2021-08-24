using System.Collections.Generic;
using System.Linq;

namespace RIB.Extensions.Metadata.Models
{
    public record PropertyEnumeratedValue : Property
    {
        public List<object> EnumerationValues { get; set; }
        //public PropertyEnumeration EnumerationReference { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            // foreach (var property in base.PropertyValues())
            //     yield return property;
            if (EnumerationValues.Count > 0)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: $"[{string.Join(";", EnumerationValues.Select(v => v.ToString()))}]",
                    name: $"{Name}:Values",
                    className: Class);
        }
    }
}