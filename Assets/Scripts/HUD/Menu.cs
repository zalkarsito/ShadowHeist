using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject pausePanel;

    private InputSystem_Actions inputActions;
    private InputAction pauseAction;

    private bool isGamePause = false;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
        pauseAction = inputActions.Player.Pause; //para tener el inout de pause del input action de player
    }
    void OnEnable()
    {
        inputActions.Enable();
        pauseAction.performed += OnPause;
    }

    void OnDisable()
    {
        pauseAction.performed -= OnPause;
        inputActions.Disable();
    }

    void OnPause(InputAction.CallbackContext context)
    {
        isGamePause = !isGamePause;
        PauseGame();
    }

    private void PauseGame()
    {
        if (isGamePause)
        {
            Time.timeScale = 0f;

            pausePanel.SetActive(true);
        }

        else
        {
            Time.timeScale = 1f;

            pausePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
