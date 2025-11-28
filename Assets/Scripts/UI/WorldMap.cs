
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class WorldMap : MonoBehaviour
{
    public UnityEngine.UI.Button vamp2Button;
    public UnityEngine.UI.Button vamp3Button;
    public UnityEngine.UI.Button werewolf2Button;
    public UnityEngine.UI.Button werewolf3Button;
    public void Start()
    {
        if(PlayerPrefs.GetInt("Vampire1", 0) == 0)
            vamp2Button.interactable = false;
        if(PlayerPrefs.GetInt("Vampire2", 0) == 0)
            vamp3Button.interactable = false;
        if(PlayerPrefs.GetInt("Werewolf1", 0) == 0)
            werewolf2Button.interactable = false;
        if(PlayerPrefs.GetInt("Werewolf2", 0) == 0)
            werewolf3Button.interactable = false;
    }
    public void WolfDinerOne()
    {
        SceneManager.LoadScene("Werewolf_1");
    }

    public void WolfDinerTwo()
    {
        SceneManager.LoadScene("Werewolf_2");
    }

    public void WolfDinerThree()
    {
        SceneManager.LoadScene("Werewolf_3");
    }

    public void VampiniOne()
    {
        SceneManager.LoadScene("Vampire_1");
    }

    public void VampiniTwo()
    {
        SceneManager.LoadScene("Vampire_2");
    }

    public void VampiniThree()
    {
        SceneManager.LoadScene("Vampire_3");
    }

    /*public void Grimm()
    {
        int numLevels = PlayerPrefs.GetInt("LevelsWon", 0);

        if(numLevels < 3)
            SceneManager.LoadScene("Test");
        else if(numLevels < 6)
            SceneManager.LoadScene("Upgrade1");
        else
            SceneManager.LoadScene("Upgrade2");
    }*/
}
