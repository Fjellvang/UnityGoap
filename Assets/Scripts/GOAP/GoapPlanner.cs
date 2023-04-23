using Assets.Scripts.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GOAP
{
    public class GoapPlanner
    {
        public Queue<GoapAction> Plan(GameObject agent, List<GoapAction> availableActions, Dictionary<string, object> worldState, Dictionary<string, object> goal)
        {
            List<GoapAction> useableActions = new();
            for (int i = 0; i < availableActions.Count; i++)
            {
                GoapAction goapAction = availableActions[i];
                goapAction.DoReset();
                if (goapAction.CheckProceduralPrecondition(agent))
                {
                    useableActions.Add(goapAction);
                }
            }

            var start = new Node(parent: null, action: null, runningCost: 0, worldState);

            var priorityQueue = new PriorityQueue<Node>();
            var found = BuildGraph(start, priorityQueue, useableActions, goal);

            if (!found)
            {
                Debug.LogWarning("FOUND NO SOLUTION");
                return null;
            }

            var node = priorityQueue.Peek();

            var stack = new Stack<GoapAction>();

            while(node != null)
            {
                if (node.Action != null)
                {
                    stack.Push(node.Action);
                }
                node = node.Parent;
            }

            //TODO: refactor this mess
            var actionQueue = new Queue<GoapAction>();
            var item = stack.Pop();
            while (item != null)
            {
                actionQueue.Enqueue(item);
                stack.TryPop(out item);
            }

            return actionQueue;
        }

        private bool BuildGraph(Node parent, PriorityQueue<Node> priorityQueue, List<GoapAction> useableActions, Dictionary<string, object> goal)
        {
            // TODO: Consider if buildGraph should be fused with AStar here.
            // WE could use the priority queue to check next paths which are cheaper first???
            // And end the search as soon as we found a worldstate which satisfies the goal
            // So something like storing the nodes in that priority queue with the running cost.
            var foundASolution = false;
            foreach (var action in useableActions)
            {
                if (action.Preconditions.Satisfy(parent.WorldState))
                {
                    var newWorldState = parent.WorldState.ApplyActionEffects(action.Effects);

                    var node = new Node(parent, action, parent.RunningCost + action.cost, newWorldState);


                    if (goal.Satisfy(newWorldState))
                    {
                        foundASolution = true;
                        priorityQueue.Enqueue(node); // With this the cheapest graph will always be first...
                    }
                    else
                    {
                        var subSet = useableActions.ActionSubSet(action);
                        var success = BuildGraph(node, priorityQueue, subSet, goal);
                        if (success)
                        {
                            foundASolution = true;
                        }
                    }
                }
            }

            return foundASolution;
        }
        public Queue<GoapAction> PlanWithAStar(GameObject agent, List<GoapAction> availableActions, Dictionary<string, object> worldState, Dictionary<string, object> goal)
        {
            List<GoapAction> useableActions = new();
            for (int i = 0; i < availableActions.Count; i++)
            {
                GoapAction goapAction = availableActions.ElementAt(i);
                goapAction.DoReset();
                if (goapAction.CheckProceduralPrecondition(agent))
                {
                    useableActions.Add(goapAction);
                }
            }

            var aStar = new AStar<GoapPrecondition>(new GoapGraph());

            var path = aStar.ComputeAStar(
                new GoapPrecondition(useableActions.First(), useableActions, worldState),// We should have an action first always.
                new GoapPrecondition(null, null, goal)); ;

            var actionQueue = new Queue<GoapAction>();
            var item = path.Pop();
            while (item != null)
            {
                if(item.Action != null)
                {
                    actionQueue.Enqueue(item.Action);
                }
            }

            return actionQueue;
        }
    }
    //TODO: Rethink the naming for these classes
    public class GoapPrecondition : IEquatable<GoapPrecondition>
    {
        public GoapPrecondition(GoapAction action, List<GoapAction> useableActions, Dictionary<string, object> worldState)
        {
            Action = action;
            UseableActions = useableActions;
            WorldState = worldState;
        }

        public GoapAction Action { get; }
        public List<GoapAction> UseableActions { get; }
        public Dictionary<string, object> WorldState { get; }

        // To use this in my aStart i cannot have this equality check.
        public bool Equals(GoapPrecondition other)
        {
            return Action == other.Action;
        }

        public override int GetHashCode()
        {
            return Action.GetHashCode();
        }
    }

    public class GoapGraph : IPathfindingGraph<GoapPrecondition>
    {
        public double Heuistic(GoapPrecondition a, GoapPrecondition b)
        {
            return 0; // The heuistic should be build in as all actions have variable cost
        }

        public IEnumerable<CoordWithWeight<GoapPrecondition>> Neighbors(CoordWithWeight<GoapPrecondition> current)
        {
            foreach(var action in current.Coord.UseableActions)
            {
                if (action.Preconditions.Satisfy(current.Coord.Action.Preconditions))
                {
                    var worldState = current.Coord.WorldState.ApplyActionEffects(action.Effects);
                    var actionSubSet = current.Coord.UseableActions.ActionSubSet(action);
                    var GoapPrecondition = new GoapPrecondition(action, actionSubSet, worldState);
                    yield return new CoordWithWeight<GoapPrecondition>(GoapPrecondition, action.cost);
                }
            }
        }
    }

    public class Node : IComparable<Node>
    {
        public Node(Node parent, GoapAction action, float runningCost, Dictionary<string, object> worldState)
        {
            Parent = parent;
            Action = action;
            RunningCost = runningCost;
            WorldState = worldState;
        }
        public List<Node> Children = new List<Node>();


        public Node Parent { get; }
        public GoapAction Action { get; }
        public float RunningCost { get; }
        public Dictionary<string, object> WorldState { get; }

        public int CompareTo(Node other)
        {
            return this.RunningCost.CompareTo(other.RunningCost);
        }
    }
    
}
