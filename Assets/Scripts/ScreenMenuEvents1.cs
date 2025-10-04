using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;

public class ScreenMenuEvents1 : MonoBehaviour
{
    private UIDocument _document;

    public Button _GrimmButton;
    public Button _PlayButton;
    public Button _DecorButton;

    public Sprite idle;
    public Sprite active;
    int toggle = 0;

    private void Awake()
    {
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
        Debug.Log("Grimm");

    }
    private void OnPlayClick(ClickEvent evt)
    {

        Debug.Log("Play");
        if (toggle == 0)
        {
            _PlayButton.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Idle_Play.png"));
            toggle = 1;
        }
        else if (toggle == 1)
        {
            _PlayButton.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Active_Play.png"));
            toggle = 0;
        }

    }
    private void OnDecorClick(ClickEvent evt)
    {
        Debug.Log("Decor");
    }
}
