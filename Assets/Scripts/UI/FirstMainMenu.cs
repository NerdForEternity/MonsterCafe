using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstMainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BeginGame()
    {
        SceneManager.LoadScene("Test");
    }
}
