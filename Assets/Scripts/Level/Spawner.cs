using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : TilePrefab {
    [SerializeField]
    Transform prefab;

    void Start()
    {
        Transform instance = Instantiate<Transform>(prefab);
        instance.transform.SetParent(this.transform.parent);
        instance.position = new Vector3(this.transform.position.x, this.transform.position.y, instance.transform.position.z);
        Destroy(this.gameObject);
    }
}
