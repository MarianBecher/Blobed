using UnityEngine;
using System;
using System.Xml.Serialization;

[Serializable]
public class Tile
{
    private static int lastID = 0;
    private int id;

    public int ID
    {
        get { return id; }
        private set
        {
            this.id = value;
            if (this.id > lastID)
                lastID = this.id;
        }
    }

    public string PrefabPath { get; private set; }
    public Vector3 Scale { get; private set; }
    public TileSettings Settings { get; private set; }

    public Tile() {}
    public Tile(string prefabPath, Vector3 scale)
    {
        this.ID = lastID + 1;
        this.Scale = scale;
        lastID = this.ID;

        this.PrefabPath = prefabPath;
        this.Settings = new TileSettings();
    }

    public Tile(string prefabPath, Vector3 scale, TileSettings settings) : this (prefabPath, scale)
    {
        updateSettings(settings);
    }

    public void updateSettings(TileSettings settings)
    {
        this.Settings = settings;
    }
}
