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
        public Stack<GoapAction> Plan(GameObject agent, HashSet<GoapAction> availableActions, Dictionary<string, object> worldState, Dictionary<string, object> goal)
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

            var start = new Node(parent: null, action: null, runningCost: 0, worldState);

            var priorityQueue = new PriorityQueue<Node>();
            var found = BuildGraph(start, priorityQueue, useableActions, goal);

            if (!found)
            {
                Debug.LogWarning("FOUND NO SOLUTION");
            }

            var node = priorityQueue.Peek();

            var stack = new Stack<GoapAction>();

            while(node != null)
            {
                stack.Push(node.Action);
                node = node.Parent;
            }

            return stack;
        }

        private bool BuildGraph(Node parent, PriorityQueue<Node> priorityQueue, HashSet<GoapAction> useableActions, Dictionary<string, object> goal)
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

                    var node = new Node(parent, action, parent.RunningCost + action.cost, newWorldState);


                    if (newWorldState.Satisfy(goal))
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
