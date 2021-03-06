﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTileCommand : Command
{
    private Map map;
    private MapLayer layer;
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector3 tileScale;
    private string prefabPath;
    private List<OldTileInfo> oldTiles;

    public PlaceTileCommand(String prefabPath, Vector3 tileScale, Map map, MapLayer layer, Vector2 endPos, Vector2 startPos)
    {
        this.map = map;
        this.prefabPath = prefabPath;
        this.startPos = startPos;
        this.endPos = endPos;
        this.layer = layer;
        this.tileScale = tileScale;
        this.oldTiles = new List<OldTileInfo>();
    }

    public override void perform()
    {
        int xStart = (int)Mathf.Min(startPos.x, endPos.x);
        int yStart = (int)Mathf.Min(startPos.y, endPos.y);
        int xEnd = (int)Mathf.Max(startPos.x, endPos.x);
        int yEnd = (int)Mathf.Max(startPos.y, endPos.y);

        for (int x = xStart; x <= xEnd; x++)
        {
            for (int y = yStart; y <= yEnd; y++)
            {
                Tile oldTile = map.getTile(layer, x, y);
                this.oldTiles.Add(new OldTileInfo(oldTile, x, y));
                
                if(this.prefabPath != "")
                    map.setTile(new Tile(prefabPath, tileScale), layer, x, y);
                else
                    map.setTile(null, layer, x, y);
            }
        }
    }

    public override void undo()
    {
        foreach(OldTileInfo oti in oldTiles)
        {
            map.setTile(oti.tile, layer, oti.x, oti.y);
        }
    }

    private struct OldTileInfo
    {
        public int x;
        public int y;
        public Tile tile;
        
        public OldTileInfo(Tile t, int x, int y)
        {
            this.x = x;
            this.y = y;
            this.tile = t;
        }
    }
}
