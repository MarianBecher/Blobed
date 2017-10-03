using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TilePrefabSelector : MonoBehaviour
{
    private enum Tab { BLOCK, DECORATION, SPECIAL };
    [Header("Components")]
    [SerializeField]
    private Button decorationTabBtn;
    [SerializeField]
    private Button blockTabBtn;
    [SerializeField]
    private Button specialTabBtn;
    [SerializeField]
    Transform content;
    [Header("Tile Prefabs")]
    public Transform[] groundTiles;
    public Transform[] decorationTiles;
    public Transform[] specialTiles;

    private List<GameObject> availibleBlocks;
    
    public MapLayer SelectedTileLayer {get; private set;}
    public string SelectedTilePrefabPath { get; private set; }
    public Sprite SelectedTileSprite { get; private set; }
    
    public int callbackOrder
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public Action onTileSelected;

    void Awake()
    {

        availibleBlocks = new List<GameObject>();
        /*this.tiles = new SerializableDictionary<Tab, string[]>();
        this.tiles.Add(Tab.DECORATION, getListOfPrefabsInFolder("Background"));
        this.tiles.Add(Tab.BLOCK, getListOfPrefabsInFolder("Map"));
        this.tiles.Add(Tab.SPECIAL, getListOfPrefabsInFolder("Special"));*/

        decorationTabBtn.onClick.AddListener(delegate { switchTab(Tab.DECORATION); });
        blockTabBtn.onClick.AddListener(delegate { switchTab(Tab.BLOCK); });
        specialTabBtn.onClick.AddListener(delegate { switchTab(Tab.SPECIAL); });
        switchTab(Tab.BLOCK);
    }


    private void switchTab(Tab selectedTab)
    {
        MapLayer layer = MapLayer.GROUND;
        decorationTabBtn.interactable = true;
        specialTabBtn.interactable = true;
        blockTabBtn.interactable = true;
        Transform[] prefabs = new Transform[0]; //TODO Bad Code...
        switch (selectedTab)
        {
            case Tab.DECORATION:
                layer = MapLayer.BACKGROUND;
                decorationTabBtn.interactable = false;
                prefabs = decorationTiles;
                break;
            case Tab.BLOCK:
                layer = MapLayer.GROUND;
                blockTabBtn.interactable = false;
                prefabs = groundTiles;
                break;
            case Tab.SPECIAL:
                layer = MapLayer.GROUND;
                specialTabBtn.interactable = false;
                prefabs = specialTiles;
                break;
        }

        //Clear Content
        foreach (GameObject g in availibleBlocks)
        {
            Destroy(g);
        }
        

        //Update content
        foreach(Transform prefab in prefabs)
        {
            Sprite sprite = prefab.GetComponent<TilePrefab>().Icon;

            GameObject newTileBtn = new GameObject();
            newTileBtn.transform.SetParent(content, false);
            Image imageComponent = newTileBtn.AddComponent<Image>();
            imageComponent.sprite = sprite;
            Button btnComponent = newTileBtn.AddComponent<Button>();
            btnComponent.transition = Selectable.Transition.None;
            btnComponent.onClick.AddListener(delegate {
                this.SelectedTilePrefabPath = "Tiles/" + selectedTab.ToString() +"/"+  prefab.gameObject.name; //TODO echt kacke so
                this.SelectedTileSprite = sprite;
                this.SelectedTileLayer = layer;

                if (onTileSelected != null)
                    onTileSelected();
            });

            availibleBlocks.Add(newTileBtn);
        }
    }    
}
