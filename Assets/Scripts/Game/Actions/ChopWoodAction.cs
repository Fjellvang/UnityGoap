using Assets.Scripts.GameComponents;
using Assets.Scripts.GOAP;
using UnityEngine;

namespace Assets.Scripts.Game.Actions
{
    public class ChopWoodAction : GoapAction
    {
        private bool chopped = false;
        private TreeComponent targetTree; // where we get the logs from

        private float startTime = 0;
        public float workDuration = 2; // seconds

        public ChopWoodAction()
        {
            AddPrecondition("hasTool", true);
            AddPrecondition("hasLogs", false);
            AddEffect("hasLogs", true);
        }
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            // find the nearest tree that we can chop
            // in a proper solution we would use a quadtree or something similar to find the tree instead of this brute force approach
            TreeComponent[] trees = (TreeComponent[])FindObjectsOfType(typeof(TreeComponent));
            TreeComponent closest = null;
            float closestDist = 0;

            foreach (TreeComponent tree in trees)
            {
                if (closest == null)
                {
                    // first one, so choose it for now
                    closest = tree;
                    closestDist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    // is this one closer than the last?
                    float dist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
                    if (dist < closestDist)
                    {
                        // we found a closer one, use it
                        closest = tree;
                        closestDist = dist;
                    }
                }
            }
            if (closest == null)
                return false;

            targetTree = closest;
            target = targetTree.gameObject;

            return closest != null;
        }

        public override bool IsDone() => chopped;

        public override bool Perform(GameObject agent)
        {
            throw new System.NotImplementedException();
        }

        public override bool RequiresInRange() => true;

        public override void ResetGoap()
        {
            throw new System.NotImplementedException();
        }
    }
}
