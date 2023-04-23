using System.Collections.Generic;

namespace Assets.Scripts.GOAP
{
    public record GoapCondition
    {
        private readonly Dictionary<string, object> _conditions;
        public GoapCondition(Dictionary<string, object> conditions = null)
        {
            _conditions = conditions ?? new Dictionary<string, object>();
        }

        public bool Satisfy(GoapCondition condition)
        {
            foreach (var pre in _conditions)
            {
                bool precondictionSatisfied = condition._conditions.TryGetValue(pre.Key, out var val) && pre.Value.Equals(val);
                if (!precondictionSatisfied)
                {
                    // all preconditions must be satified.
                    return false;
                }
            }
            return true;
        }

        public GoapCondition ApplyActionEffects(Dictionary<string, object> effects)
        {
            //Work on a copy, so we don't change the referenced state.
            var copy = new Dictionary<string, object>(_conditions);

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

            return new GoapCondition(copy);
        }

        public void AddCondition(string key, object value)
        {
            _conditions.Add(key, value);
        }


        public void RemoveCondition(string key)
        {
            _conditions.Remove(key);
        }
    }
}
