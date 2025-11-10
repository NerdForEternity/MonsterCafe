using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
public class UnitManager : MonoBehaviour
{
    // objects
    public List<GameObject> units;
    public GameObject[] spawnTiles;
    public GameObject currentUnit;
    public GameObject previewUnit;
    private GameObject currentTile;

    // input
    public InputActionAsset InputActions;
    private InputAction m_hitScreen;
    private Vector3 clickPos;

    // flags
    private bool clickOnTile;

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
                //show preview of placement
                if (previewUnit == null)
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
    }

    void Update()
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

                // mouse/finger is over a new tile
                if (newTile != currentTile)
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
                Instantiate(currentUnit, currentTile.transform.position, currentUnit.transform.rotation);
        }
    }
    
    public void selectUnit(int newUnit)
    {
        currentUnit = units[newUnit];
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
}
