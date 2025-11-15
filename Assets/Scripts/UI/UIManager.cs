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

    public GameObject decorMenu;
    public GameObject warningMenu;

    // The visual elements that represent the root of each screen
    private VisualElement screenRoot;

    private VisualElement bannerRoot;

    //Utility variables
    int IdleToggle = 0;
    Label moneyCount;

    private void Start()
    {
        screenRoot = screenDocument.rootVisualElement;
        bannerRoot = bannerDocument.rootVisualElement;

        Button playButton = screenRoot.Q<Button>("Play");
        playButton?.RegisterCallback<ClickEvent>(evt => ActiveIdleSwap());

        moneyCount = screenRoot.Q<Label>("MoneyCount");
    }

    private void Update()
    {
        //moneyCount.text = UpgradeManager.totalMoney.ToString();
        moneyCount.text = PlayerPrefs.GetInt("Money", 0).ToString();
    }

    public void ActiveIdleSwap()
    {
        if (!decorMenu.activeSelf && !warningMenu.activeSelf)
        {
            Button playButton = screenRoot.Q<Button>("Play");
            VisualElement Banner = bannerRoot.Q<VisualElement>("ActiveIdleBanner");


            if (IdleToggle == 0)
            {
                playButton.style.backgroundImage = new StyleBackground(idle);
                Banner.style.backgroundImage = new StyleBackground(idleBanner);
                IdleToggle = 1;
            }
            else if (IdleToggle == 1)
            {
                playButton.style.backgroundImage = new StyleBackground(active);
                Banner.style.backgroundImage = new StyleBackground(activeBanner);
                IdleToggle = 0;

            }
        }
    }
}
