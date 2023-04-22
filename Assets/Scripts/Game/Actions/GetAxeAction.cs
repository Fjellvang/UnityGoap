using Assets.Scripts.GameComponents;
using Assets.Scripts.GOAP;
using UnityEngine;

namespace Assets.Scripts.Game.Actions
{
    public class GetAxeAction : GoapAction
    {
        bool _collectedAxe = false;

        private float _startTime = 0;
        public float workDuration = .2f; // seconds

        public GetAxeAction()
        {
            AddPrecondition("hasAxe", false);
            AddEffect("hasAxe", true);// TODO: Use constants for these strings
        }
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            return CheckProcedualPrecondition<AxeComponent>(agent);
        }

        public override bool IsDone() => _collectedAxe;

        public override bool Perform(GameObject agent)
        {
            _startTime += Time.deltaTime;
            if (_startTime >= workDuration)
            {
                _collectedAxe = true;
                // Update the inventory
                // Use the tools durability etc
            }
            return true;
        }

        public override bool RequiresInRange() => true;

        public override void ResetGoap()
        {
            _collectedAxe = false;
            _startTime = 0;
        }
    }
}
