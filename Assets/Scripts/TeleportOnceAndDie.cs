using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportOnceAndDie : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void TheVoices()
    {
        SceneManager.LoadScene("Test");
        Debug.Log("ugh uh duh");
    }
}
