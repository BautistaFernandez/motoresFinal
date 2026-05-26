using UnityEngine;

public interface ILightMode
{
    void Apply(Light light);
    bool IsUVMode { get; }
}
