using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    // Assign these in the Inspector
    [SerializeField] private UIDocument mainMenuDocument;
    [SerializeField] private UIDocument screenDocument;

    // The visual elements that represent the root of each screen
    private VisualElement mainMenuRoot;
    private VisualElement screenRoot;

    //Utility variables
    int IdleToggle = 0;



    private void Start()
    {

        mainMenuRoot = mainMenuDocument.rootVisualElement;
        screenRoot = screenDocument.rootVisualElement;

        ShowScreenMenu();

        Button grimmButton = screenRoot.Q<Button>("Grimm");
        grimmButton?.RegisterCallback<ClickEvent>(evt => ShowMainMenu());

        //Change from settings to dedicated "leave menu" button
        Button backButton = mainMenuRoot.Q<Button>("Settings");
        backButton?.RegisterCallback<ClickEvent>(evt => ShowScreenMenu());

        Button playButton = screenRoot.Q<Button>("Play");
        playButton?.RegisterCallback<ClickEvent>(evt => ActiveIdleSwap());

    }

    public void ShowMainMenu()
    {
        // SHOW the main menu
        mainMenuRoot.style.display = DisplayStyle.Flex;

        // HIDE the settings menu
        screenRoot.style.display = DisplayStyle.None;

    }

    public void ShowScreenMenu()
    {
        // SHOW the settings menu
        screenRoot.style.display = DisplayStyle.Flex;

        // HIDE the main menu
        mainMenuRoot.style.display = DisplayStyle.None;

    }

    public void ActiveIdleSwap()
    {
        Button playButton = screenRoot.Q<Button>("Play");

        
        if (IdleToggle == 0)
        {
            Debug.Log("Switch to Idle Play");
            playButton.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Idle_Play.png"));
            IdleToggle = 1;
        }
        else if (IdleToggle == 1)
        {
            Debug.Log("Switch to Active Play");
            playButton.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Active_Play.png"));
            IdleToggle = 0;
        }
    }
}
