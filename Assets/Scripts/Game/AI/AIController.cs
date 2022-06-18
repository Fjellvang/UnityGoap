using Assets.Scripts.GOAP;
using Assets.Scripts.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.AI
{
    public class AIController : MonoBehaviour
    {
        public AIStateMachine stateMachine;


        public HashSet<GoapAction> availableActions; //TODO: Consider incapulation - it's currenly a consequece of our FSM design..
        public IGoap goapDataProvider; //TODO: Inject this ?
        private void Start()
        {
            stateMachine = new AIStateMachine(this);

            availableActions = new HashSet<GoapAction>();
            var actions = GetComponents<GoapAction>();
            foreach (var action in actions)
            {
                availableActions.Add(action);
            }
        }

        private void Update()
        {
            stateMachine.currentState.Update(this);
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
