using UnityEngine;

// ── HERENCIA ──────────────────────────
public abstract class Pickable : MonoBehaviour
{
    public virtual string GetPrompt() => "Press [E] to pick up";

    public abstract void OnPickup();
}
