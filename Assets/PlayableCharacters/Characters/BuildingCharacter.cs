using UnityEngine;

public class BuildingCharacter : BaseCharacter
{
    public float moveSpeed = 5f;
    [SerializeField] Building building;
    bool activeBuild;
    int buildsMax = 6;
    public int buildsCount = 6;

    protected override void HandleMovement()
    {
        if (!isPossessed) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;
        rb.linearVelocity = dir * moveSpeed;
    }

    protected override void HandleAbilities()
    {
        if (!isPossessed) return;

        if (Input.GetKeyDown(KeyCode.E) && !activeBuild && !building.enabled)
        {
            building.enabled = true;
            building.CreateProyectionObject();
            activeBuild = true;
        }
        if(Input.GetKeyDown(KeyCode.F) && activeBuild && building.enabled)
        {
            building.enabled = false;
            activeBuild = false;
            building.DestroProyectionObject();
        }
        if (activeBuild && building.enabled)
        {
            building.UpdateProyectionPosition(this);
        }
    }

    
    protected override void HandleLavaCollision(GameObject lava)
    {
        OnDead();
    }
    private void OnDisable()
    {
        building.DestroProyectionObject();
        building.enabled = false;
        activeBuild = false;
    }
}
