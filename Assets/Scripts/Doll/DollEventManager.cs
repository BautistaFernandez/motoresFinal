using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class DollEventManager : MonoBehaviour
{
    private float waitingTime = 10f;

    [SerializeField] private DemonDoll demonDoll;

    void Start()
    {
        if (demonDoll != null)
        {
            StartCoroutine(timerAparision());
        }
    }

    private IEnumerator timerAparision()
    {
        yield return new WaitForSeconds(waitingTime);

        demonDoll.Aparision();
    }
}
