using Assets.Scripts.GameComponents;
using Assets.Scripts.GOAP;
using UnityEngine;

namespace Assets.Scripts.Game.Actions
{
    public class CollectBranchesAction : GoapAction
    {
        bool _collectedBranches = false;

        private float startTime = 0;
        public float workDuration = 2; // seconds
        public CollectBranchesAction()
        {
            AddPrecondition("hasFirewood", false);
            AddEffect("hasFirewood", true);
        }
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            return CheckProcedualPrecondition<BranchComponent>(agent);
        }

        public override bool IsDone() =>
            _collectedBranches;

        public override bool Perform(GameObject agent)
        {
            startTime += Time.deltaTime;
            if (startTime >= workDuration)
            {
                _collectedBranches = true;
                Destroy(target);
                // Update the inventory
                // Use the tools durability etc
            }
            return true;
        }

        public override bool RequiresInRange() => true;
        public override void ResetGoap()
        {
            _collectedBranches = false;
            startTime = 0;
        }
    }
}
