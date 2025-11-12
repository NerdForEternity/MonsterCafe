
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class IdleMoneyWarning : MonoBehaviour
{
    public GameObject warningMessage;
    public AudioManager audioManager;
    public InputActionAsset InputActions;
    public int secondsToWait = 5;

    void Start()
    {
        audioManager.PlaySFX(audioManager.notif);
        Time.timeScale = 0f;
    }
    public void CheckboxPress()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        Time.timeScale = PlayerPrefs.GetFloat("GameSpeed", 1f);
        InputActions.FindActionMap("Player").Enable();
        warningMessage.SetActive(false);
    }
}
