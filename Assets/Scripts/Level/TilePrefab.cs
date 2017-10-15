using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TilePrefab : MonoBehaviour {
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private bool allowflipHorizontal;
    [SerializeField]
    private bool allowflipVertical;

    public Sprite Icon { get { return icon; } private set { icon = value; } }
    public bool AllowFlipHorizontal { get { return allowflipHorizontal; } private set { allowflipHorizontal = value; } }
    public bool AllowFlipVertical { get { return allowflipVertical; } private set { allowflipVertical = value; } }
    public virtual void configure(TileSettings settings, LevelLoader loader) { }
}
