using Assets.Scripts.GOAP;
using Assets.Scripts.StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game.AI
{
    public class AIController : MonoBehaviour
    {
        public AIStateMachine stateMachine;

        public HashSet<GoapAction> availableActions; //TODO: Consider incapulation - it's currenly a consequece of our FSM design..

        public Queue<GoapAction> currentActions;

        public GOAPAgentBase goapDataProvider; 
        private void Start()
        {
            stateMachine = new AIStateMachine(this);

            var actions = GetComponents<GoapAction>();
            availableActions = actions.ToHashSet();
        }

        private void Update()
        {
            stateMachine.currentState.Update(this);
        }
    }
}
