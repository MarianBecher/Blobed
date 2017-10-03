using System;
using UnityEngine;

public class EditorLevelLoader : MonoBehaviour {

    private Map map;
    private GameObject[,] tileContainers;

    public void loadMap(Map m)
    {
        this.map = m;
        m.tileChangedListeners += instantiateTile;
        tileContainers = new GameObject[m.Width, m.Height];
        for (int x = 0; x < m.Width; x++)
        {
            for (int y = 0; y < m.Height; y++)
            {
                createTileContainer(x, y);
                instantiateTile(x, y);
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

                TilePrefab tilePrefab = Resources.Load<Transform>(t.PrefabPath).GetComponent<TilePrefab>();

                GameObject newTile = new GameObject();
                newTile.transform.SetParent(layerContainer.transform, false);
                SpriteRenderer spriteComp = newTile.AddComponent<SpriteRenderer>();
                spriteComp.sprite = tilePrefab.Icon;
            }
        }
    }
}
