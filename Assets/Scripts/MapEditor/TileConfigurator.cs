using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileConfigurator : MonoBehaviour {

    [Header("Components")]
    [SerializeField]
    private Button addTargetBtn;
    [Header("References")]
    [SerializeField]
    private ToolManager toolManager;
    [SerializeField]
    private MapCreator creator;

    public Tile tile;

    void Awake () {
        addTargetBtn.onClick.AddListener(selectNewTarget);
    }

    void selectNewTarget()
    {
        toolManager.selectTiles(addTargets);
    }
    
    void addTargets(List<Tile> tiles)
    {
        TileSettings newSettings = new TileSettings(tile.Settings);
        List<int> targets = new List<int>(newSettings.triggerTargets);

        foreach(Tile t in tiles)
        {
            if (!targets.Contains(t.ID))
                targets.Add(t.ID);
        }

        newSettings.triggerTargets = targets.ToArray();
        ConfigureTileCommand command = new ConfigureTileCommand(creator.Level, tile, newSettings);
        creator.Commands.perform(command);
        this.gameObject.SetActive(false);
    }
}
