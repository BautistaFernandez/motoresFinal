using UnityEngine;

// Modo UV
public class UVLightMode : ILightMode
{
    public bool IsUVMode => true;

    public void Apply(Light light)
    {
        light.color = new Color(0.5f, 0.2f, 1f);
        light.intensity = 6f;
    }
}
