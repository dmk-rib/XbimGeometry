using System.Collections.Generic;
using System.Linq;

namespace RIB.Extensions.Metadata.Models
{
    public record PropertyListValue : Property
    {
        public string Unit { get; set; }
        public List<object> ListValues { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            // foreach (var property in base.PropertyValues())
            //     yield return property;
            if (!string.IsNullOrWhiteSpace(Unit))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Unit,
                    name: $"{Name}:Unit",
                    className: Class);
            if (ListValues.Count > 0)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: $"[{string.Join(";", ListValues.Select(v => v.ToString()))}]",
                    name: $"{Name}:Values",
                    className: Class);
        }
    }
}