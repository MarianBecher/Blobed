using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public delegate void EndOfAnimationCalback();

public enum CameraEffects { NONE, VHS};

public class CameraController : MonoBehaviour {

    public bool focusAnimationOnNewTarget = false;
    private EndOfAnimationCalback endOfAnimationCallback;
    public Transform target;
    private const float maxDistance = 5f;

	// Use this for initialization
	void Start () {
	    
	}

    public void newTarget(Transform newTarget, EndOfAnimationCalback callback)
    {
        focusAnimationOnNewTarget = true;
        target = newTarget;
        endOfAnimationCallback = callback;
    }

	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
            return;

        float smothness = 2.0f;
        if (!focusAnimationOnNewTarget)
            smothness = 1.0f;

        //Get the direction but only from the x,y position
        Vector3 direction =  (target.position - transform.position);
        direction.z = 0;
        direction = direction.normalized;

        float distance = ((Vector2)(target.position - transform.position)).magnitude;
        Vector3 newPos = Vector3.Lerp(transform.position, target.position, smothness * Time.deltaTime);
        if (distance >= maxDistance && !focusAnimationOnNewTarget)
        {
            //max distance
            newPos = target.position - (direction * maxDistance);
        }
        else if (distance <= maxDistance && focusAnimationOnNewTarget)
        {
            focusAnimationOnNewTarget = false;
            endOfAnimationCallback();
            endOfAnimationCallback = null;
        }
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
	}


    public void setCameraEffect(CameraEffects effect)
    {
        //Disable all
        VignetteAndChromaticAberration vaca = this.GetComponent<VignetteAndChromaticAberration>();
        NoiseAndScratches nas = this.GetComponent<NoiseAndScratches>();

        nas.enabled = false;
        vaca.chromaticAberration = 0;

        switch(effect)
        {
            case CameraEffects.VHS:
                vaca.chromaticAberration = 50;
                nas.enabled = true;
                break;
            case CameraEffects.NONE:
            default:
                break;
        }
    }

}
