using Assets.Scripts.StateMachine;

namespace Assets.Scripts.Game.AI
{
    public class AIBaseState : BaseState<AIController>
    {
        public static readonly AIFindActionState findActionState = new AIFindActionState();
        public static readonly AIGotoState gotoState = new AIGotoState();
        public static readonly AIPerformActionState performActionState = new AIPerformActionState();

        public override void OnEnterState(AIController controller)
        {
        }

        public override void OnExitState(AIController controller)
        {
        }

        public override void Update(AIController controller)
        {
        }
    }
}
