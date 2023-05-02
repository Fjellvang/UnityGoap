using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GOAP
{

    public abstract class GoapAction : ScriptableObject,
        IEquatable<GoapAction>
    {
        private GoapCondition _preconditions;
        private Dictionary<string, object> _effects;

        private bool _inRange = false;

        /* The cost of performing the action.
         * Figure out a weight that suits the action.
         * Changing it will affect what actions are chosen during planning.*/
        public float cost = 1f;

        /**
         * An action often has to perform on an object. This is that object. Can be null. */
        public GameObject target;

        public GoapAction()
        {
            _preconditions = new GoapCondition();
            _effects = new Dictionary<string, object>();
        }

        public void DoReset()
        {
            _inRange = false;
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
        public abstract bool IsDone { get; }

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
        public abstract bool RequiresInRange { get; }

        public virtual bool IsInRange(GameObject agent)
        {
            return agent.transform.position.Equals(target.transform.position);
        }

        /**
         * Are we in range of the target?
         * The MoveTo state will set this and it gets reset each time this action is performed.
         */
        public bool IsInRange() => _inRange;

        public void SetInRange(bool inRange)
        {
            this._inRange = inRange;
        }


        public void AddPrecondition(string key, object value)
        {
            _preconditions.AddCondition(key, value);
        }


        public void RemovePrecondition(string key)
        {
            _preconditions.RemoveCondition(key);
        }


        public void AddEffect(string key, object value)
        {
            _effects.Add(key, value);
        }


        public void RemoveEffect(string key)
        {
            _effects.Remove(key);
        }

        public bool Equals(GoapAction other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }

        public GoapCondition Preconditions => _preconditions;

        public Dictionary<string, object> Effects => _effects;

        public virtual bool CheckProcedualPrecondition<T>(GameObject agent) where T : MonoBehaviour
        {
            // find the nearest tree that we can chop
            // in a proper solution we would use a quadtree or something similar to find the tree instead of this brute force approach
            T[] trees = (T[])FindObjectsOfType(typeof(T));
            T closest = null;
            float closestDist = 0;

            foreach (T tree in trees)
            {
                if (closest == null)
                {
                    // first one, so choose it for now
                    closest = tree;
                    closestDist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    // is this one closer than the last?
                    float dist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
                    if (dist < closestDist)
                    {
                        // we found a closer one, use it
                        closest = tree;
                        closestDist = dist;
                    }
                }
            }
            if (closest == null)
                return false;

            target = closest.gameObject;

            return closest != null;
        }
    }
}
