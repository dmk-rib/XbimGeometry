using System.Linq;
using RIB.Extensions.Metadata.Models;
using Xbim.Ifc4.Interfaces;

namespace RIB.Extensions.Metadata
{
    public static class XBimIIfcPropertyExtensions
    {
        private static TProperty MakeProperty<TProperty>(this IIfcProperty bim)
            where TProperty : Property, new()
        {
            return bim == null
                ? null
                : new TProperty
                {
                    Class = bim.ExpressType?.ToString().ValidateString(),
                    Description = TypeMapper.ValidateString(bim.Description),
                    Name = TypeMapper.ValidateString(bim.Name),
                };
        }

        public static Property ToProperty(
            this IIfcProperty ifcProperty)
        {
            if (ifcProperty == null)
                return null;
            switch (ifcProperty)
            {
                case IIfcComplexProperty cp:
                    return cp.ToPropertyBoundedValue();
                case IIfcPropertyBoundedValue pbv:
                    return pbv.ToPropertyBoundedValue();
                case IIfcPropertyEnumeratedValue pev:
                    return pev.ToPropertyEnumeratedValue();
                case IIfcPropertyListValue plv:
                    return plv.ToPropertyListValue();
                case IIfcPropertyReferenceValue prv:
                    return prv.ToPropertyReferenceValue();
                case IIfcPropertySingleValue psv:
                    return psv.ToPropertySingleValue();
                case IIfcPropertyTableValue ptv:
                    return ptv.ToPropertyTableValue();
                //case IIfcSimpleProperty sp:
                //case IIfcProperty p:
                //    return ifcProperty.MakeProperty<Property>();
            }
            return ifcProperty.MakeProperty<Property>();
        }

        private static PropertyTableValue ToPropertyTableValue(this IIfcPropertyTableValue ptv)
        {
            var property = ptv.MakeProperty<PropertyTableValue>();
            property.Expression = TypeMapper.ValidateString(ptv.Expression);
            property.CurveInterpolation = ptv.CurveInterpolation?.ToString().ValidateString();
            property.DefinedUnit = ptv.DefinedUnit?.ToUnit();
            property.DefinedValues = ptv.DefinedValues?.Select(v => v.ToValue()).ValidateCollection();
            property.DefiningUnit = ptv.DefiningUnit?.ToUnit();
            property.DefiningValues = ptv.DefiningValues?.Select(v => v.ToValue()).ValidateCollection();
            return property;
        }
        private static PropertySingleValue ToPropertySingleValue(this IIfcPropertySingleValue psv)
        {
            var property = psv.MakeProperty<PropertySingleValue>();
            property.NominalValue = psv.NominalValue?.ToValue();
            property.Unit = psv.Unit?.ToUnit();
            return property;
        }
        private static PropertyReferenceValue ToPropertyReferenceValue(this IIfcPropertyReferenceValue prv)
        {
            var property = prv.MakeProperty<PropertyReferenceValue>();
            property.UsageName = TypeMapper.ValidateString(prv.UsageName);
            //TODO: notimplemented;
            property.PropertyReference = prv.PropertyReference?.ToString().ValidateString();
            return property;
        }
        private static PropertyListValue ToPropertyListValue(this IIfcPropertyListValue plv)
        {
            var property = plv.MakeProperty<PropertyListValue>();
            property.Unit = plv.Unit?.ToUnit();
            property.ListValues = plv.ListValues?.Select(v => v.ToValue()).ValidateCollection();
            return property;
        }
        private static PropertyEnumeratedValue ToPropertyEnumeratedValue(this IIfcPropertyEnumeratedValue pev)
        {
            var property = pev.MakeProperty<PropertyEnumeratedValue>();
            property.EnumerationValues = pev.EnumerationValues?.Select(v => v.ToValue()).ValidateCollection();
           // property.EnumerationReference = pev.EnumerationReference?.ToPropertyEnumeration();
            return property;
        }
        private static PropertyBoundedValue ToPropertyBoundedValue(this IIfcPropertyBoundedValue pbv)
        {
            var property = pbv.MakeProperty<PropertyBoundedValue>();
            property.LowerBoundValue = pbv.LowerBoundValue?.ToValue();
            property.UpperBoundValue = pbv.UpperBoundValue?.ToValue();
            property.SetPointValue = pbv.SetPointValue?.ToValue();
            property.Unit = pbv.Unit?.ToUnit();
            return property;
        }
        private static ComplexProperty ToPropertyBoundedValue(this IIfcComplexProperty cp)
        {
            var property = cp.MakeProperty<ComplexProperty>();
            property.UsageName = TypeMapper.ValidateString(cp.UsageName);
            property.HasProperties = cp.HasProperties?.Select(ToProperty).ValidateCollection();
            return property;
        }
    }
}