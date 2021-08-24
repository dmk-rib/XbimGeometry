using System;
using System.Collections.Generic;
using Xbim.Common;
using Xbim.Common.Metadata;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.UtilityResource;

namespace RIB.Extensions.Metadata.Models
{
    public record NullObjectDefinition : IIfcObjectDefinition
    {
        public const int Id = int.MinValue;

        public IIfcValue this[string property] => throw new NotImplementedException();
        public IEnumerable<IIfcRelAssigns> HasAssignments => throw new NotImplementedException();
        public IEnumerable<IIfcRelNests> Nests => throw new NotImplementedException();
        public IEnumerable<IIfcRelNests> IsNestedBy => throw new NotImplementedException();
        public IEnumerable<IIfcRelDeclares> HasContext => throw new NotImplementedException();
        public IEnumerable<IIfcRelAggregates> IsDecomposedBy => throw new NotImplementedException();
        public IEnumerable<IIfcRelAggregates> Decomposes => throw new NotImplementedException();
        public IEnumerable<IIfcRelAssociates> HasAssociations => throw new NotImplementedException();
        public IIfcMaterialSelect Material => throw new NotImplementedException();
        public IfcGloballyUniqueId GlobalId { get => null; set => throw new NotImplementedException(); }
        public IIfcOwnerHistory OwnerHistory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IfcLabel? Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IfcText? Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int EntityLabel => Id;
        public IModel Model => throw new NotImplementedException();
        public bool Activated => throw new NotImplementedException();
        public ExpressType ExpressType => throw new NotImplementedException();
        public IModel ModelOf => throw new NotImplementedException();

        public void Parse(int propIndex, IPropertyValue value, int[] nested) => throw new NotImplementedException();
    }
}