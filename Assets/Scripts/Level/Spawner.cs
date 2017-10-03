using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : TilePrefab {
    [SerializeField]
    Transform prefab;

    void Start()
    {
        if(GameObject.Find("LevelManager") != null)
        {
            //In Game
            Transform instance = Instantiate<Transform>(prefab);
            instance.transform.SetParent(this.transform.parent);
            Debug.Log(this.transform.parent);
            instance.position = new Vector3(this.transform.position.x, this.transform.position.y, instance.transform.position.z);
            Destroy(this.gameObject);
        }
        else
        {
            //In Editor
            SpriteRenderer sr = this.gameObject.AddComponent<SpriteRenderer>();
            sr.sprite = this.Icon;
        }
    }
}
