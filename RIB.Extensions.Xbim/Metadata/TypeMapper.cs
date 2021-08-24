using System.Collections.Generic;
using System.Linq;

namespace RIB.Extensions.Metadata
{
    public static class TypeMapper
    {
        public static string ValidateString(this string value) => string.IsNullOrWhiteSpace(value) ? null : value;
        public static List<TElement> ValidateCollection<TElement>(this IEnumerable<TElement> collection)
        {
            var nonEmpty = collection?.Where(e => e != null).ToList();
            return nonEmpty?.Count > 0 ? nonEmpty : null;
        }
        public static IfcProperty ToDisplayPropertyValue(
            object value, 
            string name, 
            string className, 
            string groupName = null)
        {
            return value == null || value is string str && string.IsNullOrWhiteSpace(str)
                ? null
                : new IfcProperty
                {
                    Name = name,
                    Value = value,
                    Class = className,
                    GroupClass = groupName
                };
        }
    }
}