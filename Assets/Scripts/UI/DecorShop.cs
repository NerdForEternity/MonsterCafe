using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;

public class DecorShop : MonoBehaviour
{
    public GameObject decorMenu;
    public InputActionAsset InputActions;

    public TileBase[] floors;
    public TileBase[] walls;
    public TileBase[] walls2;
    public TileBase[] windows;
    public TileBase[] pillars;
    public TileBase[] tables;
    public TileBase[] chairs;
    public TileBase[] chairs2;
    public TileBase[] counters;
    public TileBase[] corners;

    public Tilemap floorsMap;
    public Tilemap wallsMap;
    public Tilemap decorMap;
    public Tilemap furnitureMap;

    public void GoBack()
    {
        Time.timeScale = PlayerPrefs.GetFloat("GameSpeed");
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
            furnitureMap.SwapTile(tables[oldTile], tables[newTile]);
            //note: chairs are gameobjects, need to change code here
            furnitureMap.SwapTile(chairs[oldTile], chairs[newTile]);
            furnitureMap.SwapTile(chairs2[oldTile], chairs2[newTile]);
            //////
            furnitureMap.SwapTile(counters[oldTile], counters[newTile]);
            furnitureMap.SwapTile(corners[oldTile], corners[newTile]);

            PlayerPrefs.SetInt("Decor", newTile);
        }
    }
}


