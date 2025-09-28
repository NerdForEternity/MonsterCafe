using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private Button _CampaignButton;
    private Button _UnlocksButton;
    private Button _SettingsButton;
    private Button _UpgradesButton;

    private VisualElement _Container;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _UpgradesButton = _document.rootVisualElement.Q("Upgrades") as Button;
        _SettingsButton = _document.rootVisualElement.Q("Settings") as Button;
        _UnlocksButton = _document.rootVisualElement.Q("Unlocks") as Button;
        _CampaignButton = _document.rootVisualElement.Q("Campaign") as Button;
        _Container = _document.rootVisualElement.Q("Container");

        _UpgradesButton.RegisterCallback<ClickEvent>(OnUpgradesClick);
        _SettingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        _UnlocksButton.RegisterCallback<ClickEvent>(OnUnlocksClick);
        _CampaignButton.RegisterCallback<ClickEvent>(OnCampaignClick);
    }

    private void OnDisable()
    {
        _UpgradesButton.UnregisterCallback<ClickEvent>(OnUpgradesClick);
        _SettingsButton.UnregisterCallback<ClickEvent>(OnSettingsClick);
        _UnlocksButton.UnregisterCallback<ClickEvent>(OnUnlocksClick);
        _CampaignButton.UnregisterCallback<ClickEvent>(OnCampaignClick);
    }
    private void OnUpgradesClick(ClickEvent evt)
    {
        Debug.Log("Upgrade");
        _Container.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Grimm_Upgrade.png"));

    }
    private void OnSettingsClick(ClickEvent evt)
    {
        Debug.Log("Settings");
        _Container.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Grimm_Settings.png"));
    }
    private void OnUnlocksClick(ClickEvent evt)
    {
        Debug.Log("Unlocks");
        _Container.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Grimm_Employee.png"));
    }
    private void OnCampaignClick(ClickEvent evt)
    {
        Debug.Log("Campaign");
        _Container.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Grimm_Campaign.png"));
    }
}
