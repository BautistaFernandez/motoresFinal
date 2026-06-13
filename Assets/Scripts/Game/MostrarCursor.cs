using UnityEngine;

public class MostrarCursor : MonoBehaviour
{
    void Start()
    {
       
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}