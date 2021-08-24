using System.Linq;
using RIB.Extensions.Metadata.Models;
using Xbim.Ifc4.Interfaces;

namespace RIB.Extensions.Metadata
{
    public static class XBimIIfcPhysicalQuantityExtensions
    {
        private static TQuantity MakeProperty<TQuantity>(this IIfcPhysicalQuantity bim)
            where TQuantity : PhysicalQuantity, new()
        {
            return bim == null
                ? null
                : new TQuantity
                {
                    Class = bim.ExpressType?.ToString().ValidateString(),
                    Description = TypeMapper.ValidateString(bim.Description),
                    Name = TypeMapper.ValidateString(bim.Name),
                };
        }

        public static PhysicalQuantity ToQuantity(
            this IIfcPhysicalQuantity bim)
        {
            if (bim == null)
                return null;
            switch (bim)
            {
                case IIfcPhysicalComplexQuantity pcq:
                    return pcq.ToPhysicalComplexQuantity();
                case IIfcQuantityArea qa:
                    return ToQuantityArea(qa);
                case IIfcQuantityCount qc:
                    return ToQuantityCount(qc);
                case IIfcQuantityLength ql:
                    return ToQuantityLength(ql);
                case IIfcQuantityTime qt:
                    return ToQuantityTime(qt);
                case IIfcQuantityVolume qv:
                    return ToQuantityVolume(qv);
                case IIfcQuantityWeight qw:
                    return ToQuantityWeight(qw);
                case IIfcPhysicalSimpleQuantity psq:
                    return ToPhysicalSimpleQuantity(psq);
            }
            return bim.MakeProperty<PhysicalQuantity>();
        }

        private static PhysicalSimpleQuantity ToPhysicalSimpleQuantity(IIfcPhysicalSimpleQuantity psq)
        {
            var properties = psq.MakeProperty<PhysicalSimpleQuantity>();
            properties.Unit = psq.Unit?.ToUnit();
            return properties;
        }
        private static QuantityWeight ToQuantityWeight(IIfcQuantityWeight qw)
        {
            var properties = qw.MakeProperty<QuantityWeight>();
            properties.Formula = TypeMapper.ValidateString(qw.Formula);
            properties.WeightValue = qw.WeightValue.ToValue();
            return properties;
        }
        private static QuantityVolume ToQuantityVolume(IIfcQuantityVolume qv)
        {
            var properties = qv.MakeProperty<QuantityVolume>();
            properties.Formula = TypeMapper.ValidateString(qv.Formula);
            properties.VolumeValue = qv.VolumeValue.ToValue();
            return properties;
        }
        private static QuantityTime ToQuantityTime(IIfcQuantityTime qt)
        {
            var properties = qt.MakeProperty<QuantityTime>();
            properties.Formula = TypeMapper.ValidateString(qt.Formula);
            properties.TimeValue = qt.TimeValue.ToValue();
            return properties;
        }
        private static QuantityLength ToQuantityLength(IIfcQuantityLength ql)
        {
            var properties = ql.MakeProperty<QuantityLength>();
            properties.Formula = TypeMapper.ValidateString(ql.Formula);
            properties.LengthValue = ql.LengthValue.ToValue();
            return properties;
        }
        private static QuantityCount ToQuantityCount(IIfcQuantityCount qc)
        {
            var properties = qc.MakeProperty<QuantityCount>();
            properties.CountValue = qc.CountValue.ToValue();
            properties.Formula = TypeMapper.ValidateString(qc.Formula);
            return properties;
        }
        private static QuantityArea ToQuantityArea(this IIfcQuantityArea qa)
        {
            var properties = qa.MakeProperty<QuantityArea>();
            properties.AreaValue = qa.AreaValue.ToValue();
            properties.Formula = TypeMapper.ValidateString(qa.Formula);
            return properties;
        }
        private static PhysicalComplexQuantity ToPhysicalComplexQuantity(this IIfcPhysicalComplexQuantity pcq)
        {
            var properties = pcq.MakeProperty<PhysicalComplexQuantity>();
            properties.Discrimination = TypeMapper.ValidateString(pcq.Discrimination);
            properties.Quality = TypeMapper.ValidateString(pcq.Quality);
            properties.Usage = TypeMapper.ValidateString(pcq.Usage);
            properties.HasQuantities = pcq.HasQuantities?.Select(ToQuantity).ValidateCollection();
            return properties;
        }
    }
}