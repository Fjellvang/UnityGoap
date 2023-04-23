using Assets.Scripts.GOAP;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Pathfinding
{
	//TODO: This is too generic and doesn't work with goap refactor.
    public class AStar<TCoordType>
		where TCoordType : GoapPrecondition
	{
        public AStar(IPathfindingGraph<TCoordType> pathfindingGraph)
        {
            this._pathfindingGraph = pathfindingGraph;
        }

        private readonly IPathfindingGraph<TCoordType> _pathfindingGraph;

		public Stack<TCoordType> ComputeAStar(TCoordType start, TCoordType goal)
		{
            Stack<TCoordType> path = new();
			var frontier = new PriorityQueue<CoordWithWeight<TCoordType>>();
			var cameFrom = new Dictionary<TCoordType, TCoordType>();
			var costSoFar = new Dictionary<TCoordType, double>();
			var goalFound = false;
			costSoFar[start] = 0;
			frontier.Enqueue(new CoordWithWeight<TCoordType>(start, 0));
			while (frontier.Count > 0 && !goalFound)
			{
				var current = frontier.Dequeue();

				foreach (var point in this._pathfindingGraph.Neighbors(current))
				{
					//TODO: test if we need early exit?
					var newCost = costSoFar[current.Coord] + point.Weight;

					if (point.Coord.Action.Preconditions.Satisfy(goal.WorldState))
					{
						goalFound = true;
						cameFrom[point.Coord] = current.Coord;
						break;
					}

					if (!costSoFar.ContainsKey(point.Coord) ||
						newCost < costSoFar[point.Coord])
					{
						costSoFar[point.Coord] = newCost;

						var priority = newCost + _pathfindingGraph.Heuistic(goal, point.Coord);
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
