using UnityEngine;
using System;
using System.Xml.Serialization;

[Serializable]
public class Tile
{
    private static int lastID = 0;
    public int ID { get; private set; }
    public string PrefabPath { get; private set; }
    public TileSettings Settings { get; private set; }

    public Tile() {}
    public Tile(string prefabPath)
    {
        this.ID = lastID + 1;
        lastID = this.ID;

        this.PrefabPath = prefabPath;
        this.Settings = new TileSettings();
    }

    public Tile(string prefabPath, TileSettings settings) : this (prefabPath)
    {
        updateSettings(settings);
    }

    public void updateSettings(TileSettings settings)
    {
        this.Settings = settings;
    }
}
