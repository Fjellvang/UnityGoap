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
    }
}
