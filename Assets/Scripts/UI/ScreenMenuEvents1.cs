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
    AudioManager audioManager;
    public InputActionAsset InputActions;
    public GameObject idleWarningScreen;
    private bool idleWarningDisplayed = false;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        _document = GetComponent<UIDocument>();

        _GrimmButton = _document.rootVisualElement.Q("Grimm") as Button;
        _PlayButton = _document.rootVisualElement.Q("Play") as Button;
        _DecorButton = _document.rootVisualElement.Q("Decor") as Button;

        _GrimmButton.RegisterCallback<ClickEvent>(OnGrimmClick);
        _PlayButton.RegisterCallback<ClickEvent>(OnPlayClick);
        _DecorButton.RegisterCallback<ClickEvent>(OnDecorClick);
    }

    private void OnDisable()
    {
        _GrimmButton.UnregisterCallback<ClickEvent>(OnGrimmClick);
        _PlayButton.UnregisterCallback<ClickEvent>(OnPlayClick);
        _DecorButton.UnregisterCallback<ClickEvent>(OnDecorClick);
    }
    private void OnGrimmClick(ClickEvent evt)
    {
        audioManager.PlaySFX(audioManager.openingJournal);
        if (!isSceneLoaded("GrimmJournal"))
        {
            InputActions.FindActionMap("Player").Disable();
            SceneManager.LoadScene("GrimmJournal", LoadSceneMode.Additive);
        }
    }
    private void OnPlayClick(ClickEvent evt)
    {
        if (toggle == 0)
        {
            if (!idleWarningDisplayed)
            {
                Instantiate(idleWarningScreen);
                idleWarningDisplayed = true;
            }
            _PlayButton.style.backgroundImage = new StyleBackground(idleSprite);
            customerManager.idle = true;
            toggle = 1;
        }
        else if (toggle == 1)
        {
            _PlayButton.style.backgroundImage = new StyleBackground(activeSprite);
            customerManager.idle = false;
            toggle = 0;
        }

    }
    private void OnDecorClick(ClickEvent evt)
    {
        Debug.Log("Decor");
        decorMenu.SetActive(true);
        Time.timeScale = 0f;
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
