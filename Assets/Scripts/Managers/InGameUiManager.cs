using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InGameUiManager : MonoBehaviour
{
    public static InGameUiManager instance;

    [Header("References")]
    [SerializeField] TextMeshProUGUI messageTMP;

    [Header("Settings")]
    [SerializeField] float nextCharDelayTime = 0.1f;
    public System.Action onMessageComplete;

    [Header("Debug")]
    [SerializeField] int messageIndex = 0;
    [SerializeField] List<string> messageToDisplay = new();

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

    public void DisplayMessage(List<string> message, System.Action _onMessageComplete = null)
    {
        InputHandler.instance.Interact += IterateMessageIndex;

        messageToDisplay = message;
        onMessageComplete = _onMessageComplete;
        displayMessageRoutine = StartCoroutine(DisplayMessageTextRoutine(messageToDisplay[messageIndex]));
        
        GameStateManager.instance.IsPaused = true;
    }

    public void DisplayMessage(string message)
    {
        List<string> stringList = new List<string>() { message };
        DisplayMessage(stringList);
    }

    void IterateMessageIndex(InputAction.CallbackContext context)
    {
        messageIndex++;
        if (messageIndex < messageToDisplay.Count)
        {
            if (displayMessageRoutine == null)
            {
                displayMessageRoutine = StartCoroutine(DisplayMessageTextRoutine(messageToDisplay[messageIndex]));
                return;
            }
        }

        if(displayMessageRoutine != null)
        {
            StopAllCoroutines();
            displayMessageRoutine = null;
            messageIndex--;
            messageTMP.text = messageToDisplay[messageIndex];
            return;
        }

        messageIndex = 0;
        messageToDisplay = new();
        messageTMP.text = "";
        GameStateManager.instance.IsPaused = false;
        InputHandler.instance.Interact -= IterateMessageIndex;
        onMessageComplete?.Invoke();
        onMessageComplete = null;
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