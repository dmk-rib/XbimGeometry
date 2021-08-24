using System.Collections.Generic;
using Xbim.Ifc4.Interfaces;

namespace RIB.Extensions.Metadata.Models
{
    public record Project : IfcLocation { }

    public record Site : IfcLocation
    {
        public string LandTitleNumber { get; set; }
        public object RefElevation { get; set; }
        public object RefLatitude { get; set; }
        public object RefLongitude { get; set; }
        public PostalAddress SiteAddress { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            foreach (var propertyValue in base.PropertyValues())
                yield return propertyValue;
            if (!string.IsNullOrWhiteSpace(LandTitleNumber))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: LandTitleNumber,
                    name: "Land_Title_Number",
                    className: Class);
            yield return TypeMapper.ToDisplayPropertyValue(
                value: RefElevation,
                name: "Elevation",
                className: Class);
            yield return TypeMapper.ToDisplayPropertyValue(
                value: RefLatitude,
                name: "Latitude",
                className: Class);
            yield return TypeMapper.ToDisplayPropertyValue(
                value: RefLongitude,
                name: "Longitude",
                className: Class);
            if (SiteAddress != null)
                foreach (var property in SiteAddress.PropertyValues())
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Address:{property.Name}";
                        yield return property;
                    }
        }
    }

    public record Building : IfcLocation
    {
        public object ElevationOfTerrain { get; set; }
        public object ElevationOfRefHeight { get; set; }
        public PostalAddress BuildingAddress { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            foreach (var propertyValue in base.PropertyValues())
                yield return propertyValue;
            yield return TypeMapper.ToDisplayPropertyValue(
                value: ElevationOfTerrain,
                name: "Elevation_Of_Terrain",
                className: Class);
            yield return TypeMapper.ToDisplayPropertyValue(
                value: ElevationOfRefHeight,
                name: "Elevation_Of_RefHeight",
                className: Class);
            if (BuildingAddress != null)
                foreach (var property in BuildingAddress.PropertyValues())
                    if (property != null)
                    {
                        property.GroupClass = Class;
                        property.Name = $"Address:{property.Name}";
                        yield return property;
                    }
        }
    }

    public record BuildingStorey : IfcLocation
    {
        public object GrossFloorArea { get; set; }
        public object TotalHeight { get; set; }
        public double? Elevation { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            foreach (var propertyValue in base.PropertyValues())
                yield return propertyValue;
            yield return TypeMapper.ToDisplayPropertyValue(
                value: GrossFloorArea,
                name: "Gross_Floor_Area",
                className: Class);
            yield return TypeMapper.ToDisplayPropertyValue(
                value: TotalHeight,
                name: "Total_Height",
                className: Class);
            yield return TypeMapper.ToDisplayPropertyValue(
                value: Elevation,
                name: "Elevation",
                className: Class);
        }
    }

    public record Space : IfcLocation
    {
        public object ElevationWithFlooring { get; internal set; }
        public string PredefinedType { get; internal set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            foreach (var propertyValue in base.PropertyValues())
                yield return propertyValue;
            if (PredefinedType != null)
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PredefinedType,
                    name: "Type",
                    className: Class);
            yield return TypeMapper.ToDisplayPropertyValue(
                value: ElevationWithFlooring,
                name: "Elevation_With_Flooring",
                className: Class);
        }
    }

    public record Product : IfcLocation { }

    public record Door : Product
    {
        // public object OverallHeight { get; set; }
        // public object OverallWidth { get; set; }
        public string PredefinedType { get; set; }
        public string OperationType { get; set; }
        public string UserDefinedOperationType { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            foreach (var propertyValue in base.PropertyValues())
                yield return propertyValue;
            // if (OverallHeight != null)
            //     yield return TypeMapper.ToDisplayPropertyValue(
            //         value: OverallHeight,
            //         name: "Overall_Height",
            //         className: Class);
            // if (OverallWidth != null)
            //     yield return TypeMapper.ToDisplayPropertyValue(
            //         value: OverallWidth,
            //         name: "Overall_Width",
            //         className: Class);
            if (!string.IsNullOrWhiteSpace(PredefinedType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PredefinedType,
                    name: "Predefined_Type",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(OperationType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: OperationType,
                    name: "Operation_Type",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(UserDefinedOperationType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: UserDefinedOperationType,
                    name: "User_Defined_Operation_Type",
                    className: Class);
        }
    }

    public record Window : Product
    {
        // public object OverallHeight { get; set; }
        // public object OverallWidth { get; set; }
        public string PredefinedType { get; set; }
        public string PartitioningType { get; set; }
        public string UserDefinedPartitioningType { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            foreach (var propertyValue in base.PropertyValues())
                yield return propertyValue;
            // if (OverallHeight != null)
            //     yield return TypeMapper.ToDisplayPropertyValue(
            //         value: OverallHeight,
            //         name: "Overall_Height",
            //         className: Class);
            // if (OverallWidth != null)
            //     yield return TypeMapper.ToDisplayPropertyValue(
            //         value: OverallWidth,
            //         name: "Overall_Width",
            //         className: Class);
            if (!string.IsNullOrWhiteSpace(PredefinedType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PredefinedType,
                    name: "Predefined_Type",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(PartitioningType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: PartitioningType,
                    name: "Partitioning_Type",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(UserDefinedPartitioningType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: UserDefinedPartitioningType,
                    name: "User_Defined_Operation_Type",
                    className: Class);
        }
    }
}