using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
public class UnitManager : MonoBehaviour
{
    // objects
    public List<GameObject> units; // references to unit types
    public GameObject[] spawnTiles; // references to spawn tiles in scene
    private GameObject currentUnit; // current unit reference being used
    private GameObject previewUnit; // transparent preview of current unit 
    private GameObject currentTile; // tile to place current unit on

    // input
    public InputActionAsset InputActions;
    private InputAction m_hitScreen;
    private Vector3 clickPos;

    // managers
    public ShopManager shopManager;

    // flags
    private bool clickOnTile;
    private int currentID;

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
                if (previewUnit == null && units[currentID].GetComponent<Unit>().numInInventory > 0 && 
                (newTile.GetComponent<SpawnTile>().unitType == null || newTile.GetComponent<SpawnTile>().unitType.myType == units[currentID].GetComponent<Unit>().myType))
                {
                    clickOnTile = true;
                    CreatePreview(newTile.GetComponent<SpawnTile>().tileID);
                }
                else
Debug.Log("Not enough units in inventory to place unit.");
            }
            else
                clickOnTile = false;
        };
    }
    
    void Start()
    {
        // sets the ghost as default unit to spawn
        currentUnit = units[0];
        //for debugging
PlayerPrefs.SetInt("Money", 999);

        // resets all unit counts and updates shop display
        for (int i = 0; i < units.Count; i++)
        {
Debug.Log("Unit " + i + " reset");
            units[i].GetComponent<Unit>().numInInventory = 0;
            units[i].GetComponent<Unit>().numBought = 0;
            shopManager.UpdatePrice(i);
            shopManager.UpdateAmount(i);
        }
    }

    void Update()
    {
        if(units[currentID].GetComponent<Unit>().numInInventory > 0)
        {
            // mouse/finger is currently held down and initial click was on a tile, track position
            if ((int)m_hitScreen.phase == 2 && clickOnTile)
            {
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
Debug.Log("Added health to unit: " + currentID + ", In Inventory: " + units[currentID].GetComponent<Unit>().numInInventory);
                            shopManager.UpdateAmount(currentID);
                        }
                    }
                    //if there isn't a unit here...
                    else
                    {
                        //set the unit type of the tile
                        GameObject tileBaseUnit = Instantiate(currentUnit, currentTile.transform.position, currentUnit.transform.rotation);
                        spawnTile.unitType = tileBaseUnit.GetComponent<Unit>();

                        units[currentID].GetComponent<Unit>().numInInventory--;
Debug.Log("Placed unit: " + currentID + ", In Inventory: " + units[currentID].GetComponent<Unit>().numInInventory);
                        shopManager.UpdateAmount(currentID);
                    }
                }
            }
        }
    }
    
    public void selectUnit(int newUnit)
    {
        currentUnit = units[newUnit];
        currentID = newUnit;
    }

    public void CreatePreview(int tileID)
    {
        if (previewUnit != null)
            Destroy(previewUnit);

        currentTile = spawnTiles[tileID];
        previewUnit = Instantiate(currentUnit, currentTile.transform.position, currentUnit.transform.rotation);

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

    public void BuyUnit(int newUnit)
    {
        if(PlayerPrefs.GetInt("Money", 0) >= units[newUnit].GetComponent<Unit>().price)
        {
            units[newUnit].GetComponent<Unit>().numInInventory++;
            units[newUnit].GetComponent<Unit>().numBought++;
            shopManager.UpdatePrice(newUnit);
            shopManager.UpdateAmount(newUnit);
Debug.Log("Bought unit: " + newUnit + ", Number Bought: " + units[newUnit].GetComponent<Unit>().numBought + ", In Inventory: " + units[newUnit].GetComponent<Unit>().numInInventory);
        }
        else
Debug.Log("Not enough money to buy unit!");
    }
}
