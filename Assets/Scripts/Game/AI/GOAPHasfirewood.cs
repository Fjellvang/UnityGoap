using Assets.Scripts.Game.AI;
using Assets.Scripts.GOAP;
using System.Collections.Generic;

public class GOAPHasfirewood : GOAPAgentBase
{
    public override GoapCondition CreateGoalState()
    {
        return new GoapCondition(new Dictionary<string, object>()
        {
            { "hasFirewood", true }
        });
    }

    public override GoapCondition GetWorldState() // Todo this should probably be abstracted to a service or something
    {
        return new GoapCondition(new Dictionary<string, object> 
        {
            { "hasAxe", false },
            { "hasFirewood", false }
        });
    }
}
