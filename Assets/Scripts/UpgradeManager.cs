using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    //total money the player has earned
    public int totalMoney = 0;
    //food takes 0.5 seconds less to prepare, max of 3 upgrades
    public int cookSpeedAdd = 0;
    //customers wait for 1 second longer, max of 3 upgrades
    public float patienceAdd = 0f;
    //increases price of everything by 20%, no cap
    public int priceAdd = 0;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
