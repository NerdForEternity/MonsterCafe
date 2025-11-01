using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using TMPro;

public class GrimmJournal : MonoBehaviour
{
    //UI Variables
    public Image backgroundImage; // Assign the Image component from the Inspector
    public Sprite[] backgroundSprites; // Array of sprites to switch between
    public GameObject upgradesPage;
    public GameObject campaignPage;
    public GameObject unlocksPage;
    public GameObject settingsPage;
    //Upgrades
    public GameObject cookingSpeedPage;
    private GameObject cookingText;
    private GameObject cookingPriceText;
    public GameObject customerPatiencePage;
    private GameObject patienceText;
    private GameObject patiencePriceText;
    public GameObject foodPricePage;
    public GameObject priceText;
    public GameObject foodPriceText;
    //Settings
    public GameObject audioSettingsPage;
    public GameObject gameplaySettingsPage;

    private int currentSpriteIndex = 0;
    private int foodIndex;

    // Audio
    public AudioMixer Mixer;
    AudioManager audioManager;
    AudioSource musicSource;
    AudioSource sfxSource;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Button masterButton;
    public Button musicButton;
    public Button sfxButton;
    int masterFlag = 0;
    int musicFlag = 0;
    int sfxFlag = 0;
    public Sprite[] muteSprites;

    //Gameplay
    public Slider sensitivitySlider;
    public Toggle xInvertToggle;
    public Toggle yInvertToggle;
    public InputActionAsset InputActions;

    public void ChangeBackground(int targetBackground)
    {
        if (backgroundImage != null && backgroundSprites != null && backgroundSprites.Length > 0)
        {
            currentSpriteIndex = targetBackground;
            backgroundImage.sprite = backgroundSprites[currentSpriteIndex];
        }
        else
        {
            Debug.LogWarning("Background Image or Sprites not assigned!");
        }
    }

    private void Start()
    {
        //Audio Sources
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        musicSource = audioManager.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        sfxSource = audioManager.transform.GetChild(1).gameObject.GetComponent<AudioSource>();

        //Sliders
        masterSlider.value = PlayerPrefs.GetFloat("Volume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        sensitivitySlider.value = PlayerPrefs.GetFloat("CamSensitivity", 20.5f);
        
        if (PlayerPrefs.GetInt("XInvert") == 1)
            xInvertToggle.isOn = true;
        else
            xInvertToggle.isOn = false;
        if (PlayerPrefs.GetInt("YInvert") == 1)
            yInvertToggle.isOn = true;
        else
            yInvertToggle.isOn = false;

        masterFlag = PlayerPrefs.GetInt("MasterMute", 0);
        musicFlag = PlayerPrefs.GetInt("MusicMute", 0);
        sfxFlag = PlayerPrefs.GetInt("SFXMute", 0);

        masterButton.image.sprite = muteSprites[masterFlag];
        musicButton.image.sprite = muteSprites[musicFlag];
        sfxButton.image.sprite = muteSprites[sfxFlag];

        UpgradesPage();
    }

    public void ShowPanel(GameObject panelToShow)
    {
        // Deactivate all panels
        campaignPage.SetActive(false);
        upgradesPage.SetActive(false);
        unlocksPage.SetActive(false);
        settingsPage.SetActive(false);

        cookingSpeedPage.SetActive(false);
        customerPatiencePage.SetActive(false);
        foodPricePage.SetActive(false);

        audioSettingsPage.SetActive(false);
        gameplaySettingsPage.SetActive(false);

        // Activate the selected panel
        panelToShow.SetActive(true);
    }

    //Page Changes
    public void CampaignPage()
    {
        ChangeBackground(0);
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(campaignPage);

        if (!isSceneLoaded("WorldMapScene"))
            SceneManager.LoadScene("WorldMapScene", LoadSceneMode.Additive);
    }
    public void UpgradesPage()
    {
        if (isSceneLoaded("WorldMapScene"))
            SceneManager.UnloadSceneAsync("WorldMapScene");
        ChangeBackground(1);
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(upgradesPage);
    }
    public void UnlocksPage()
    {
        if (isSceneLoaded("WorldMapScene"))
            SceneManager.UnloadSceneAsync("WorldMapScene");
        ChangeBackground(2);
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(unlocksPage);
    }
    public void SettingsPage()
    {
        if (isSceneLoaded("WorldMapScene"))
            SceneManager.UnloadSceneAsync("WorldMapScene");
        ChangeBackground(3);
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(settingsPage);
    }

    //Upgrade Pages

    public void CookingSpeedPage()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(upgradesPage);

        cookingSpeedPage.SetActive(true);
        cookingText = cookingSpeedPage.transform.GetChild(2).gameObject;
        cookingPriceText = cookingSpeedPage.transform.GetChild(3).gameObject;

        cookingText.GetComponent<TMP_Text>().text = ("LVL: " + UpgradeManager.cookSpeedAdd.ToString());
        if (UpgradeManager.cookSpeedAdd < 3)
            cookingPriceText.GetComponent<TMP_Text>().text = ("COST: " + (2 * (UpgradeManager.cookSpeedAdd + 1)));
        else
            cookingPriceText.GetComponent<TMP_Text>().text = ("COST: SOLD OUT");
    }

    public void CustomerPatiencePage()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(upgradesPage);

        customerPatiencePage.SetActive(true);
        patienceText = customerPatiencePage.transform.GetChild(2).gameObject;
        patiencePriceText = customerPatiencePage.transform.GetChild(3).gameObject;

        patienceText.GetComponent<TMP_Text>().text = ("LVL: " + UpgradeManager.patienceAdd.ToString());
        if (UpgradeManager.patienceAdd < 3)
            patiencePriceText.GetComponent<TMP_Text>().text = ("COST: " + (2 * (UpgradeManager.patienceAdd + 1)));
        else
            patiencePriceText.GetComponent<TMP_Text>().text = ("COST: SOLD OUT");
    }

    public void FoodPricePage()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(upgradesPage);

        foodPricePage.SetActive(true);
        priceText = foodPricePage.transform.GetChild(2).gameObject;
        foodPriceText = foodPricePage.transform.GetChild(3).gameObject;

        UpdatePrice();
    }

    //Upgrade Functions
    public void upgradeCooking()
    {
        int price = 2 * (UpgradeManager.cookSpeedAdd + 1);
        if (UpgradeManager.cookSpeedAdd < 3 && UpgradeManager.totalMoney >= price)
        {
            UpgradeManager.cookSpeedAdd++;
            UpgradeManager.totalMoney -= price;
            cookingText.GetComponent<TMP_Text>().text = ("LVL: " + UpgradeManager.cookSpeedAdd.ToString());

            if (UpgradeManager.cookSpeedAdd < 3)
                cookingPriceText.GetComponent<TMP_Text>().text = ("COST: " + 2 * (UpgradeManager.cookSpeedAdd + 1));
            else
                cookingPriceText.GetComponent<TMP_Text>().text = ("COST: SOLD OUT");
        }
    }
    public void upgradeCustomer()
    {
        int price = 2 * ((int)UpgradeManager.patienceAdd + 1);
        if (UpgradeManager.patienceAdd < 3 && UpgradeManager.totalMoney >= price)
        {
            UpgradeManager.patienceAdd++;
            UpgradeManager.totalMoney -= price;
            patienceText.GetComponent<TMP_Text>().text = ("LVL: " + UpgradeManager.patienceAdd.ToString());

            if (UpgradeManager.patienceAdd < 3)
                patiencePriceText.GetComponent<TMP_Text>().text = ("COST: " + (2 * (UpgradeManager.patienceAdd + 1)));
            else
                patiencePriceText.GetComponent<TMP_Text>().text = ("COST: SOLD OUT");
        }
    }

    public void upgradeFood()
    {
        float upgradePrice = 2f;
        if (UpgradeManager.orderList[foodIndex].numUpgrades > 0)
        {
            for (int i = 0; i < UpgradeManager.orderList[foodIndex].numUpgrades; i++)
                upgradePrice = upgradePrice * 1.2f;
        }
        upgradePrice = Mathf.Round(upgradePrice);

        if (UpgradeManager.totalMoney >= (int)upgradePrice)
        {
            UpgradeManager.totalMoney -= (int)upgradePrice;

            UpgradeManager.orderList[foodIndex].numUpgrades++;
        }
        UpdatePrice();
    }

    public void UpdatePrice()
    {
        priceText.GetComponent<TMP_Text>().text = ("LVL: " + UpgradeManager.orderList[foodIndex].numUpgrades.ToString());

        float upgradePrice = 2f;
        if (UpgradeManager.orderList[foodIndex].numUpgrades > 0)
        {
            for (int i = 0; i < UpgradeManager.orderList[foodIndex].numUpgrades; i++)
                upgradePrice = upgradePrice * 1.2f;
        }
        upgradePrice = Mathf.Round(upgradePrice);

        foodPriceText.GetComponent<TMP_Text>().text = ("COST: " + upgradePrice);
    }

    //Back Button Functionality
    public void GoBack()
    {
        if (isSceneLoaded("GrimmJournal"))
            SceneManager.UnloadSceneAsync("GrimmJournal");
        if (isSceneLoaded("WorldMapScene"))
            SceneManager.UnloadSceneAsync("WorldMapScene");

        InputActions.FindActionMap("Player").Enable();
    }

    //Select Food Item
    public void SelectCoffee()
    {
        foodIndex = 0;
        UpdatePrice();
    }

    public void SelectSoda()
    {
        foodIndex = 1;
        UpdatePrice();
    }

    public void SelectMartini()
    {
        foodIndex = 2;
        UpdatePrice();
    }
    public void SelectBurger()
    {
        foodIndex = 3;
        UpdatePrice();
    }
    public void SelectPanini()
    {
        foodIndex = 4;
        UpdatePrice();
    }

    //Settings Pages
    public void AudioSettingsPage()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(settingsPage);

        audioSettingsPage.SetActive(true);
    }
    public void GameplaySettingsPage()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(settingsPage);

        gameplaySettingsPage.SetActive(true);
    }
    //Audio Settings
    public void ChangeMasterAudio(float value)
    {
        if (PlayerPrefs.GetInt("MasterMute") == 1)
            Mixer.SetFloat("Volume", -80f);
        else
            Mixer.SetFloat("Volume", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
    public void ChangeMusicAudio(float value)
    {
        if (PlayerPrefs.GetInt("MusicMute") == 1)
            musicSource.volume = 0f;
        else
            musicSource.volume = value;

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }
    public void ChangeSFXAudio(float value)
    {
        if (PlayerPrefs.GetInt("SFXMute") == 1)
            sfxSource.volume = 0f;
        else
            sfxSource.volume = value;

        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    public void MuteMaster()
    {
        //mute
        if (masterFlag == 0)
        {
            masterFlag = 1;
            PlayerPrefs.SetInt("MasterMute", 1);
            Mixer.SetFloat("Volume", -80f);
        }
        //unmute
        else
        {
            masterFlag = 0;
            PlayerPrefs.SetInt("MasterMute", 0);
            Mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume")) * 20);
        }
        masterButton.image.sprite = muteSprites[PlayerPrefs.GetInt("MasterMute")];
    }

    public void MuteMusic()
    {
        //mute
        if (musicFlag == 0)
        {
            musicFlag = 1;
            PlayerPrefs.SetInt("MusicMute", 1);
            musicSource.volume = 0f;
        }
        //unmute
        else
        {
            musicFlag = 0;
            PlayerPrefs.SetInt("MusicMute", 0);
            musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        }
        musicButton.image.sprite = muteSprites[PlayerPrefs.GetInt("MusicMute")];
    }
    public void MuteSFX()
    {
        //mute
        if (sfxFlag == 0)
        {
            sfxFlag = 1;
            PlayerPrefs.SetInt("SFXMute", 1);
            sfxSource.volume = 0f;
        }
        //unmute
        else
        {
            sfxFlag = 0;
            PlayerPrefs.SetInt("SFXMute", 0);
            sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume");
        }
        sfxButton.image.sprite = muteSprites[PlayerPrefs.GetInt("SFXMute")];
    }

    //Gameplay Settings
    public void ChangeSensitivity(float value)
    {
        CameraControls.sensitivity = value;
        PlayerPrefs.SetFloat("CamSensitivity", value);
    }
    public void XInvert()
    {
        if (xInvertToggle.isOn)
            CameraControls.xInvert = 1;
        else
            CameraControls.xInvert = -1;

        PlayerPrefs.SetInt("XInvert", CameraControls.xInvert);
    }
    public void YInvert()
    {
        if (yInvertToggle.isOn)
            CameraControls.yInvert = 1;
        else
            CameraControls.yInvert = -1;

        PlayerPrefs.SetInt("YInvert", CameraControls.yInvert);
    }
    public void ChangeSpeed(float value)
    {
        Time.timeScale = (float)value;
        PlayerPrefs.SetFloat("GameSpeed", (float)value);
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
