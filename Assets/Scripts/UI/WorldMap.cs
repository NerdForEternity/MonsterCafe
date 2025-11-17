
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class WorldMap : MonoBehaviour
{
    public void WolfDinerOne()
    {
        SceneManager.LoadScene("WolfTestScene");
    }

    public void WolfDinerTwo()
    {
        SceneManager.LoadScene("WolfTestScene");
    }

    public void WolfDinerThree()
    {
        SceneManager.LoadScene("WolfTestScene");
    }

    public void VampiniOne()
    {
        SceneManager.LoadScene("Vampire_Decor");
    }

    public void VampiniTwo()
    {
        SceneManager.LoadScene("VampTestScene");
    }

    public void VampiniThree()
    {
        SceneManager.LoadScene("VampTestScene");
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
