using System.Collections.Generic;
using System.Linq;

namespace RIB.Extensions.Metadata.Models
{
    public record SectionReinforcementProperties : IfcClass
    {

        public object LongitudinalStartPosition { get;  set; }
        public object LongitudinalEndPosition { get;  set; }
        public string ReinforcementRole { get;  set; }
        public object TransversePosition { get;  set; }
        public List<ReinforcementBarProperties> CrossSectionReinforcementDefinitions { get;  set; }
        public SectionProperties SectionDefinition { get;   set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (LongitudinalStartPosition != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LongitudinalStartPosition, 
                    name: "Longitudinal_Start_Position",
                    className: Class);
            if (LongitudinalEndPosition != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LongitudinalEndPosition,
                    name: "Longitudinal_End_Position",
                    className: Class);
            if (ReinforcementRole != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ReinforcementRole,
                    name: "Reinforcement_Role",
                    className: Class);
            if (TransversePosition != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: TransversePosition,
                    name: "Transverse_Position",
                    className: Class);
            if (SectionDefinition != null)
                foreach (var property in SectionDefinition.PropertyValues())
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Section_Definition:{property.Name}";
                        yield return property;
                    }

            if (CrossSectionReinforcementDefinitions?.Count > 0)
                foreach (var property in CrossSectionReinforcementDefinitions.SelectMany(cs => cs.PropertyValues()))
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Cross_Section_Reinforcement_Definitions:{property.Name}";
                        yield return property;
                    }

        }
    }
}