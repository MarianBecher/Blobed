using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePrefab : MonoBehaviour {
    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } private set { icon = value; } }
    public virtual void configure(TileSettings settings) { }
}
