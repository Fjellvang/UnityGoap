﻿using Assets.Scripts.GameComponents;
using Assets.Scripts.GOAP;
using UnityEngine;

namespace Assets.Scripts.Game.Actions
{
    [CreateAssetMenu(menuName = "GoapActions/CollectBranches")]
    public class CollectBranchesAction : GoapAction
    {
        bool _collectedBranches = false;

        private float _startTime = 0;
        public float workDuration = 2; // seconds
        public CollectBranchesAction()
        {
            AddPrecondition(Constants.Actions.HasFirewood, false);
            AddEffect(Constants.Actions.HasFirewood, true);
        }
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            return CheckProcedualPrecondition<BranchComponent>(agent);
        }

        public override bool IsDone =>
            _collectedBranches;

        public override bool Perform(GameObject agent)
        {
            _startTime += Time.deltaTime;
            if (_startTime >= workDuration)
            {
                _collectedBranches = true;
                Destroy(target);
                // Update the inventory
                // Use the tools durability etc
            }
            return true;
        }

        public override bool RequiresInRange => true;
        public override void ResetGoap()
        {
            _collectedBranches = false;
            _startTime = 0;
        }
    }
}
