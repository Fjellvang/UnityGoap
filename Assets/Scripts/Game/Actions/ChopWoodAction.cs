﻿using Assets.Scripts.GameComponents;
using Assets.Scripts.GOAP;
using UnityEngine;

namespace Assets.Scripts.Game.Actions
{
    [CreateAssetMenu(menuName = "GoapActions/ChopWood")]
    public class ChopWoodAction : GoapAction
    {
        private bool _chopped = false;

        private float _startTime = 0;
        public float workDuration = 2; // seconds

        public ChopWoodAction()
        {
            AddPrecondition(Constants.Actions.HasAxe, true); //TODO: GOD DAMN CONSTANTS
            AddPrecondition(Constants.Actions.HasFirewood, false);
            AddEffect(Constants.Actions.HasFirewood, true);
        }
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            return CheckProcedualPrecondition<TreeComponent>(agent);
        }

        public override bool IsDone => _chopped;

        public override bool Perform(GameObject agent)
        {
            _startTime += Time.deltaTime;
            if (_startTime >= workDuration)
            {
                _chopped = true;

                Destroy(target);
                // Update the inventory
                // Use the tools durability etc
            }
            return true;
        }

        public override bool RequiresInRange => true;

        public override void ResetGoap()
        {
            _chopped = false;
            _startTime = 0;
        }
    }
}
