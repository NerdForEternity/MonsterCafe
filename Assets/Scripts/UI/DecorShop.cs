using Unity.AppUI.UI;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class DecorShop : MonoBehaviour
{
    public GameObject decorMenu;
    public InputActionAsset InputActions;
    public Sprite[] chairSprites;
    public Sprite[] tableSprites;
    public GameObject tableParent;
    public Sprite[] counterSprites;
    public GameObject counterParent;
    public Sprite[] cornerSprites;
    public GameObject cornerParent;

    public TileBase[] floors;
    public TileBase[] walls;
    public TileBase[] walls2;
    public TileBase[] windows;
    public TileBase[] pillars;

    public Tilemap floorsMap;
    public Tilemap wallsMap;
    public Tilemap decorMap;

    public UnityEngine.UI.Button vampButton;
    public UnityEngine.UI.Button werewolfButton;
    public void Start()
    {
        if(PlayerPrefs.GetInt("Vampire3", 0) == 0)
            vampButton.interactable = false;
        else
            vampButton.interactable = true;
        
        if(PlayerPrefs.GetInt("Werewolf3", 0) == 0)
            werewolfButton.interactable = false;
        else
            werewolfButton.interactable = true;
    }
    public void GoBack()
    {
        Time.timeScale = PlayerPrefs.GetFloat("GameSpeed", 1f);
        InputActions.FindActionMap("Player").Enable();
        decorMenu.SetActive(false);
    }
    public void GhostScene()
    {
        int oldTile = PlayerPrefs.GetInt("Decor", 0);
        SwapTiles(oldTile, 0);
    }

    public void VampireScene()
    {
        int oldTile = PlayerPrefs.GetInt("Decor", 0);
        SwapTiles(oldTile, 1);
    }

    public void WerewolfScene()
    {
        int oldTile = PlayerPrefs.GetInt("Decor", 0);
        SwapTiles(oldTile, 2);
    }

    public void SwapTiles(int oldTile, int newTile)
    {
        if (oldTile != newTile)
        {
            floorsMap.SwapTile(floors[oldTile], floors[newTile]);
            wallsMap.SwapTile(walls[oldTile], walls[newTile]);
            wallsMap.SwapTile(walls2[oldTile], walls2[newTile]);
            decorMap.SwapTile(windows[oldTile], windows[newTile]);
            decorMap.SwapTile(pillars[oldTile], pillars[newTile]);
            
            foreach(SpriteRenderer n in tableParent.GetComponentsInChildren<SpriteRenderer>())
                n.sprite = tableSprites[newTile];
            
            foreach(SpriteRenderer n in counterParent.GetComponentsInChildren<SpriteRenderer>())
                n.sprite = counterSprites[newTile];
            
            foreach(SpriteRenderer n in cornerParent.GetComponentsInChildren<SpriteRenderer>())
                n.sprite = cornerSprites[newTile];

            foreach (Chair n in FindObjectsByType<Chair>(FindObjectsSortMode.None))
                n.gameObject.GetComponent<SpriteRenderer>().sprite = chairSprites[newTile];

            PlayerPrefs.SetInt("Decor", newTile);
        }
    }
}


