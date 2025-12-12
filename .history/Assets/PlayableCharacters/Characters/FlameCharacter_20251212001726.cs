using UnityEngine;

public class FlameCharacter : BaseCharacter
{
    public float moveSpeed = 5f;

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
    }

    // FlameCharacter es inmune a la lava: sobreescribimos el manejador
    protected override void HandleLavaCollision(GameObject lava)
    {
        // no hacer nada: inmune
    }
}
