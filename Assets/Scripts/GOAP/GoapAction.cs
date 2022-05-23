using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GOAP
{
    public abstract class GoapAction : MonoBehaviour
    {
        private HashSet<KeyValuePair<string, object>> preconditions;
        private HashSet<KeyValuePair<string, object>> effects;

        private bool inRange = false;

        /* The cost of performing the action.
         * Figure out a weight that suits the action.
         * Changing it will affect what actions are chosen during planning.*/
        public float cost = 1f;

        /**
         * An action often has to perform on an object. This is that object. Can be null. */
        public GameObject target;

        public GoapAction()
        {
            preconditions = new HashSet<KeyValuePair<string, object>>();
            effects = new HashSet<KeyValuePair<string, object>>();
        }

        public void DoReset()
        {
            inRange = false;
            target = null;
            ResetGoap();
        }

        /**
         * Reset any variables that need to be reset before planning happens again.
         */
        public abstract void ResetGoap();

        /**
         * Is the action done?
         */
        public abstract bool IsDone();

        /**
         * Procedurally check if this action can run. Not all actions
         * will need this, but some might.
         */
        public abstract bool CheckProceduralPrecondition(GameObject agent);

        /**
         * Run the action.
         * Returns True if the action performed successfully or false
         * if something happened and it can no longer perform. In this case
         * the action queue should clear out and the goal cannot be reached.
         */
        public abstract bool Perform(GameObject agent);

        /**
         * Does this action need to be within range of a target game object?
         * If not then the moveTo state will not need to run for this action.
         */
        public abstract bool RequiresInRange();


        /**
         * Are we in range of the target?
         * The MoveTo state will set this and it gets reset each time this action is performed.
         */
        public bool IsInRange() => inRange;

        public void SetInRange(bool inRange)
        {
            this.inRange = inRange;
        }


        public void AddPrecondition(string key, object value)
        {
            preconditions.Add(new KeyValuePair<string, object>(key, value));
        }


        public void RemovePrecondition(string key)
        {
            preconditions.RemoveWhere(kvp => kvp.Key.Equals(key,System.StringComparison.Ordinal));
        }


        public void AddEffect(string key, object value)
        {
            effects.Add(new KeyValuePair<string, object>(key, value));
        }


        public void RemoveEffect(string key)
        {
            KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
            effects.RemoveWhere(kvp => kvp.Key.Equals(key, System.StringComparison.Ordinal));
        }


        public HashSet<KeyValuePair<string, object>> Preconditions => preconditions;

        public HashSet<KeyValuePair<string, object>> Effects => effects;
    }
}
