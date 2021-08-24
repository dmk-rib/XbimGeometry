using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record ProfileDef : IfcClass
    {
        public string ProfileName { get;   set; }
        public string ProfileType { get;   set; }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(ProfileName) && !string.IsNullOrWhiteSpace(ProfileType))
                return $"{ProfileName} ({ProfileType})";
            if (!string.IsNullOrWhiteSpace(ProfileName))
                return ProfileName;
            if (!string.IsNullOrWhiteSpace(ProfileType))
                return ProfileType;
            return string.Empty;
        }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(ProfileName))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ProfileName,
                    name: "Profile_Name",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(ProfileType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ProfileType,
                    name: "Profile_Type",
                    className: Class);
        }
    }
}