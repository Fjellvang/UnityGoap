using UnityEngine;

namespace Assets.Scripts.Game.AI
{
    public class AIGotoState : AIBaseState
    {
        public override void Update(AIController controller)
        {
            GOAP.GoapAction nextAction = controller.currentActions.Peek();
            if (nextAction == null)
            {
                Debug.Log("Next axtion is null, find new action");
                controller.stateMachine.TransitionState(findActionState);
            }

            if (controller.goapDataProvider.MoveAgent(nextAction))
            {
                controller.stateMachine.TransitionState(performActionState);
            }
        }
    }
}
