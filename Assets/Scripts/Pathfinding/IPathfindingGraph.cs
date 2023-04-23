using Assets.Scripts.GOAP;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Pathfinding
{
    public interface IPathfindingGraph<TCoordType> where TCoordType : IEquatable<GoapPrecondition>
    {
		IEnumerable<CoordWithWeight<TCoordType>> Neighbors(CoordWithWeight<TCoordType> current);
		double Heuistic(TCoordType a, TCoordType b);

	}
}
