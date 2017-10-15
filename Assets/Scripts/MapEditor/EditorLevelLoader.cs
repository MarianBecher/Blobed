using System;
using UnityEngine;

public class EditorLevelLoader : MonoBehaviour {

    private Map map;
    private GameObject[,] tileContainers;
    [SerializeField]
    private PostLineRenderer lineRenderer;
    [SerializeField]
    private Material groundLayerMat;
    [SerializeField]
    private Material backgroundLayerMat;

    public void loadMap(Map m)
    {
        this.map = m;
        tileContainers = new GameObject[m.Width, m.Height];

        for (int x = 0; x < m.Width; x++)
        {
            for (int y = 0; y < m.Height; y++)
            {
                createTileContainer(x, y);
            }
        }

        for (int x = 0; x < m.Width; x++)
        {
            for (int y = 0; y < m.Height; y++)
            {
                instantiateTile(x, y);
            }
        }

        updateLines();
        m.tileChangedListeners += instantiateTile;
    }

    public void highlightLayer(MapLayer layer)
    {
        Color fadout = new Color(1, 1, 1, 0.5f);
        groundLayerMat.color = fadout;
        backgroundLayerMat.color = fadout;

        Material mat = layer == MapLayer.GROUND ? groundLayerMat : backgroundLayerMat;
        mat.color = Color.white;
    }

    public void removeLayerHighlight()
    {
        groundLayerMat.color = Color.white;
        backgroundLayerMat.color = Color.white;
    }

    private void updateLines()
    {
        lineRenderer.clear();
        for (int map_x = 0; map_x < map.Width; map_x++)
        {
            for(int map_y = 0; map_y < map.Height; map_y++)
            {
                Tile t = map.getTile(MapLayer.GROUND,map_x, map_y);

                if (t == null || t.Settings.triggerTargets.Length == 0)
                    continue;
                
                foreach (int id in t.Settings.triggerTargets)
                {
                    Vector2 targetTileCoordinates = map.getTilePos(id);
                    GameObject container = getContainer(new Vector2(map_x, map_y));
                    GameObject targetContainer = getContainer(targetTileCoordinates);
                    lineRenderer.addLine(new PostLineRenderer.Line(container.transform.position, targetContainer.transform.position));
                }
            }
        }
    }

    private GameObject getContainer(Vector2 pos)
    {
        return tileContainers[(int)pos.x, (int)pos.y];
    }

    private void createTileContainer(int x, int y)
    {
        GameObject go = new GameObject("Tile (" + x + "|" + y + ")");
        go.transform.SetParent(this.transform);
        go.transform.position = new Vector3(x, y, 0);

        MapLayer[] layers = (MapLayer[])Enum.GetValues(typeof(MapLayer));
        foreach (MapLayer layer in layers)
        {
            GameObject layerContainer = new GameObject(layer.ToString());
            layerContainer.transform.SetParent(go.transform, false);
            layerContainer.transform.localPosition = new Vector3(0, 0, (int)layer);
        }
        tileContainers[x, y] = go;
    }
    
    private void instantiateTile(int x, int y)
    {
        GameObject container = getContainer(new Vector2(x, y));
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
                newTile.transform.localScale = t.Scale;
                SpriteRenderer spriteComp = newTile.AddComponent<SpriteRenderer>();
                spriteComp.sprite = tilePrefab.Icon;

                Material mat = layer == MapLayer.GROUND ? groundLayerMat : backgroundLayerMat;
                spriteComp.material = mat;
            }
        }
        updateLines();
    }
}
