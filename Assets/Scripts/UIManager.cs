using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    // Assign these in the Inspector
    [SerializeField] private UIDocument mainMenuDocument;
    [SerializeField] private UIDocument screenDocument;
    public Sprite idle;
    public Sprite active;

    // The visual elements that represent the root of each screen
    private VisualElement mainMenuRoot;
    private VisualElement screenRoot;

    //Utility variables
    int IdleToggle = 0;
    Label moneyCount;
    private CustomerManager customerManager;

    private void Start()
    {
        customerManager = this.GetComponent<CustomerManager>();

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

        moneyCount = screenRoot.Q<Label>("MoneyCount");
    }

    private void Update()
    {
        moneyCount.text = customerManager.numServed.ToString();
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
            playButton.style.backgroundImage = new StyleBackground(idle);
            IdleToggle = 1;
        }
        else if (IdleToggle == 1)
        {
            Debug.Log("Switch to Active Play");
            playButton.style.backgroundImage = new StyleBackground(active);
            IdleToggle = 0;
        }
    }
}
