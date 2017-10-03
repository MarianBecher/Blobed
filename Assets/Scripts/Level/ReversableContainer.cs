using UnityEngine;
using System.Collections.Generic;

public class ReversableContainer : MonoBehaviour {


    public const float MAX_REWIND_TIME = 5;
    public const int REVERSE_SPEED = 2;

    public float endOfReverseTs;
    public float currentReverseTs;
    private bool isReversing = false;
    public bool IsReversing { get { return isReversing; } private set { isReversing = value; } }

    private List<Reversable> reversableObjects;
    private List<float> safeReversableTimeStamps;

    private CharacterContainer charContainer;
    private CameraController cameraController;

	// Use this for initialization
	void Start () {
        initialize();
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.IsReversing)
        {
            updateSafeTimeStamps();
            for (int i = reversableObjects.Count - 1; i >= 0; i--)
            {
                Reversable r = this.reversableObjects[i];
                if (r.getDestroyedTs() != -1 && Time.time - r.getDestroyedTs() > 1.5 * MAX_REWIND_TIME)
                {
                    //Will newver be rewinded => realy destroy object
                    this.removeObject(r);
                }
            }
        }
        else
            reverse();
	}

    private void updateSafeTimeStamps()
    {
        //Add possilbe new Ts
        bool isReversable = true;
        bool minOneActive = false;
        foreach (Reversable rv in this.reversableObjects)
        {
            if (!rv.isSaveNow())
            {
                isReversable = false;
                break;
            }
            if (rv.isActiveCharacter())
                minOneActive = true;
        }
        if (isReversable && minOneActive)
            safeReversableTimeStamps.Add(Time.time);

        //Clear old TS
        float currentTime = Time.time;
        for (int i = safeReversableTimeStamps.Count - 1; i >= 0; i--)
        {
            if (currentTime - safeReversableTimeStamps[i] > ReversableContainer.MAX_REWIND_TIME)
            {
                safeReversableTimeStamps.RemoveAt(i);
            }
        }
    }

    private void reverse()
    {
        bool endReverse = false;
        float nextTs = currentReverseTs - (Time.deltaTime * REVERSE_SPEED);

        if (nextTs <= endOfReverseTs)
        {
            nextTs = endOfReverseTs;
            endReverse = true;
        }

        for (int i = reversableObjects.Count - 1; i >= 0; i--)
        {
            reversableObjects[i].rewindTo(nextTs);
        }
        currentReverseTs = nextTs;

        if (endReverse)
        {
            this.IsReversing = false;
            safeReversableTimeStamps.Clear();
            safeReversableTimeStamps.Add(Time.time);
            foreach (Reversable rv in this.reversableObjects)
            {
                rv.clearHistory();
            }
            cameraController.setCameraEffect(CameraEffects.NONE);
            charContainer.GetComponent<CharacterContainer>().fixActivePlayer();
        }

    }

    public void startReversing()
    {

        if (isReversing)
            return; //ignore new input

        this.IsReversing = true;
        currentReverseTs = Time.time;
        endOfReverseTs = safeReversableTimeStamps[0]; //oldest safe spot;
        Debug.Log((Time.time - endOfReverseTs) + ": " + endOfReverseTs + "      " + Time.time);
        cameraController.setCameraEffect(CameraEffects.VHS);
    }

    public void addObject(Reversable obj)
    {
        initialize();
        this.reversableObjects.Add(obj);
    }

    public void removeObject(Reversable obj)
    {
        initialize();
        this.reversableObjects.Remove(obj);
    }

    public void initialize()
    {
        if (this.reversableObjects == null)
        {
            reversableObjects = new List<Reversable>();
            safeReversableTimeStamps = new List<float>();

            this.charContainer = GameObject.Find("LevelManager").GetComponent<CharacterContainer>();
            this.cameraController = GameObject.Find("Camera").GetComponent<CameraController>();
        }
    }
}
