using System;
using UnityEngine;

class ConfigureTileCommand : Command
{
    Tile targetTile;
    TileSettings oldSettings;
    TileSettings newSettings;
    Vector2 pos;
    Map map;

    public ConfigureTileCommand(Map map, Tile target, TileSettings setting)
    {
        this.newSettings = setting;
        this.targetTile = target;
        this.map = map;
        pos = map.getTilePos(target.ID);
    }

    public override void perform()
    {
        targetTile.updateSettings(newSettings);
        map.refreshTile((int)pos.x, (int)pos.y);
    }

    public override void undo()
    {
        //TODO evtl muss ich des tile neu laden??
        targetTile.updateSettings(oldSettings);
        map.refreshTile((int)pos.x, (int)pos.y);
    }
}