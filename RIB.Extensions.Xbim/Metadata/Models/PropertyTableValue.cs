using System.Collections.Generic;
using System.Linq;

namespace RIB.Extensions.Metadata.Models
{
    public record PropertyTableValue : Property
    {
        public string Expression { get; set; }
        public string CurveInterpolation { get; set; }
        public string DefinedUnit { get; set; }
        public List<object> DefinedValues { get; set; }
        public string DefiningUnit { get; set; }
        public List<object> DefiningValues { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            // if (!string.IsNullOrWhiteSpace(Description))
            //     yield return TypeMapper.ToDisplayPropertyValue(
            //         value: Description,
            //         name: $"{Name}:Description",
            //         className: Class);
            if (!string.IsNullOrWhiteSpace(Expression))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Expression,
                    name: $"{Name}:Expression",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(CurveInterpolation))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: CurveInterpolation,
                    name: $"{Name}:Curve_Interpolation",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(DefinedUnit))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: DefinedUnit,
                    name: $"{Name}:Defined_Unit",
                    className: Class);
            if (DefinedValues?.Count > 0)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: $"[{string.Join(";", DefinedValues.Select(v => v.ToString()))}]",
                    name: $"{Name}:Defined_Values",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(DefiningUnit))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: DefiningUnit,
                    name: $"{Name}:Defining_Unit",
                    className: Class);
            if (DefiningValues?.Count > 0)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: $"[{string.Join(";", DefiningValues.Select(v => v.ToString()))}]",
                    name: $"{Name}:Defining_Values",
                    className: Class);
        }
    }
}