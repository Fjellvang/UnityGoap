namespace Assets.Scripts.Game.AI
{
    public class AIGotoState : AIBaseState
    {
        public override void Update(AIController controller)
        {
            if (controller.goapDataProvider.MoveAgent(currentActions.Peek()))
            {
                controller.stateMachine.TransitionState(performActionState);
            }
        }
    }
}
