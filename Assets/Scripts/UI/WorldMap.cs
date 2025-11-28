
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class WorldMap : MonoBehaviour
{
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

    public void Grimm()
    {
        SceneManager.LoadScene("Test");
    }

    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("WorldMapScene");
    }
}
