using System;
using System.Collections.Generic;
using System.Linq;

namespace RIB.Extensions.Metadata.Models
{
    public record Person : IfcClass
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(FamilyName))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FamilyName,
                    name: "Family_Name",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(GivenName))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: GivenName,
                    name: "Given_Name",
                    className: Class);
        }
        public override string ToString()
        {
            return string.Join(" ", FamilyName, GivenName);
        }
    }

    public record PostalAddress : IfcClass
    {
        public string Country { get; set; }
        public string InternalLocation { get; set; }
        public string AddressLines { get; set; }
        public string PostalBox { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public string Town { get; set; }

        public override string ToString()
        {
            return string.Join(Environment.NewLine,
                new[]
                {
                    InternalLocation,
                    string.Join(" ", new[]
                    {
                        PostalBox,
                        AddressLines
                    }.Where(p => !string.IsNullOrWhiteSpace(p))),
                    Town,
                    Region,
                    Country,
                    PostalCode
                }.Where(p => !string.IsNullOrWhiteSpace(p)));
        }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(InternalLocation))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: InternalLocation,
                    name: "Internal_Location",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(PostalBox))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PostalBox,
                    name: "Postal_Box",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(AddressLines))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: AddressLines,
                    name: "Address_Lines",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Town))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Town,
                    name: "Town",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Region))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Region,
                    name: "Region",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Country))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Country,
                    name: "Country",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(PostalCode))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PostalCode,
                    name: "Postal_Code",
                    className: Class);
        }
    }
}