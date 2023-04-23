using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game.AI
{
    public class AIPerformActionState : AIBaseState
    {
        public override void Update(AIController controller)
        {
            var hasActionPlan = controller.currentActions.Any();
            if (!hasActionPlan)
            {
                Debug.Log("AI Perfomed action");
                controller.goapDataProvider.ActionsFinished();
                controller.stateMachine.TransitionState(findActionState);
                return;
            }

            var action = controller.currentActions.Peek();
            if (action.IsDone())
            {
                controller.currentActions.Dequeue();
            }

            var inRange = !action.RequiresInRange() || action.IsInRange();

            if (inRange)
            {
                var success = action.Perform(controller.gameObject);

                if (!success)
                {
                    //Plan failed we need to abort and find a new plan
                    controller.goapDataProvider.PlanAborted(action);
                    controller.stateMachine.TransitionState(findActionState);
                }
            }
            else
            {
                //Move to the action
                controller.stateMachine.TransitionState(goToActionState);
            }
        }
    }
}
