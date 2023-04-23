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

            //var plan = goapPlanner.PlanWithAStar(controller.gameObject, controller.actions, worldState, goal);
            var plan = goapPlanner.Plan(controller.gameObject, controller.actions, worldState, goal);

            if (plan == null)
            {
                Debug.Log("finding plan failed");
                controller.goapDataProvider.PlanFailed(goal);
                return;
            }

            controller.currentActions = plan;
            controller.stateMachine.TransitionState(goToActionState);
        }
    }
}
