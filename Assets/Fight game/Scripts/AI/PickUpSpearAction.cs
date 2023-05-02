using Assets.Scripts.GOAP;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(menuName = "FightGame/GoapActions/PickUpSpearAction")]
public class PickUpSpearAction : GoapAction
{
    bool isDone = false;
    public PickUpSpearAction()
    {
        AddPrecondition("HasSpear", false);
        AddEffect("HasSpear", true);
    }
    public override bool IsDone => isDone;

    public override bool RequiresInRange => true;

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        // TODO: in a proper solution we would use a quadtree or something similar to find the tree instead of this brute force approach
        var spears = GameObject.FindGameObjectsWithTag("Spear");
        GameObject closest = null;
        float closestDist = 0;

        foreach (var spear in spears)
        {
            if (closest == null)
            {
                // first one, so choose it for now
                closest = spear;
                closestDist = (spear.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                float dist = (spear.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    // we found a closer one, use it
                    closest = spear;
                    closestDist = dist;
                }
            }
        }
        if (closest == null)
            return false;

        target = closest;

        return closest != null;
    }

    public override bool IsInRange(GameObject agent)
    {
        var d = Vector3.Distance(agent.transform.position, target.transform.position);

        return d < 1;//TODO: make configureable
    }

    public override bool Perform(GameObject agent)
    {
        //TODO: update inventory and stuff?
        isDone = true;
        return true;
    }

    public override void ResetGoap()
    {
        isDone = false;
    }
}
