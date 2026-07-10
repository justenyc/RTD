using UnityEngine;
using TMPro;
using System.Collections;

public class InGameUiManager : MonoBehaviour
{
    public static InGameUiManager instance;

    [Header("References")]
    [SerializeField] TextMeshProUGUI messageTMP;

    [Header("Settings")]
    [SerializeField] float nextCharDelayTime = 0.1f;

    Coroutine displayMessageRoutine;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    public void DisplayMessage(string message)
    {
        if (displayMessageRoutine != null)
        {
            StopCoroutine(displayMessageRoutine);
        }
        displayMessageRoutine = StartCoroutine(DisplayMessageTextRoutine(message));
    }

    IEnumerator DisplayMessageTextRoutine(string message)
    {
        messageTMP.text = "";

        for (int ii = 0; ii < message.Length; ii++)
        {
            messageTMP.text = messageTMP.text + message[ii];
            yield return new WaitForSecondsRealtime(nextCharDelayTime);
        }

        displayMessageRoutine = null;
    }
}
