using UnityEngine;

namespace Assets.Scripts.Game.AI
{
    public class AIGotoState : AIBaseState
    {
        public override void Update(AIController controller)
        {
            GOAP.GoapAction currentAction = controller.currentActions.Peek();
            if (currentAction == null)
            {
                Debug.Log("Next axtion is null, find new action");
                controller.stateMachine.TransitionState(findActionState);
            }

            if (controller.goapDataProvider.MoveAgent(currentAction))
            {
                controller.stateMachine.TransitionState(performActionState);
            }
        }
    }
}
