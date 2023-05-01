using Assets.Scripts.Game.AI;
using Assets.Scripts.GOAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrower : GOAPAgentBase
{
    public override GoapCondition CreateGoalState()
    {
        return new GoapCondition(new Dictionary<string, object>
        {
            { "SpearThrown", true },
        });
    }

    public override GoapCondition GetWorldState()
    {
        return new GoapCondition(new Dictionary<string, object>
        {
            { "HasSpear", false },
        });
    }
}
