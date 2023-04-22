using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game.AI
{
    public class AIFindActionState : AIBaseState
    {
        public override void Update(AIController controller)
        {
            var worldState = controller.goapDataProvider.GetWorldState();
            var goal = controller.goapDataProvider.CreateGoalState();

            var plan = goapPlanner.PlanWithAStar(controller.gameObject, controller.availableActions, worldState, goal);

            if (plan == null)
            {
                Debug.Log("finding plan failed");
                controller.goapDataProvider.PlanFailed(goal);
                return;
            }

            currentActions = plan;
            controller.stateMachine.TransitionState(goToActionState);
        }
    }
}
