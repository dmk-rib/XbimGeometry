using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;

namespace RIB.Extensions.Metadata
{
    public class IfcProperty : IEquatable<IfcProperty>
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Class { get; set; }
        public string GroupClass { get; set; }

        public override string ToString() => $"{Name} : {Value}";
        public bool Equals(IfcProperty other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) &&
                   Equals(Value, other.Value) &&
                   string.Equals(Class, other.Class, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(GroupClass, other.GroupClass, StringComparison.OrdinalIgnoreCase);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            if (ReferenceEquals(this, obj)) 
                return true;
            if (obj.GetType() != this.GetType()) 
                return false;
            return Equals((IfcProperty)obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Name) : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Class != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Class) : 0);
                hashCode = (hashCode * 397) ^ (GroupClass != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(GroupClass) : 0);
                return hashCode;
            }
        }
    }

    public static class IfcPropertyExtensions
    {
        public static IEnumerable<IfcProperty> GetPropertyValues(this IPersistEntity entity)
        {
            IfcLocation element = entity switch
            {
                IIfcSpace space => space.ToSpace(),
                IIfcBuildingStorey storey => storey.ToBuildingStorey(),
                IIfcBuilding building => building.ToBuilding(),
                IIfcSite site => site.ToSite(),
                IIfcProject project => project.ToProject(),
                IIfcWindow window => window.ToWindow(),
                IIfcDoor door => door.ToDoor(),
                IIfcProduct product => product.ToProduct(),
                _ => null,
            };
            return element?.PropertyValues() ?? Enumerable.Empty<IfcProperty>();
        }
        public static HashSet<TElement> Merge<TElement>(
            this IEnumerable<TElement> left,
            IEnumerable<TElement> right)
        {
            var unique = left != null && right != null
                ? left.Concat(right)
                : left ?? right;
            return unique == null
                ? null
                : new HashSet<TElement>(unique.Where(e => e != null));
        }
    }


    public abstract record IfcClass : IPropertyNavigator
    {
        public string Class { get; set; }

        public static string ToFriendlyName(string name, string longName)
        {
            if (!string.IsNullOrWhiteSpace(longName) && !string.IsNullOrWhiteSpace(name))
            {
                return string.Equals(longName, name, StringComparison.InvariantCultureIgnoreCase)
                    ? longName
                    : $"{longName} ({name})";
            }
            if (!string.IsNullOrWhiteSpace(longName))
                return longName;
            if (!string.IsNullOrWhiteSpace(name))
                return name;
            return null;
        }
        public abstract IEnumerable<IfcProperty> PropertyValues();
    }

    public abstract record IfcLocation : IfcClass
    {
        public string GlobalId { get; set; }
        public string Name { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public string ObjectType { get; set; }

        

        public string FriendlyName()
        {
            var friendly = ToFriendlyName(Name, LongName);
            if (!string.IsNullOrWhiteSpace(friendly)) 
                return friendly;
            if (!string.IsNullOrWhiteSpace(Name))
                return Name;
            if (!string.IsNullOrWhiteSpace(Class))
                return Class;
            return friendly;
        }

        public override IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(GlobalId))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: GlobalId,
                    name: "Global_Id",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Name))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FriendlyName(),
                    name: "Name",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(ObjectType))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: ObjectType,
                    name: "Object_Type",
                    className: Class);
            if (!string.IsNullOrWhiteSpace(Description) && !string.Equals(ObjectType, Description, StringComparison.OrdinalIgnoreCase))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: Description,
                    name: "Description",
                    className: Class);
        }
    }

    public record IfcModel : IPropertyNavigator
    {
        public string FileName { get; set; }
        public double MetersFactor { get; set; }

        public IEnumerable<IfcProperty> PropertyValues()
        {
            if (!string.IsNullOrWhiteSpace(FileName))
                yield return TypeMapper.ToDisplayPropertyValue(
                    value: FileName,
                    name: "Name",
                    className: null);
            yield return TypeMapper.ToDisplayPropertyValue(
                value: MetersFactor,
                name: "Meters_Factor",
                className: null);
        }
    }

    public interface IPropertyNavigator
    {
        IEnumerable<IfcProperty> PropertyValues();
    }
}
