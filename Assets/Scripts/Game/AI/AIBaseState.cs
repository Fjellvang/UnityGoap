using Assets.Scripts.GOAP;
using Assets.Scripts.StateMachine;
using System.Collections.Generic;

namespace Assets.Scripts.Game.AI
{
    public abstract class AIBaseState : BaseState<AIController>
    {
        public static readonly AIFindActionState findActionState = new AIFindActionState();
        public static readonly AIGotoState goToActionState = new AIGotoState();
        public static readonly AIPerformActionState performActionState = new AIPerformActionState();

        protected Queue<GoapAction> currentActions;

        protected GoapPlanner goapPlanner = new GoapPlanner(); 

        public override void OnEnterState(AIController controller)
        {
        }

        public override void OnExitState(AIController controller)
        {
        }
    }
}
