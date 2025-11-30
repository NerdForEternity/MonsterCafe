using UnityEngine;

public class Employee : MonoBehaviour
{
    public enum UnitType
    {
        Ghost, Vampire, Werewolf
    }
    public UnitType myType;
    public Animator animator;
    public bool isUnlocked;

    public void Start()
    {
        // if this is a vampire and the vampire employee has been unlocked
        if((int)myType == 1  && PlayerPrefs.GetInt("Vampire2", 0) == 1)
            isUnlocked = true;
        // if this is a werewolf and the werewolf employee has been unlocked
        else if((int)myType == 2 && PlayerPrefs.GetInt("Werewolf2", 0) == 1)
            isUnlocked = true;
        // else, if this is not Nico, disable
        else if((int)myType != 0)
            this.gameObject.SetActive(false);

        animator = this.transform.GetChild(0).GetComponent<Animator>();
    }
}
