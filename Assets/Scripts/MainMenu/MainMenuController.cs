using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    int currentlvlIndex = 0;
    Config cfg;
    bool isStartingALevel = false;
    private Animator loadLevelAnimator;

	// Use this for initialization
    void Start()
    {
        loadLevelAnimator = GameObject.Find("overlay").GetComponent<Animator>();

        int cellSize = Screen.width / 10;
        GameObject btnContainer = GameObject.Find("BtnContainer");
        GridLayoutGroup glg = btnContainer.GetComponent<GridLayoutGroup>();
        glg.padding.top = cellSize / 2;
        glg.padding.bottom = cellSize / 2;
        glg.padding.right = cellSize * 2;
        glg.padding.left = cellSize;
        glg.cellSize = new Vector2(cellSize, cellSize);
        glg.spacing = new Vector2(cellSize / 4, cellSize / 4);
        RectTransform rt = btnContainer.GetComponent<RectTransform>();
        this.cfg = Config.getConfig();
        for (var i = 1; i <= cfg.getUnlockedLevelCount(); i++ )
        {
            GameObject.Find("lvl_" + i).GetComponent<LevelSelection>().setUnlocked(true);
        }

	}
	
    public void setSelection(int lvlIndex)
    {
        for (var i = 1; i <= cfg.getUnlockedLevelCount(); i++)
        {
            if(i != lvlIndex)
                GameObject.Find("lvl_" + i).GetComponent<LevelSelection>().setStatus(false);
        }
        currentlvlIndex = lvlIndex;
        GameObject.Find("Play-Button").GetComponent<Button>().interactable = true;

    }

    public void startLvl()
    {
        if (!isStartingALevel)
        {
            isStartingALevel = true;
            cfg.setCurrentLvl(currentlvlIndex);
            loadLevelAnimator.SetTrigger("startAnimation");

            StartCoroutine(startLevel());
        }
    }

    IEnumerator startLevel()
    {
        yield return new WaitForSeconds(0.75f);
        Application.LoadLevel(this.currentlvlIndex);
    }
}
