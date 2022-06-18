using System.Collections.Generic;

namespace Assets.Scripts.GOAP
{
    public static class GoapExtensions //TODO: I'm not sure extensions are correct here. Consider wrapping in dedicated class?
    {
        public static HashSet<GoapAction> ActionSubSet(this HashSet<GoapAction> actions, GoapAction toExclude)
        {
            var copy = new HashSet<GoapAction>();
            foreach (var action in actions)
            {
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
            var conditionSatisfied = true;
            foreach (var pre in preconditions)
            {
                // all preconditions must be satified.
                if (worldState.ContainsKey(pre.Key) && pre.Value == worldState[pre.Key])
                {
                    conditionSatisfied &= true;
                }
                else
                {
                    conditionSatisfied &= false;
                }
            }
            return conditionSatisfied;
        }
    }
}
