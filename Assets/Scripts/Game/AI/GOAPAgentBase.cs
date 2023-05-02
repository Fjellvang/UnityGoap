using Assets.Scripts.GOAP;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game.AI
{
    public abstract class GOAPAgentBase : MonoBehaviour, IGoap
    {
        public float moveSpeed = 1;

        public void ActionsFinished()
        {
            Debug.Log("<color=blue>Actions completed</color>");
        }

        public abstract GoapCondition CreateGoalState();

        public abstract GoapCondition GetWorldState();

        public bool MoveAgent(GoapAction currentAction)
        {
            //TODO: not sure i want this here.
            // move towards the NextAction's target
            float step = moveSpeed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, currentAction.target.transform.position, step);

            if (currentAction.IsInRange(gameObject))
            {
                // we are at the target location, we are done
                currentAction.SetInRange(true);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PlanAborted(GoapAction aborter)
        {
            Debug.Log($"Plan aborted: {aborter.GetType().Name}");
        }

        public void PlanFailed(GoapCondition failedGoal)
        {
            Debug.Log("Plan failed called");
        }

        public void PlanFound(GoapCondition goal, Queue<GoapAction> actions)
        {
            Debug.Log($"Found Actions for goal \nActions: {string.Join(", ", actions.Select(x => x.GetType().Name))}");

        }
    }
}
