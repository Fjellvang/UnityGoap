using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GOAP
{
    public static class GoapExtensions //TODO: I'm not sure extensions are correct here. Consider wrapping in dedicated class?
    {
        public static List<GoapAction> ActionSubSet(this List<GoapAction> actions, GoapAction toExclude)
        {
            var copy = new List<GoapAction>();
            for (int i = 0; i < actions.Count; i++)
            {
                var action = actions[i];
                if (!action.Equals(toExclude))
                {
                    copy.Add(action);
                }
            }
            return copy;
        }
        /// <summary>
        /// Applies the <paramref name="effects"/> to a copy of <paramref name="worldState"/> and returns the copy
        /// </summary>
        /// <param name="worldState"></param>
        /// <param name="effects"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ApplyActionEffects(this Dictionary<string, object> worldState, Dictionary<string, object> effects)
        {
            //Work on a copy, so we don't change the referenced state.
            var copy = new Dictionary<string, object>(worldState);

            foreach (var effect in effects)
            {
                if (copy.ContainsKey(effect.Key))
                {
                    copy[effect.Key] = effect.Value;
                }
                else
                {
                    copy.Add(effect.Key, effect.Value);
                }
            }

            return copy;
        }
        /// <summary>
        /// Returns True if all <paramref name="preconditions"/> are satified by the <paramref name="worldState"/>
        /// </summary>
        /// <param name="preconditions"></param>
        /// <param name="worldState"></param>
        /// <returns></returns>
        public static bool Satisfy(this Dictionary<string, object> preconditions, Dictionary<string, object> worldState)
        {
            foreach (var pre in preconditions)
            {
                bool precondictionSatisfied = worldState.TryGetValue(pre.Key, out var val) && pre.Value.Equals(val);
                if (!precondictionSatisfied)
                {
                    // all preconditions must be satified.
                    return false;
                }
            }
            return true;
        }
    }
}
