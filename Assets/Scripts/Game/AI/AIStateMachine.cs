using Assets.Scripts.StateMachine;

namespace Assets.Scripts.Game.AI
{
    public class AIStateMachine : StateMachine<AIBaseState, AIController>
    {
        public AIStateMachine(AIController controller) : base(controller)
        {
            currentState = AIBaseState.findActionState;
        }
    }
}
