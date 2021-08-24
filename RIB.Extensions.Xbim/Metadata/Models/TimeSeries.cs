using System.Collections.Generic;

namespace RIB.Extensions.Metadata.Models
{
    public record TimeSeries : IfcClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TimeSeriesDataType { get; set; }
        public string DataOrigin { get; set; }
        public string UserDefinedDataOrigin { get; set; }
        public string Unit { get; set; }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(Name))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Name,
                    name: "Name",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Description))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Description,
                    name: "Description",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(StartTime))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: StartTime,
                    name: "Start_Time",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(EndTime))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: EndTime,
                    name: "End_Time",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(TimeSeriesDataType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: TimeSeriesDataType,
                    name: "Time_Series_Data_Type",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(DataOrigin))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: DataOrigin,
                    name: "Data_Origin",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(UserDefinedDataOrigin))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: UserDefinedDataOrigin,
                    name: "User_Defined_Data_Origin",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Unit))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Unit,
                    name: "Unit",
                    className: Class);
        }
    }
}