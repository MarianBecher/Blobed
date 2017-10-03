using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public delegate void MapTileChanged(int x, int y);

[Serializable]
public class Map : IXmlSerializable
{
    public String name;
    public int Width { get; private set; }
    public int Height { get; private set; }

    private Dictionary<MapLayer, Tile[,]> tiles;

    [XmlIgnoreAttribute]
    public MapTileChanged tileChangedListeners;

    public Map(){}

    public Map(int width, int height)
    {
        initMap(width, height);
    }

    private void initMap(int width, int height)
    {
        this.Width = width;
        this.Height = height;

        tiles = new SerializableDictionary<MapLayer, Tile[,]>();
        MapLayer[] layers = (MapLayer[])Enum.GetValues(typeof(MapLayer));
        foreach (MapLayer layer in layers)
        {
            Tile[,] tileArray = new Tile[width, height];
            tiles.Add(layer, tileArray);
        }
    }
    
    public Tile getTile(MapLayer l, int x, int y)
    {
        checkCoordinate(x, y);
        Tile[,] layer = getLayer(l);
        return layer[x, y];
    }

    public Tile getTile(int id)
    {
        Tile result = null;
        MapLayer[] layers = (MapLayer[])Enum.GetValues(typeof(MapLayer));
        foreach (MapLayer layer in layers)
        {
            Tile[,] layerTiles;
            tiles.TryGetValue(layer, out layerTiles);
            for(int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (layerTiles[x,y].ID == id)
                        result = layerTiles[x, y];
                }
            }
        }
        return result;
    }
    public Vector2 getTilePos(int id)
    {
        MapLayer[] layers = (MapLayer[])Enum.GetValues(typeof(MapLayer));
        foreach (MapLayer layer in layers)
        {
            Tile[,] layerTiles;
            tiles.TryGetValue(layer, out layerTiles);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (layerTiles[x, y] != null && layerTiles[x, y].ID == id)
                        return new Vector2(x, y);
                }
            }
        }
        return new Vector2(-1,-1);
    }


    public void setTile(Tile t, MapLayer l, int x, int y)
    {
        checkCoordinate(x, y);
        Tile[,] layer = getLayer(l);
        layer[x, y] = t;

        if (tileChangedListeners != null)
            tileChangedListeners(x, y);
    }

    public void refreshTile(int x, int y)
    {
        Debug.Log("refresh " + x + " " + y);
        if (tileChangedListeners != null)
            tileChangedListeners(x, y);
    }

    private Tile[,] getLayer(MapLayer l)
    {
        Tile[,] t;
        tiles.TryGetValue(l, out t);
        return t;
    }

    private void checkCoordinate(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            throw new Exception("Invalid Tile Coordinate");
    }

    #region IXmlSerializable Members
    public System.Xml.Schema.XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(System.Xml.XmlReader reader)
    {
        this.name = reader["Name"];
        this.Width = Int32.Parse(reader["Width"]);
        this.Height = Int32.Parse(reader["Height"]);
        initMap(Width, Height);
        XmlSerializer tileSerializer = new XmlSerializer(typeof(SerializedTile[]));
        reader.ReadToDescendant("ArrayOfSerializedTile");
        SerializedTile[] tiles = (SerializedTile[]) tileSerializer.Deserialize(reader);
        foreach(SerializedTile tileInfo in tiles)
        {
            Tile[,] layer;
            this.tiles.TryGetValue(tileInfo.l, out layer);
            layer[tileInfo.x, tileInfo.y] = tileInfo.t;
        }

    }

    public void WriteXml(System.Xml.XmlWriter writer)
    {
        XmlSerializer tileSerializer = new XmlSerializer(typeof(SerializedTile[]));

        writer.WriteAttributeString("Name", name);
        writer.WriteAttributeString("Width", Width.ToString());
        writer.WriteAttributeString("Height", Height.ToString());
        tileSerializer.Serialize(writer, convertMapToSerialize(tiles));
    }

    private SerializedTile[] convertMapToSerialize(Dictionary<MapLayer, Tile[,]> input)
    {
        List<SerializedTile> res = new List<SerializedTile>();
        foreach(MapLayer l in input.Keys)
        {
            Tile[,] tiles;
            input.TryGetValue(l, out tiles);
            for(int x = 0; x < Width;  x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    Tile t = this.getTile(l, x, y);
                    if(t != null)
                    {
                        SerializedTile st = new SerializedTile(t, l, x, y);
                        res.Add(st);
                    }
                }
            }
        }
        return res.ToArray();
    }

    [Serializable]
    public struct SerializedTile
    {
        public int x;
        public int y;
        public Tile t;
        public MapLayer l;

        public SerializedTile(Tile t, MapLayer l, int x, int y)
        {
            this.x = x;
            this.y = y;
            this.t = t;
            this.l = l;
        }
    }
    #endregion
}
