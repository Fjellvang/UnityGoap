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

    public override Dictionary<string, object> GetWorldState() // Todo this should probably be abstracted to a service or something
    {
        return new Dictionary<string, object> 
        {
            { "hasAxe", false },
            { "hasFirewood", false }
        };
    }
}
