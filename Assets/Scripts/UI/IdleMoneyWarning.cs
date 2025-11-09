
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using Unity.VisualScripting;

public class IdleMoneyWarning : MonoBehaviour
{
    public GameObject warningMessage;
    public int secondsToWait = 5;
    public void CheckboxPress()
    {
        Debug.Log("Checkbox Clicked");
        warningMessage.SetActive(false);
    }


}
