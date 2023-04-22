using Assets.Scripts.Game.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GOAPHasfirewood : GOAPAgentBase
{
    public override Dictionary<string, object> CreateGoalState()
    {
        return new Dictionary<string, object>()
        {
            { "hasFirewood", true }
        };
    }
}
