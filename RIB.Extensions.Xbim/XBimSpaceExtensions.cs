using System.Linq;
using Xbim.Ifc4.Interfaces;

namespace RIB.Extensions
{
    public static class XBimSpaceExtensions
    {
        public static IIfcBuildingStorey GetFloor(this IIfcSpace space)
        {
            //get all objectified relations which model decomposition by this space
            return space.Decomposes
                //select decomposed objects (these might be either other space or building storey)
                .Select(r => r.RelatingObject)
                //get only storeys
                .OfType<IIfcBuildingStorey>()
                //get the first one
                .FirstOrDefault();
        }
    }
}