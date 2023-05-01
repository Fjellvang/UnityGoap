using Assets.Scripts.GOAP;
using UnityEngine;

[CreateAssetMenu(menuName = "FightGame/GoapActions/ThrowSpearAction")]
public class ThrowSpearAction : GoapAction
{
    public ThrowSpearAction()
    {
        AddEffect("SpearThrown", true);
        AddPrecondition("HasSpear", true);
    }
    bool spearThrown = false;
    public float aimTime = 1f;
    private float startTime = 0f;

    public override bool IsDone => spearThrown;

    public override bool RequiresInRange => false;

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        // Not sure there is any here.
        return true;
    }

    public override bool Perform(GameObject agent)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        agent.transform.LookAt(player.transform, Vector3.up);

        startTime += Time.deltaTime;
        if (startTime >= aimTime)
        {
            //we shoot
            Debug.Log("SHOOT");
            agent.GetComponent<SpearShooter>().Shoot();
            spearThrown = true;
        }

        return true;
    }

    public override void ResetGoap()
    {
        spearThrown = false;
        startTime = 0f;
    }
}
