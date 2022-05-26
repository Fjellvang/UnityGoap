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

            var start = new Node(null, worldState);
            var result = BuildGraph(start, useableActions, goal);
        }

        private (bool success, List<Node> graph) BuildGraph(Node parent, HashSet<GoapAction> useableActions, Dictionary<string, object> goal)
        {
            var foundASolution = false;
            List<Node> graph = new List<Node>();
            foreach (var action in useableActions)
            {
                if (action.Preconditions.Satisfy(parent.WorldState))
                {
                    var newWorldState = parent.WorldState.ApplyActionEffects(action.Effects);

                    var node = new Node(action, newWorldState);

                    parent.Children.Add(node);

                    if (newWorldState.Satisfy(goal))
                    {
                        foundASolution = true;
                        graph.Add(node);
                    }
                    else
                    {
                        var subSet = useableActions.ActionSubSet(action);
                        var result = BuildGraph(node, subSet, goal);
                        graph.AddRange(result.graph);
                        if (result.success)
                        {
                            foundASolution = true;
                        }
                    }
                }
            }

            return (foundASolution, graph);
        }
    }

    public static class GoapExtensions
    {
        public static HashSet<GoapAction> ActionSubSet(this HashSet<GoapAction> actions, GoapAction toExclude)
        {
            var copy = new HashSet<GoapAction>();
            foreach (var action in actions)
            {
                if (!action.Equals(toExclude))
                {
                    copy.Add(action);
                }
            }
            return copy;
        }
        /// <summary>
        /// Applies the <paramref name="effects"/> to a copy of <paramref name="worldState"/> and returns the copy
        /// </summary>
        /// <param name="worldState"></param>
        /// <param name="effects"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ApplyActionEffects(this Dictionary<string, object> worldState, Dictionary<string, object> effects)
        {
            //Work on a copy, so we don't change the referenced state.
            var copy = new Dictionary<string, object>(worldState);

            foreach (var effect in effects)
            {
                if (copy.ContainsKey(effect.Key))
                {
                    copy[effect.Key] = effect.Value;
                }
                else
                {
                    copy.Add(effect.Key, effect.Value);
                }
            }

            return copy;
        }
        /// <summary>
        /// Returns True if all <paramref name="preconditions"/> are satified by the <paramref name="worldState"/>
        /// </summary>
        /// <param name="preconditions"></param>
        /// <param name="worldState"></param>
        /// <returns></returns>
        public static bool Satisfy(this Dictionary<string, object> preconditions, Dictionary<string, object> worldState)
        {
            var conditionSatisfied = true;
            foreach (var pre in preconditions)
            {
                // all preconditions must be satified.
                if (worldState.ContainsKey(pre.Key) && pre.Value == worldState[pre.Key])
                {
                    conditionSatisfied &= true;
                }
                else
                {
                    conditionSatisfied &= false;
                }
            }
            return conditionSatisfied;
        }
    }

    public class Node : IPathfindingGraph<GoapAction>
    {
        public Node(GoapAction action, Dictionary<string, object> worldState)
        {
            this.action = action;
            WorldState = worldState;
        }
        public List<Node> Children = new List<Node>();

        private readonly GoapAction action;

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
    }
}
