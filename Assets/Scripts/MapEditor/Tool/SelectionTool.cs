using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class SelectionTool<T> : Tool where T : MonoBehaviour
{
    public Action onSelected;
    public List<Tile> slectedTiles;
    private Type filter;

    public SelectionTool(MapCreator creator) : base(creator)
    {
        slectedTiles = new List<Tile>();
    }
    public override void perform(Vector2 startPos, Vector2 endPos)
    {
        slectedTiles.Clear();

        int xStart = (int)Mathf.Min(startPos.x, endPos.x);
        int yStart = (int)Mathf.Min(startPos.y, endPos.y);
        int xEnd = (int)Mathf.Max(startPos.x, endPos.x);
        int yEnd = (int)Mathf.Max(startPos.y, endPos.y);

        for (int x = xStart; x <= xEnd; x++)
        {
            for (int y = yStart; y <= yEnd; y++)
            {

                Tile t = map.getTile(MapLayer.GROUND, x, y);
                if (t != null)
                {
                    TilePrefab prefab = Resources.Load<TilePrefab>(t.PrefabPath);

                    if (prefab.GetComponent<T>() != null)
                        slectedTiles.Add(t);
                }
            }
        }

        if (onSelected != null && slectedTiles.Count > 0)
            onSelected();
    }
}
