using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Assign these in the Inspector
    [SerializeField] private UIDocument screenDocument;
    [SerializeField] private UIDocument bannerDocument;
    public Sprite idle;
    public Sprite active;
    public Sprite activeBanner;
    public Sprite idleBanner;

    // The visual elements that represent the root of each screen
    private VisualElement screenRoot;

    private VisualElement bannerRoot;

    //Utility variables
    int IdleToggle = 0;
    Label moneyCount;
    public UpgradeManager upgradesManager;

    private void Start()
    {
        screenRoot = screenDocument.rootVisualElement;
        bannerRoot = bannerDocument.rootVisualElement;

        //Change from settings to dedicated "leave menu" button
        //Button backButton = screenRoot.Q<Button>("Settings");
        //backButton?.RegisterCallback<ClickEvent>(evt => ShowScreenMenu());

        Button playButton = screenRoot.Q<Button>("Play");
        playButton?.RegisterCallback<ClickEvent>(evt => ActiveIdleSwap());

        Button goButton = screenRoot.Q<Button>("GoButton");
        goButton?.RegisterCallback<ClickEvent>(evt => ShowWorldMap());


        moneyCount = screenRoot.Q<Label>("MoneyCount");
    }

    private void Update()
    {
        moneyCount.text = UpgradeManager.totalMoney.ToString();
    }

    /*public void ShowScreenMenu()
    {
        // SHOW the settings menu
        screenRoot.style.display = DisplayStyle.Flex;

        // HIDE the main menu
        SceneManager.UnloadSceneAsync("GrimmJournal");
    }*/

    public void ActiveIdleSwap()
    {
        Button playButton = screenRoot.Q<Button>("Play");
        VisualElement Banner = bannerRoot.Q<VisualElement>("ActiveIdleBanner");


        if (IdleToggle == 0)
        {
            //Debug.Log("Switch to Idle Play");
            playButton.style.backgroundImage = new StyleBackground(idle);
            Banner.style.backgroundImage = new StyleBackground(idleBanner);
            IdleToggle = 1;
        }
        else if (IdleToggle == 1)
        {
            //Debug.Log("Switch to Active Play");
            playButton.style.backgroundImage = new StyleBackground(active);
            Banner.style.backgroundImage = new StyleBackground(activeBanner);
            IdleToggle = 0;

        }
    }

    public void ShowWorldMap()
    {
        SceneManager.LoadScene("WorldMapScene", LoadSceneMode.Additive);
        //Debug.Log("tried to load map");
    }
}
