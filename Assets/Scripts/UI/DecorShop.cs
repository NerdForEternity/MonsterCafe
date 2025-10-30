using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DecorShop : MonoBehaviour
{
    public Scene ghostPaletteScene;
    public Scene vampirePaletteScene;
    public GameObject decorMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Start()
    {
        decorMenu.SetActive(false);
    }

    public void GoBack()
    {
        decorMenu.SetActive(false);
    }
    public void GhostPalette()
    {
        SceneManager.LoadScene("Test");
    }

    public void VampireScene()
    {
        SceneManager.LoadScene("Vampire_Decor");
    }
}


