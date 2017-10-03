using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    private Map map;
    private GameObject[,] tileContainers;

    public void loadMap(Map m, bool isEditor=false)
    {
        this.map = m;
        m.tileChangedListeners += instantiateTile;
        tileContainers = new GameObject[m.Width, m.Height];
        for (int x = 0; x < m.Width; x++)
        {
            for (int y = 0; y < m.Height; y++)
            {
                createTileContainer(x, y);
                instantiateTile(x, y, isEditor);
            }
        }
    }

    private void createTileContainer(int x, int y)
    {
        GameObject go = new GameObject("Tile (" + x + "|" + y + ")");
        go.transform.SetParent(this.transform);
        go.transform.position = new Vector3(x, y, 0);

        MapLayer[] layers = (MapLayer[])Enum.GetValues(typeof(MapLayer));
        foreach (MapLayer layer in layers)
        {
            GameObject instancedTile = new GameObject(layer.ToString());
            instancedTile.transform.SetParent(go.transform, false);
        }
        tileContainers[x, y] = go;
    }
    private void instantiateTile(int x, int y)
    {
        instantiateTile(x, y, false);
    }
    
    private void instantiateTile(int x, int y, bool isEditor)
    {
        GameObject container = tileContainers[x, y];
        MapLayer[] layers = (MapLayer[])Enum.GetValues(typeof(MapLayer));
        foreach (MapLayer layer in layers)
        {
            GameObject layerContainer = container.transform.Find(layer.ToString()).gameObject;

            foreach (Transform child in layerContainer.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            Tile t = this.map.getTile(layer, x, y);
            if (t != null)
            {
                GameObject newTile = Instantiate<Transform>(Resources.Load<Transform>(t.PrefabPath)).gameObject;
                newTile.transform.SetParent(layerContainer.transform, false);
                TilePrefab tileComp = newTile.GetComponent<TilePrefab>();
                tileComp.configure(t.Settings);

                if (isEditor)
                {
                    if (typeof(Trigger).IsAssignableFrom(tileComp.GetType()))
                    {
                        newTile.AddComponent<TileTargetVisualizer>();
                    }
                }
            }
        }
    }
}
