using System.Collections.Generic;
using System.Linq;

namespace RIB.Extensions.Metadata.Models
{
    public record IrregularTimeSeries : TimeSeries
    {
        public List<List<object>> Values { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            foreach (var property in base.PropertyValues())
                yield return property;
            if (Values.Count > 0)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: $"[{string.Join(";", Values.Select(v => v?.Count > 0 ? $"[{string.Join(";", v)}]" : "[]"))}]",
                    name: "Values",
                    className: Class);
        }
    }
}