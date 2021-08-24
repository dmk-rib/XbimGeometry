using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record PropertyBoundedValue : Property
    {
        public object LowerBoundValue { get; set; }
        public object UpperBoundValue { get; set; }
        public object SetPointValue { get; set; }
        public string Unit { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (LowerBoundValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LowerBoundValue,
                    name: $"{Name}:Lower_Bound_Value",
                    className: Class);
            if (UpperBoundValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: UpperBoundValue,
                    name: $"{Name}:Upper_Bound_Value",
                    className: Class);
            if (SetPointValue != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: SetPointValue,
                    name: $"{Name}:Set_Point_Value",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Unit))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Unit,
                    name: $"{Name}:Unit",
                    className: Class);
        }
    }
}