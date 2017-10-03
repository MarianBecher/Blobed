using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserTool : Tool {

    public EraserTool(MapCreator creator) : base(creator)   {   }

    public override void perform(Vector2 startPos, Vector2 endPos)
    {
        eraseTile(startPos, endPos);
    }

    private bool hasTilesToDelete(MapLayer layer, Vector2 startPos, Vector2 endPos)
    {
        bool foundTile = false;
        int xStart = (int)Mathf.Min(startPos.x, endPos.x);
        int yStart = (int)Mathf.Min(startPos.y, endPos.y);
        int xEnd = (int)Mathf.Max(startPos.x, endPos.x);
        int yEnd = (int)Mathf.Max(startPos.y, endPos.y);

        for (int x = xStart; x <= xEnd; x++)
        {
            for (int y = yStart; y <= yEnd; y++)
            {
                Tile oldTile = map.getTile(layer, x, y);
                if (oldTile != null)
                {
                    foundTile = true;
                    break;
                }
            }
        }

        return foundTile;
    }

    private void eraseTile(Vector2 startPos, Vector2 endPos)
    {
        if (!isValidPos(startPos)|| !isValidPos(endPos))
            return; //Invalid Coordinate

        foreach(MapLayer layer in Enum.GetValues(typeof(MapLayer)))
        {

            if (!hasTilesToDelete(layer, startPos, endPos))
                return; //Nothing to delete

            Command c = new PlaceTileCommand("", map, layer, startPos, endPos);
            commandStack.perform(c);
        }


    }
}
