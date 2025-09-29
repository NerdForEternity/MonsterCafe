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

    private void Start()
    {

        mainMenuRoot = mainMenuDocument.rootVisualElement;
        screenRoot = screenDocument.rootVisualElement;

        ShowScreenMenu();

        Button grimmButton = screenRoot.Q<Button>("Grimm");
        grimmButton?.RegisterCallback<ClickEvent>(evt => ShowMainMenu());

        Button backButton = mainMenuRoot.Q<Button>("Settings");
        backButton?.RegisterCallback<ClickEvent>(evt => ShowScreenMenu());

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
}
