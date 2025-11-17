using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ScreenMenuEvents1 : MonoBehaviour
{
    private UIDocument _document;

    public Button _GrimmButton;
    public Button _PlayButton;
    public Button _DecorButton;

    public GameObject decorMenu;
    public Sprite idleSprite;
    public Sprite activeSprite;
    public CustomerManager customerManager;
    int toggle = 0;
    public AudioManager audioManager;
    public InputActionAsset InputActions;
    public GameObject idleWarningScreen;
    private IdleMoneyWarning idleMoneyWarning;
    private bool warningActive;
    private bool idleWarningDisplayed = false;
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        idleMoneyWarning = idleWarningScreen.GetComponent<IdleMoneyWarning>();
        idleMoneyWarning.audioManager = this.audioManager;
        idleMoneyWarning.InputActions = this.InputActions;
        

        _GrimmButton = _document.rootVisualElement.Q("Grimm") as Button;
        _PlayButton = _document.rootVisualElement.Q("Play") as Button;
        _DecorButton = _document.rootVisualElement.Q("Decor") as Button;

        _GrimmButton.RegisterCallback<ClickEvent>(OnGrimmClick);
        _PlayButton.RegisterCallback<ClickEvent>(OnPlayClick);
        _DecorButton.RegisterCallback<ClickEvent>(OnDecorClick);
    }

    void Update()
    {
        Debug.Log(InputActions.FindActionMap("Player").enabled);
    }
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        _GrimmButton.UnregisterCallback<ClickEvent>(OnGrimmClick);
        _PlayButton.UnregisterCallback<ClickEvent>(OnPlayClick);
        _DecorButton.UnregisterCallback<ClickEvent>(OnDecorClick);
    }
    private void OnGrimmClick(ClickEvent evt)
    {
        //if popup, shop, or grimm is open
        if (!idleWarningScreen.activeSelf && !decorMenu.activeSelf && !isSceneLoaded("GrimmJournal"))
        {
            audioManager.PlaySFX(audioManager.openingJournal);

            InputActions.FindActionMap("Player").Disable();
            SceneManager.LoadScene("GrimmJournal", LoadSceneMode.Additive);  
        }
    }
    private void OnPlayClick(ClickEvent evt)
    {
        //if popup, shop, or grimm is open
        if (!idleWarningScreen.activeSelf && !decorMenu.activeSelf && !isSceneLoaded("GrimmJournal"))
        {
            if (toggle == 0)
            {
                if (idleWarningDisplayed)
                    audioManager.PlaySFX(audioManager.buttonClick);

                else if (!idleWarningDisplayed)
                {
                    idleWarningScreen.SetActive(true);
                    InputActions.FindActionMap("Player").Disable();
                    idleWarningDisplayed = true;
                }

                _PlayButton.style.backgroundImage = new StyleBackground(idleSprite);
                customerManager.idle = true;
                toggle = 1;
            }

            else if (toggle == 1)
            {
                audioManager.PlaySFX(audioManager.buttonClick);

                _PlayButton.style.backgroundImage = new StyleBackground(activeSprite);
                customerManager.idle = false;
                toggle = 0;
            }
        }
    }
    private void OnDecorClick(ClickEvent evt)
    {
        //if popup, shop, or grimm is open
        if (!idleWarningScreen.activeSelf && !decorMenu.activeSelf && !isSceneLoaded("GrimmJournal"))
        {
            audioManager.PlaySFX(audioManager.buttonClick);
            decorMenu.SetActive(true);
            InputActions.FindActionMap("Player").Disable();
            Time.timeScale = 0f;
        }
    }

    public bool isSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == sceneName)
                return true;
        }
        return false;
    }
}
