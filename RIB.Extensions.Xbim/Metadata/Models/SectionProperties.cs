using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record SectionProperties : IfcClass
    {
        public string SectionType { get; set; }
        public ProfileDef StartProfile { get; set; }
        public ProfileDef EndProfile { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(SectionType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: SectionType,
                    name: "Section_Type",
                    className: Class);
            if (StartProfile != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: StartProfile,
                    name: "Start_Profile",
                    className: Class);
            if (EndProfile != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: EndProfile,
                    name: "End_Profile",
                    className: Class);
        }
    }
}