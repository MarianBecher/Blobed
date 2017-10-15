using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBrush : Tool
{
    Vector3 tileScale;
    String prefabPath;
    MapLayer layer;

    public TileBrush(String prefabPath, Vector3 tileScale, MapLayer layer, MapCreator creator) : base(creator)
    {
        this.tileScale = tileScale;
        this.prefabPath = prefabPath;
        this.layer = layer;
    }

    public override void perform(Vector2 startPos, Vector2 endPos)
    {
        placeTile(startPos, endPos);
    }
    
    private bool isAllSameTile(Vector2 startPos, Vector2 endPos)
    {
        bool allSame = true;
        int xStart = (int) Mathf.Min(startPos.x, endPos.x);
        int yStart = (int) Mathf.Min(startPos.y, endPos.y);
        int xEnd = (int)Mathf.Max(startPos.x, endPos.x);
        int yEnd = (int)Mathf.Max(startPos.y, endPos.y);

        for (int x = xStart; x<= xEnd; x++)
        {
            for(int y = yStart; y <= yEnd; y++)
            {

                Tile oldTile = map.getTile(layer, x, y);
                if (oldTile == null || oldTile.PrefabPath != prefabPath)
                {
                    allSame = false;
                    break;
                }
            }
        }

        return allSame;
    }

    private void placeTile(Vector2 startPos, Vector2 endPos)
    {
        if (!this.isValidPos(startPos) || !isValidPos(endPos))
            return; //Invalid input

        if (this.isAllSameTile(startPos, endPos))
            return; //Noting to place here

        Command c = new PlaceTileCommand(prefabPath, tileScale, map, layer, startPos, endPos);
        commandStack.perform(c);
    }
}
