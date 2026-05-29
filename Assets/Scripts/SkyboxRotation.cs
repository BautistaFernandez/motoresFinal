using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    private float speedRotation = 1f;
    void Start()
    {
        
    }

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * speedRotation);
    }
}
