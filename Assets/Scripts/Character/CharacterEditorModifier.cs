using UnityEngine;
using System.Collections;

//Total dummy script so that the loadStats function is also called in edit mode

[ExecuteInEditMode]
public class CharacterEditorModifier : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Application.isEditor && !Application.isPlaying)
        {
            CharacterController cc = this.GetComponent<CharacterController>();
            cc.loadStats();
        }
	}
}
