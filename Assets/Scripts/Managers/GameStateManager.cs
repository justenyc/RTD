using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    [SerializeField] bool isPaused = false;

    public bool IsPaused {
        get => isPaused;
        set
        {
            isPaused = value;
            Logger.LogMessage($"Changing isPaused to {value}");
            if(isPaused)
            {
                Time.timeScale = 0;
                return;
            }

            Time.timeScale = 1;
        }
    }
    
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }
}
