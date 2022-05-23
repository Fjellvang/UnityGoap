using System;

namespace Assets.Scripts.Pathfinding
{
    public struct CoordWithWeight<TCoordType> : IComparable<CoordWithWeight<TCoordType>>
	{
		public TCoordType Coord;
		public double Weight;
		public CoordWithWeight(TCoordType coord, double weight)
		{
			Coord = coord;
			Weight = weight;
		}
		public int CompareTo(CoordWithWeight<TCoordType> other)
		{
			return Weight.CompareTo(other.Weight);
		}
	}
}
