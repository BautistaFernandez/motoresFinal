using UnityEngine;

// Modo normal
public class NormalLightMode : ILightMode
{
    public bool IsUVMode => false;

    public void Apply(Light light)
    {
        light.color = new Color(1f, 0.95f, 0.85f);
        light.intensity = 6f;
    }
}
