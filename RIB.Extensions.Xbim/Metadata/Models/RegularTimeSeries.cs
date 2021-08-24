using System.Collections.Generic;
using System.Linq;

namespace RIB.Extensions.Metadata.Models
{
    public record RegularTimeSeries : TimeSeries
    {
        public object TimeStep { get; set; }
        public List<List<object>> Values { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            foreach (var property in base.PropertyValues())
                yield return property;
            if (TimeStep != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: TimeStep,
                    name: "Time_Step",
                    className: Class);
            if (Values.Count > 0)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: $"[{string.Join(";", Values.Select(v => v?.Count > 0 ? $"[{string.Join(";", v)}]" : "[]"))}]",
                    name: "Values",
                    className: Class);
        }
    }
}