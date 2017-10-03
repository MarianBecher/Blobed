using UnityEngine;
using System.Collections.Generic;




public class Reversable : MonoBehaviour {


    private struct Snapshot
    {
        public bool safe;
        public Vector3 position;
        public bool characterActive;
        public float timestamp;
    };

    private List<Snapshot> saves;
    private float destroyedTS = -1;
    private float createdTS = 0;
    private bool isCharacter = false;
    private ReversableContainer container;

	void Start () {
        isCharacter = this.GetComponent<CharacterController>() != null;
        createdTS = Time.time;
        saves = new List<Snapshot>();
        container = GameObject.Find("LevelManager").GetComponent<ReversableContainer>();
        container.addObject(this);
	}
	
	// Update is called once per frame
	void Update () {
        deleteOldSaves();
        saveSnapshot();
	}

    private void deleteOldSaves()
    {
        float currentTime = Time.time;
        for(int i = saves.Count -1;  i >= 0; i--)
        {
            if(currentTime - saves[i].timestamp > ReversableContainer.MAX_REWIND_TIME * 1.5)
            {
                saves.RemoveAt(i);
            }
        }
    }

    public void clearHistory()
    {
        this.saves.Clear();
        this.saveSnapshot();
    }

    public void rewindTo(float timestamp)
    {
        int targetSnapshotIndex = saves.Count - 1;
        for (int i = saves.Count - 1; i >= 0; i--)
        {
            if (saves[i].timestamp > timestamp)
            {
                targetSnapshotIndex = i;
            }
        }

        if (timestamp < createdTS)
        {
            if(isCharacter)
            {
                GameObject.Find("LevelManager").GetComponent<CharacterContainer>().removeCharacter(this.GetComponent<CharacterController>());
            }
            container.removeObject(this);
            Destroy(this.gameObject);
            return;
        }

        if (targetSnapshotIndex < 0)
            targetSnapshotIndex = 0;

        if (saves.Count == 0)
            return; //TODO was dann machne?

        Snapshot targetSnapshot = saves[targetSnapshotIndex];
        this.gameObject.transform.position = targetSnapshot.position;

        bool revived = false;
        if (!gameObject.activeSelf && timestamp <= destroyedTS)
        {
            gameObject.SetActive(true);
            revived = true;
        }

        if (isCharacter)
        {
            this.GetComponent<CharacterController>().active = targetSnapshot.characterActive;

            //Fix outstanding information
            if(targetSnapshot.characterActive)
            {
                GameObject.Find("Camera").GetComponent<CameraController>().target = this.transform;
            }     
            if(revived)
            {
                GameObject.Find("LevelManager").GetComponent<CharacterContainer>().addCharacter(this.GetComponent<CharacterController>());
            }
        }
    }

    public void destroyObject()
    {
        this.destroyedTS = Time.time;
        this.gameObject.SetActive(false);
    }

    public float getDestroyedTs()
    {
        return this.destroyedTS;
    }

    public bool isSaveNow()
    {
        if (this.GetComponent<CharacterController>() != null)
            return this.GetComponent<CharacterController>().isGrounded();

        return true;
    }

    public bool isActiveCharacter()
    {
        if (this.GetComponent<CharacterController>() != null)
            return this.GetComponent<CharacterController>().active;

        return false;
    }

    private void saveSnapshot()
    {
        Snapshot savePoint = new Snapshot();
        savePoint.timestamp = Time.time;
        savePoint.position = this.transform.position;
        savePoint.safe = this.isSaveNow();

        savePoint.characterActive = this.isActiveCharacter();
        saves.Add(savePoint);
    }
}
