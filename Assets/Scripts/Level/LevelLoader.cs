using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    private Map map;
    private Dictionary<int, TilePrefab> tileInstances;

    public Text debug;

    public void loadMap(Map m)
    {
        debug.text += "\n" + m.Width;
        debug.text += "\n"+m.Height;

        this.tileInstances = new Dictionary<int, TilePrefab>();
        this.map = m;
        m.tileChangedListeners += instantiateTile;

        try {
            for (int x = 0; x < m.Width; x++)
            {
                for (int y = 0; y < m.Height; y++)
                {
                    instantiateTile(x, y);
                }
            }

            for (int x = 0; x < m.Width; x++)
            {
                for (int y = 0; y < m.Height; y++)
                {
                    configureTile(x, y);
                }
            }
        }
        catch(Exception e)
        {
            debug.text += "\n" + e.Message;
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
                newTile.name = String.Format("({0}|{1}) {2}", x, y, newTile.name);
                newTile.transform.position = new Vector3(x,y,newTile.transform.position.z);
                newTile.transform.localScale = t.Scale;
                TilePrefab tileComp = newTile.GetComponent<TilePrefab>();
                tileInstances.Add(t.ID, tileComp);
            }
        }
    }

    public TilePrefab getTileInstance(int ID)
    {
        TilePrefab result;
        tileInstances.TryGetValue(ID, out result);
        return result;
    }

    private void configureTile(int x, int y)
    {
        Tile t = this.map.getTile(MapLayer.GROUND, x, y);
        if (t != null)
        {
            TilePrefab tileComp = getTileInstance(t.ID);
            tileComp.configure(t.Settings, this);
        }
    }
}
