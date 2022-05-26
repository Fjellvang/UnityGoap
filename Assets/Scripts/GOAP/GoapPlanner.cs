using Assets.Scripts.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GOAP
{
    public class GoapPlanner
    {
        public void Plan(GameObject agent, HashSet<GoapAction> availableActions, Dictionary<string, object> worldState, Dictionary<string, object> goal)
        {
            HashSet<GoapAction> useableActions = new HashSet<GoapAction>();
            for (int i = 0; i < availableActions.Count; i++)
            {
                GoapAction goapAction = availableActions.ElementAt(i);
                goapAction.DoReset();
                if (goapAction.CheckProceduralPrecondition(agent))
                {
                    useableActions.Add(goapAction);
                }
            }

            var start = new Node(parent: null, action: null, worldState);
            var found = BuildGraph(start, useableActions, goal);

            if (!found)
            {
                Debug.LogWarning("FOUND NO SOLUTION");
            }

            var aStar = new AStar<GoapAction>(start);

            aStar.ComputeAStar(this does not work.);
        }

        private bool BuildGraph(Node parent, HashSet<GoapAction> useableActions, Dictionary<string, object> goal)
        {
            //TODO: Consider if buildGraph should be fused with AStar here.
            // WE could use the priority queue to check next paths which are cheaper first???
            // And end the search as soon as we found a worldstate which satisfies the goal
            // So something like storing the nodes in that priority queue with the running cost.
            var foundASolution = false;
            foreach (var action in useableActions)
            {
                if (action.Preconditions.Satisfy(parent.WorldState))
                {
                    var newWorldState = parent.WorldState.ApplyActionEffects(action.Effects);

                    var node = new Node(parent, action, newWorldState);


                    if (newWorldState.Satisfy(goal))
                    {
                        foundASolution = true;
                        parent.AddChildren(node);
                    }
                    else
                    {
                        var subSet = useableActions.ActionSubSet(action);
                        var success = BuildGraph(node, subSet, goal);
                        if (success)
                        {
                            foundASolution = true;
                        }
                    }
                }
            }

            return foundASolution;
        }
    }

    public class Node : IPathfindingGraph<GoapAction>
    {
        public Node(Node parent, GoapAction action, Dictionary<string, object> worldState)
        {
            Parent = parent;
            this.action = action;
            WorldState = worldState;
        }
        public List<Node> Children = new List<Node>();

        private readonly GoapAction action;

        public Node Parent { get; }
        public Dictionary<string, object> WorldState { get; }

        public double Heuistic(GoapAction a, GoapAction b)
        {
            return 0; // not sure what a good heuristic is.
        }

        public IEnumerable<CoordWithWeight<GoapAction>> Neighbors(CoordWithWeight<GoapAction> current)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                yield return new CoordWithWeight<GoapAction>(Children[i].action, Children[i].action.cost);
            }
        }

        /// <summary>
        /// Recursively adds children when a goal has been found.
        /// </summary>
        /// <param name="child"></param>
        public void AddChildren(Node child) {

            if (!Children.Contains(child))
            {
                Children.Add(child);
            }

            if (Parent != null)
            {
                Parent.AddChildren(this);
            }
        }

        public override int GetHashCode()
        {
            return action?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is Node node)
            {
                action.Equals(node.action);
            }
            return false;   
        }
    }
}
