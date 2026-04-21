using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Objective1 : MonoBehaviour
{
    /* [Header("Configuración Objective1")]
    private float waitingTime = 3f;
    private float waitingForObjective = 2f;
    private float waitingForHide = 1f;
    private bool objective1Active;
    [SerializeField] private TextMeshProUGUI text;
    // tomar referencia del objeto del canvas con el objectivo.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (text != null) text.gameObject.SetActive(false);

        objective1Active = false;

        StartCoroutine(timerObjective1());
    }

    private IEnumerator timerObjective1()
    {
        yield return new WaitForSeconds(waitingTime);

        if (text != null)
        {
            text.gameObject.SetActive(true);

            yield return new WaitForSeconds(waitingForObjective);

            SetActive();

            HideObjective1();
        }

    }

    private IEnumerator HideObjective1()
    {
        yield return new WaitForSeconds(waitingForHide);

        text.gameObject.SetActive(false);
    }

    private void SetActive()
    {
        objective1Active = true;
        // activar objeto de canvas con objetivo
    }

    private void SetDisabled()
    {
        objective1Active = false;
        // desactivar objeto de canvas con objetivo
    }
    */
}
