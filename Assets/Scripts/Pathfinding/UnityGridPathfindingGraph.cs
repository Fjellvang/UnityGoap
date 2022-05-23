using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Pathfinding
{
    public class UnityGridPathfindingGraph : IPathfindingGraph<Vector3Int>
    {
        private readonly Tilemap map;

        public UnityGridPathfindingGraph(Tilemap map)
        {
            this.map = map;
        }

        public double Heuistic(Vector3Int a, Vector3Int b)
        {
			return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }	


        public IEnumerable<CoordWithWeight<Vector3Int>> Neighbors(CoordWithWeight<Vector3Int> c)
		{
			var current = c.Coord;
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					var point = new Vector3Int(current.x + i, current.y + j, 0);
					var tile = map.GetTile(point);
					if (tile == null)
					{
						//TODO: ADD BOUNDS 
						if (i != 0 && j != 0)
						{
							yield return new CoordWithWeight<Vector3Int>(point, 2);
						}
						else
						{
							yield return new CoordWithWeight<Vector3Int>(point, 1);
						}
					}
				}
			}
		}

    }
}
