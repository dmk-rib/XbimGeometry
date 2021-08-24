using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record ReinforcementBarProperties : IfcClass
    {
        public object BarCount { get; set; }
        public string BarSurface { get; set; }
        public object EffectiveDepth { get; set; }
        public object NominalBarDiameter { get; set; }
        public string SteelGrade { get; set; }
        public object TotalCrossSectionArea { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (BarCount != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: BarCount,
                    name: "Bar_Count",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(BarSurface))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: BarSurface,
                    name: "Bar_Surface",
                    className: Class);
            if (EffectiveDepth != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: EffectiveDepth,
                    name: "Effective_Depth",
                    className: Class);
            if (NominalBarDiameter != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: NominalBarDiameter,
                    name: "Nominal_Bar_Diameter",
                    className: Class);
            if (SteelGrade != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: SteelGrade,
                    name: "Steel_Grade",
                    className: Class);
            if (TotalCrossSectionArea != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: TotalCrossSectionArea,
                    name: "Total_Cross_SectionArea",
                    className: Class);
        }
    }
}