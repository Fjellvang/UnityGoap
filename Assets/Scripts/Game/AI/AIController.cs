using Assets.Scripts.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Game.AI
{
    public class AIController : MonoBehaviour
    {
        AIStateMachine stateMachine;
        private void Start()
        {
            stateMachine = new AIStateMachine();
        }

        private void Update()
        {
            currentState.Update(this);
        }
    }

    public class AIStateMachine : StateMachine<AIBaseState, AIController>
    {
        public AIStateMachine(AIController controller) : base(controller)
        {
            currentState = AIBaseState.findActionState;
        }
    }
}
