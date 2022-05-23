using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Pathfinding
{
    public class AStar<TCoordType>
		where TCoordType : IEquatable<TCoordType>
	{
        public AStar(IPathfindingGraph<TCoordType> pathfindingGraph)
        {
            this.pathfindingGraph = pathfindingGraph;
        }

        private readonly IPathfindingGraph<TCoordType> pathfindingGraph;

		public Stack<TCoordType> ComputeAStar(TCoordType start, TCoordType goal)
		{
            Stack<TCoordType> path = new Stack<TCoordType>();
			var frontier = new PriorityQueue<CoordWithWeight<TCoordType>>();
			var cameFrom = new Dictionary<TCoordType, TCoordType>();
			var costSoFar = new Dictionary<TCoordType, double>();
			var goalFound = false;
			costSoFar[start] = 0;
			frontier.Enqueue(new CoordWithWeight<TCoordType>(start, 0));
			while (frontier.Count > 0 && !goalFound)
			{
				var current = frontier.Dequeue();

				foreach (var point in this.pathfindingGraph.Neighbors(current))
				{
					//TODO: test if we need early exit?
					var newCost = costSoFar[current.Coord] + point.Weight;

					if (point.Coord.Equals(goal))
					{
						goalFound = true;
						cameFrom[point.Coord] = current.Coord;
						break;
					}

					if (!costSoFar.ContainsKey(point.Coord) ||
						newCost < costSoFar[point.Coord])
					{
						costSoFar[point.Coord] = newCost;

						var priority = newCost + pathfindingGraph.Heuistic(goal, point.Coord);
						frontier.Enqueue(new CoordWithWeight<TCoordType>(point.Coord, priority));

						cameFrom[point.Coord] = current.Coord;
					}
				}
			}

			var next = goal;

			while (!next.Equals(start))
			{
				path.Push(next);
				next = cameFrom[next];
			}
			path.Push(start);
			return path;
		}
	}
}
