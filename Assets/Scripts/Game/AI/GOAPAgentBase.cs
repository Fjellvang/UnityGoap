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

        public abstract Dictionary<string, object> CreateGoalState();

        public Dictionary<string, object> GetWorldState()
        {
            var worldData = new Dictionary<string, object> //TODO: this should'nt be here
            {
                { "hasAxe", false },
                { "hasFirewood", false }
            };
            return worldData;
        }

        public bool MoveAgent(GoapAction nextAction)
        {
            //TODO: not sure i want this here.
            // move towards the NextAction's target
            float step = moveSpeed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextAction.target.transform.position, step);

            if (gameObject.transform.position.Equals(nextAction.target.transform.position))
            {
                // we are at the target location, we are done
                nextAction.SetInRange(true);
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

        public void PlanFailed(Dictionary<string, object> failedGoal)
        {
            Debug.Log("Plan failed called");
        }

        public void PlanFound(Dictionary<string, object> goal, Queue<GoapAction> actions)
        {
            Debug.Log($"Found Actions for goal: {string.Join(", ", goal.Keys.Select(x => x))}\nActions: {string.Join(", ", actions.Select(x => x.GetType().Name))}");

        }
    }
}
