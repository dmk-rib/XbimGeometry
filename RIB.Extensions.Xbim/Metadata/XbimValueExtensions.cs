using System.Linq;
using System.Text;
using RIB.Extensions.Metadata.Models;
using Xbim.Ifc4.Interfaces;
using IIfcTimeSeries = Xbim.Ifc2x3.Interfaces.IIfcTimeSeries;

namespace RIB.Extensions.Metadata
{
    public static class XbimValueExtensions
    {
        public static string ToUnit(this IIfcUnit value) => (value?.FullName).ValidateString();
        public static object ToValue(this IIfcValue value) => value?.Value;
        public static object ToValue(this Xbim.Ifc2x3.Interfaces.IIfcValue value) =>
            value is IIfcValue value4 ? ToValue(value4) : value?.Value;

        //internal static PropertyEnumeration ToPropertyEnumeration(this IIfcPropertyEnumeration value)
        //{
        //    return value == null ? null
        //        : new PropertyEnumeration
        //        {
        //            Name = TypeMapper.TypeMapper.ValidateString(value.Name),
        //            Unit = value.Unit?.ToUnit(),
        //            Values = ValidateCollection(value.EnumerationValues?.Select(v => v.ToValue()))
        //        };
        //}
        public static TimeSeries ToTimeSeries(this IIfcTimeSeries ifcTimeSeries)
        {
            TSeries FromIfc4TimeSeries<TSeries>(Xbim.Ifc4.Interfaces.IIfcTimeSeries time)
                where TSeries : TimeSeries, new()
                => time == null
                    ? null
                    : new TSeries
                    {
                        Class = time.ExpressType?.ToString().ValidateString(),
                        Name = TypeMapper.ValidateString(time.Name),
                        Description = TypeMapper.ValidateString(time.Description),
                        DataOrigin = time.DataOrigin.ToString().ValidateString(),
                        EndTime = TypeMapper.ValidateString(time.EndTime),
                        StartTime = TypeMapper.ValidateString(time.StartTime),
                        TimeSeriesDataType = time.TimeSeriesDataType.ToString().ValidateString(),
                        UserDefinedDataOrigin = TypeMapper.ValidateString(time.UserDefinedDataOrigin),
                        Unit = time.Unit?.ToUnit()
                    };

            if (ifcTimeSeries == null)
                return null;
            switch (ifcTimeSeries)
            {
                case IIfcIrregularTimeSeries irregularTime:
                {
                    var timeSeries = FromIfc4TimeSeries<IrregularTimeSeries>(irregularTime);
                    timeSeries.Values = irregularTime.Values?.Select(v =>
                        v.ListValues?.Select(ToValue).ValidateCollection()).ValidateCollection();
                    return timeSeries;
                }
                case IIfcRegularTimeSeries regularTime:
                {
                    var timeSeries = FromIfc4TimeSeries<RegularTimeSeries>(regularTime);
                    timeSeries.TimeStep = regularTime.TimeStep.ToValue();
                    timeSeries.Values = (regularTime.Values?.Select(v =>
                        (v.ListValues?.Select(l => l.ToValue())).ValidateCollection())).ValidateCollection();
                    return timeSeries;
                }
                case Xbim.Ifc4.Interfaces.IIfcTimeSeries time4:
                    return FromIfc4TimeSeries<TimeSeries>(time4);
                default:
                    return new TimeSeries
                    {
                        Class = ifcTimeSeries.ExpressType?.ToString().ValidateString(),
                        Name = TypeMapper.ValidateString(ifcTimeSeries.Name),
                        Description = TypeMapper.ValidateString(ifcTimeSeries.Description),
                        DataOrigin = ifcTimeSeries.DataOrigin.ToString().ValidateString(),
                        EndTime = (ifcTimeSeries.EndTime?.ToString()).ValidateString(),
                        StartTime = (ifcTimeSeries.StartTime?.ToString()).ValidateString(),
                        TimeSeriesDataType = ifcTimeSeries.TimeSeriesDataType.ToString().ValidateString(),
                        UserDefinedDataOrigin = TypeMapper.ValidateString(ifcTimeSeries.UserDefinedDataOrigin),
                        Unit = (ifcTimeSeries.Unit?.ToString()).ValidateString()
                    };
            }
        }
        public static ReinforcementBarProperties ToReinforcementBarProperties(this IIfcReinforcementBarProperties ifcProperty) =>
            ifcProperty == null
                ? null
                : new ReinforcementBarProperties
                {
                    Class = ifcProperty.ExpressType?.ToString().ValidateString(),
                    BarCount = ifcProperty.BarCount?.ToValue(),
                    BarSurface = ifcProperty.BarSurface.ToString(),
                    EffectiveDepth = ifcProperty.EffectiveDepth?.ToValue(),
                    NominalBarDiameter = ifcProperty.NominalBarDiameter?.ToValue(),
                    SteelGrade = TypeMapper.ValidateString(ifcProperty.SteelGrade),
                    TotalCrossSectionArea = ifcProperty.TotalCrossSectionArea.ToValue()
                };
        public static ProfileDef ToProfileDef(this IIfcProfileDef profile) =>
            profile == null
                ? null
                : new ProfileDef
                {
                    Class = profile.ExpressType?.ToString().ValidateString(),
                    ProfileName = TypeMapper.ValidateString(profile.ProfileName),
                    ProfileType = profile.ProfileType.ToString().ValidateString()
                };
        public static SectionProperties ToSectionProperties(this IIfcSectionProperties ifcSection) =>
            ifcSection == null
                ? null
                : new SectionProperties
                {
                    Class = ifcSection.ExpressType?.ToString().ValidateString(),
                    EndProfile = ifcSection.EndProfile?.ToProfileDef(),
                    StartProfile = ifcSection.StartProfile?.ToProfileDef(),
                    SectionType = ifcSection.SectionType.ToString().ValidateString()
                };
        public static SectionReinforcementProperties ToSectionReinforcementProperties(this IIfcSectionReinforcementProperties ifcProperty)
        {
            if (ifcProperty == null)
                return null;
            return new SectionReinforcementProperties
            {
                Class = ifcProperty.ExpressType?.ToString().ValidateString(),
                LongitudinalStartPosition = ifcProperty.LongitudinalStartPosition.ToValue(),
                LongitudinalEndPosition = ifcProperty.LongitudinalEndPosition.ToValue(),
                ReinforcementRole = ifcProperty.ReinforcementRole.ToString().ValidateString(),
                TransversePosition = ifcProperty.TransversePosition.ToValue(),
                CrossSectionReinforcementDefinitions = TypeMapper.ValidateCollection(
                    ifcProperty.CrossSectionReinforcementDefinitions?.Select(p => p.ToReinforcementBarProperties())),
                SectionDefinition = ifcProperty.SectionDefinition?.ToSectionProperties()
            };
        }
        public static ShapeAspect ToShapeAspect(this IIfcShapeAspect ifcProperty)
        {
            if (ifcProperty == null)
                return null;
            return new ShapeAspect
            {
                Class = ifcProperty.ExpressType?.ToString().ValidateString(),
                Name = TypeMapper.ValidateString(ifcProperty.Name),
                ProductDefinitional = ifcProperty.ProductDefinitional.ToValue(),
                Description = TypeMapper.ValidateString(ifcProperty.Description)
            };
        }
        public static PostalAddress ToPostalAddress(this IIfcPostalAddress ifcPostal)
        {
            if (ifcPostal == null)
                return null;
            static string ValidatedAddress(string addressLine) => addressLine.Replace("Enter address here", "").ValidateString();
            
            return new PostalAddress
            {
                Class = ifcPostal.ExpressType?.ToString().ValidateString(),
                PostalCode = TypeMapper.ValidateString(ifcPostal.PostalCode)?.Trim(),
                PostalBox = TypeMapper.ValidateString(ifcPostal.PostalBox)?.Trim(),
                Country = TypeMapper.ValidateString(ifcPostal.Country)?.Trim(),
                Region = TypeMapper.ValidateString(ifcPostal.Region)?.Trim(),
                Town = TypeMapper.ValidateString(ifcPostal.Town)?.Trim(),
                InternalLocation = TypeMapper.ValidateString(ifcPostal.InternalLocation)?.Trim(),
                AddressLines = ValidatedAddress(ifcPostal.AddressLines?.Select(l => TypeMapper.ValidateString(l))
                    .Aggregate(new StringBuilder(), (s, l) => string.IsNullOrEmpty(l) ? s : s.AppendLine(l.Trim()),
                        s => s.ToString()))
            };
        }

        public static Person ToPerson(this IIfcPerson ifcPerson)
        {
            if (ifcPerson == null)
                return null;
            return new Person
            {
                Class = ifcPerson.ExpressType?.ToString().ValidateString(),
                GivenName = TypeMapper.ValidateString(ifcPerson.GivenName)?.Trim(),
                FamilyName = TypeMapper.ValidateString(ifcPerson.FamilyName)?.Trim(),
            };

        }
    }
}