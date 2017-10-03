using System;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    private Map map;

    public void loadMap(Map m)
    {
        this.map = m;
        m.tileChangedListeners += instantiateTile;

        for (int x = 0; x < m.Width; x++)
        {
            for (int y = 0; y < m.Height; y++)
            {
                instantiateTile(x, y);
            }
        }
    }

    
    private void instantiateTile(int x, int y)
    {
        MapLayer[] layers = (MapLayer[])Enum.GetValues(typeof(MapLayer));
        foreach (MapLayer layer in layers)
        {
            Tile t = this.map.getTile(layer, x, y);
            if (t != null)
            {
                GameObject newTile = Instantiate<Transform>(Resources.Load<Transform>(t.PrefabPath)).gameObject;
                newTile.name = String.Format("({0}|{1})", x, y);
                newTile.transform.position = new Vector3(x,y,newTile.transform.position.z);
                TilePrefab tileComp = newTile.GetComponent<TilePrefab>();
                tileComp.configure(t.Settings);
            }
        }
    }
}
