using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UnitManager : MonoBehaviour
{
    // objects
    public List<GameObject> units; // references to unit types
    public List<GameObject> vampireTypes; // vampire variants;
    public List<GameObject> werewolfTypes; // werewolf variants;
    public GameObject[] spawnTiles; // references to spawn tiles in scene
    public GameObject[] targetTiles; // references to enemy spawn tiles
    public List<GameObject> spawnedUnits = new List<GameObject>(); // all player units in the scene
    public List<GameObject> enemies = new List<GameObject>(); // all enemies in scene
    private GameObject currentUnit; // current unit reference being used
    private GameObject previewUnit; // transparent preview of current unit 
    private GameObject currentTile; // tile to place current unit on
    public GameObject winScreen;
    public GameObject loseScreen;

    // input
    public InputActionAsset InputActions;
    private InputAction m_hitScreen;
    private Vector3 clickPos;
    public Button vampButton;
    public Button vampBuy;
    public Button werewolfButton;
    public Button werewolfBuy;

    // managers
    public ShopManager shopManager;
    public CameraControls cameraControls;

    // food unlocks
    public FoodItem martini;
    public FoodItem panini;
    public FoodItem soda;
    public FoodItem burger;

    // flags
    private bool clickOnTile; // checks if initial click was on a tile
    private int currentID; // determines which unit is selected
    public bool waveStarted; // checks if wave has started yet
    public int numRounds; // number of rounds played
    public int maxRounds; // maximum number of rounds to complete level
    public int levelNumber; // determines which level this is to configure unlocks
    public int levelType; // determines if level is vampire (0) or werewolf (1)

    private void Awake()
    {
        m_hitScreen = InputSystem.actions.FindAction("Click");

        m_hitScreen.started += context =>
        {
            Vector3 clickPos = m_hitScreen.ReadValue<Vector2>();
            clickPos = Camera.main.ScreenToWorldPoint(clickPos);

            if (spawnTiles.Any(a => a.GetComponent<Collider2D>().OverlapPoint(clickPos)))
            {
                GameObject newTile = spawnTiles.First(a => a.GetComponent<Collider2D>().OverlapPoint(clickPos));
                //show preview of placement if:
                // - there isnt already a preview
                // - the player has enough of that unit
                // - there isnt already a unit on that tile that isnt the selected unit
                // - the wave hasn't started
                if (previewUnit == null &&
                units[currentID].GetComponent<Unit>().numInInventory > 0 && 
                (newTile.GetComponent<SpawnTile>().unitType == null || newTile.GetComponent<SpawnTile>().unitType.myType == units[currentID].GetComponent<Unit>().myType) &&
                !waveStarted)
                {
                    clickOnTile = true;
                    CreatePreview(newTile.GetComponent<SpawnTile>().tileID);
                }
            }
            else
                clickOnTile = false;
        };
    }
    
    void Start()
    {
        // sets the ghost as default unit to spawn
        currentUnit = units[0];

        // check if vamp/werewolf units are unlocked
        if(PlayerPrefs.GetInt("Vampire2", 0) == 0)
        {
            vampButton.interactable = false;
            vampBuy.interactable = false;
        }
        else
        {
            vampButton.interactable = true;
            vampBuy.interactable = true;
        }
        
        if(PlayerPrefs.GetInt("Werewolf2", 0) == 0)
        {
            werewolfButton.interactable = false;
            werewolfBuy.interactable = false;
        }
        else
        {
            werewolfButton.interactable = true;
            werewolfBuy.interactable = true;
        }

        // resets all unit counts and updates shop display
        for (int i = 0; i < units.Count; i++)
        {
            units[i].GetComponent<Unit>().numInInventory = 0;
            units[i].GetComponent<Unit>().numBought = 0; // resets unit price to default
            shopManager.UpdatePrice(i); // update unit price text
            shopManager.UpdateAmount(i); // update amount in inventory
        }

        // player has two ghosts by default
        units[0].GetComponent<Unit>().numInInventory=+ 2;
        shopManager.UpdateAmount(0);

        numRounds = 1;
        shopManager.UpdateRounds();
    }

    void Update()
    {
        // if the player has units to place AND wave hasn't started
        if(units[currentID].GetComponent<Unit>().numInInventory > 0 && !waveStarted)
        {
            // mouse/finger is currently held down and initial click was on a tile, track position
            if ((int)m_hitScreen.phase == 2 && clickOnTile)
            {
                // disable camera movement
                cameraControls.enabled = false;

                clickPos = m_hitScreen.ReadValue<Vector2>();
                clickPos = Camera.main.ScreenToWorldPoint(clickPos);

                // mouse/finger is over a tile
                if (spawnTiles.Any(a => a.GetComponent<Collider2D>().OverlapPoint(clickPos)))
                {
                    GameObject newTile = spawnTiles.First(a => a.GetComponent<Collider2D>().OverlapPoint(clickPos));

                    // mouse/finger is over a new tile...
                    // and there isnt a unit there that isnt the current unit
                    if (newTile != currentTile && 
                    (newTile.GetComponent<SpawnTile>().unitType == null || newTile.GetComponent<SpawnTile>().unitType.myType == units[currentID].GetComponent<Unit>().myType))
                    {
                        currentTile = newTile;
                        CreatePreview(currentTile.GetComponent<SpawnTile>().tileID);
                    }
                }
            }
            // player has released hold, destroy preview
            else if ((int)m_hitScreen.phase == 1 && previewUnit != null)
            {
                // enable camera movement
                cameraControls.enabled = true;
                Destroy(previewUnit);

                // player has mouse/finger on a tile, spawn unit at that tile
                if (spawnTiles.Any(a => a.GetComponent<Collider2D>().OverlapPoint(clickPos)))
                {
                    //if there is already a unit here...
                    SpawnTile spawnTile = currentTile.GetComponent<SpawnTile>();
                    if(spawnTile.unitType != null)
                    {   
                        //get reference to that unit
                        Unit existingUnit = currentUnit.GetComponent<Unit>();
                        //check if the unit being spawned is the same as existing unit
                        if(existingUnit.myType == spawnTile.unitType.myType)
                        {
                            spawnTile.unitType.ChangeHealth(2);
                            
                            units[currentID].GetComponent<Unit>().numInInventory--;
                            shopManager.UpdateAmount(currentID);
                        }
                    }
                    //if there isn't a unit here...
                    else
                    {
                        GameObject tileBaseUnit = Instantiate(currentUnit, currentTile.transform.position, currentUnit.transform.rotation); // spawn unit
                        spawnedUnits.Add(tileBaseUnit); // add to total player units
                        spawnTile.unitType = tileBaseUnit.GetComponent<Unit>(); // set tile unit type
                        int targetID = System.Array.IndexOf(spawnTiles, currentTile);
                        tileBaseUnit.GetComponent<Unit>().myTile = currentTile;
                        tileBaseUnit.GetComponent<Unit>().targetTile = targetTiles[targetID]; // set tile for unit to move to
                        tileBaseUnit.GetComponent<Unit>().unitManager = this; // pass manager ref

                        units[currentID].GetComponent<Unit>().numInInventory--;
                        shopManager.UpdateAmount(currentID);
                    }
                }
            }
        }
        else if(waveStarted)
        {
            // if there are still units or enemies remaining..
            if(spawnedUnits.Count > 0 && enemies.Count > 0)
            {
                // check if all units have made it to the end
                if(!spawnedUnits.Any(a => a.transform.position != a.GetComponent<Unit>().targetTile.transform.position))
                {
                    ResetWave();
                    waveStarted = false;
                }
            }
            else
            {
                //if this wave either eliminated all units or enemies, it will exit the above loop
                if(spawnedUnits.Count == 0)
                    StartCoroutine(WinOrLose(false));
                else if(enemies.Count == 0)
                    StartCoroutine(WinOrLose(true));
            }
        }
    }
    
    // changes current unit to place
    public void selectUnit(int newUnit)
    {
        currentUnit = units[newUnit];
        currentID = newUnit;
    }

    // buys unit if money is available and adds to inventory
    public void BuyUnit(int newUnit)
    {
        if(PlayerPrefs.GetInt("Money", 0) >= units[newUnit].GetComponent<Unit>().price)
        {
            units[newUnit].GetComponent<Unit>().numInInventory++;
            units[newUnit].GetComponent<Unit>().numBought++;
            shopManager.UpdatePrice(newUnit);
            shopManager.UpdateAmount(newUnit);
        }
    }

    // creates preivew of unit when player holds down
    public void CreatePreview(int tileID)
    {
        currentTile = spawnTiles[tileID];

        // randomize appearance of unit
        if(previewUnit == null)
        {
            switch(currentID)
            {
                // vampire
                case 1:
                    int vampType = Random.Range(0, vampireTypes.Count - 1);
                    currentUnit = vampireTypes[vampType];
                    break;
                // werewolf
                case 2:
                    int wereType = Random.Range(0, werewolfTypes.Count - 1);
                    currentUnit = werewolfTypes[wereType];
                    break;
                // ghost (no variants)
                default:
                    break;
            }
        }

        else
            Destroy(previewUnit);

        previewUnit = Instantiate(currentUnit, currentTile.transform.position, currentUnit.transform.rotation);
        previewUnit.GetComponent<Unit>().isPreview = true;

        SpriteRenderer parentSprite = previewUnit.GetComponent<SpriteRenderer>();
        SpriteRenderer[] sprites = previewUnit.GetComponentsInChildren<SpriteRenderer>();

        if (parentSprite != null)
        {
            Color tmp = parentSprite.color;
            tmp.a = 0.4f;
            parentSprite.color = tmp;
        }
        if (sprites != null)
        {
            foreach (SpriteRenderer sprite in sprites)
            {
                Color tmp = sprite.color;
                tmp.a = 0.4f;
                sprite.color = tmp;
            }
        }
    }

    // called when go button is pressed
    public void StartWave()
    {
        // if player has spawned at least one unit, the wave can start
        if(spawnedUnits.Count > 0)
        {
            waveStarted = true;
            shopManager.ChangeUI(false);
            if(previewUnit != null)
                Destroy(previewUnit);
        }
    }
    
    // called after a wave
    public void ResetWave()
    {
        // reset player units
        for (int i = 0; i < spawnedUnits.Count; i++)
            spawnedUnits[i].transform.position = spawnedUnits[i].GetComponent<Unit>().myTile.transform.position;
        // reset enemy units
        for(int i = 0; i < enemies.Count; i++)
            enemies[i].transform.position = enemies[i].GetComponent<Unit>().myTile.transform.position;
        
        // increase round counter: if all rounds used up, lose
        numRounds++;
        shopManager.UpdateRounds();

        if(numRounds > maxRounds)
            StartCoroutine(WinOrLose(false));
        // reveal UI
        else
            shopManager.ChangeUI(true);
    }

    IEnumerator WinOrLose(bool didIWin)
    {   
        // player won
        if(didIWin)
        {
            winScreen.SetActive(true);
            Unlocks();
        }
        // player lost
        else
            loseScreen.SetActive(true);
        
        yield return new WaitForSeconds(3);
        Flee();
    }

    private void Unlocks()
    {
        switch(levelType)
        {
            // vampire level
            case 0:
            {
                switch(levelNumber)
                {
                    // vamp level 1
                    case 1:
                        if(PlayerPrefs.GetInt("Vampire1", 0) == 0)
                            PlayerPrefs.SetInt("LevelsWon", PlayerPrefs.GetInt("LevelsWon", 0) + 1);
                        PlayerPrefs.SetInt("Vampire1", 1);
                        martini.isUnlocked = true;
                        break;

                    // vamp level 2
                    case 2:
                        if(PlayerPrefs.GetInt("Vampire2", 0) == 0)
                            PlayerPrefs.SetInt("LevelsWon", PlayerPrefs.GetInt("LevelsWon", 0) + 1);
                        PlayerPrefs.SetInt("Vampire2", 1);
                        break;

                    // vamp level 3
                    case 3:
                        if(PlayerPrefs.GetInt("Vampire3", 0) == 0)
                            PlayerPrefs.SetInt("LevelsWon", PlayerPrefs.GetInt("LevelsWon", 0) + 1);
                        PlayerPrefs.SetInt("Vampire3", 1);
                        panini.isUnlocked = true;
                        break;
                }
                break;
            }
            // werewolf level
            case 1:
            {
                switch(levelNumber)
                {
                    // wolf level 1
                    case 1:
                        if(PlayerPrefs.GetInt("Werewolf1", 0) == 0)
                            PlayerPrefs.SetInt("LevelsWon", PlayerPrefs.GetInt("LevelsWon", 0) + 1);
                        PlayerPrefs.SetInt("Werewolf1", 1);
                        soda.isUnlocked = true;
                        break;
                    
                    // wolf level 2
                    case 2:
                        if(PlayerPrefs.GetInt("Werewolf2", 0) == 0)
                            PlayerPrefs.SetInt("LevelsWon", PlayerPrefs.GetInt("LevelsWon", 0) + 1);
                        PlayerPrefs.SetInt("Werewolf2", 1);
                        break;
                    
                    // wolf level 3
                    case 3:
                        if(PlayerPrefs.GetInt("Werewolf3", 0) == 0)
                            PlayerPrefs.SetInt("LevelsWon", PlayerPrefs.GetInt("LevelsWon", 0) + 1);
                        PlayerPrefs.SetInt("Werewolf3", 1);
                        burger.isUnlocked = true;
                        break;
                }
                break;
            }
        }
    }

    public void Flee()
    {
        //NOTE: put different cafes here
        //int numLevels = PlayerPrefs.GetInt("LevelsWon", 0);

        //if(numLevels < 3)
            SceneManager.LoadScene("Test");
        //else if(numLevels < 6)
            //load second cafe
        //else
            //load final cafe
    }
}
