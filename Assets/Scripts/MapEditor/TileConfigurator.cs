using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileConfigurator : MonoBehaviour {

    [Header("Components")]
    [SerializeField]
    private Button addTargetBtn;
    [SerializeField]
    private Button removeTargetsBtn;
    [SerializeField]
    private SpriteRenderer selectionIndicator;
    [Header("References")]
    [SerializeField]
    private ToolManager toolManager;
    [SerializeField]
    private MapCreator creator;

    private Tile tile;

    void Awake () {
        addTargetBtn.onClick.AddListener(selectNewTarget);
        removeTargetsBtn.onClick.AddListener(removeTargets);
    }

    public void show(Tile t)
    {
        this.tile = t;
        Vector2 tilePos = creator.Level.getTilePos(t.ID);
        selectionIndicator.transform.position = new Vector3(tilePos.x, tilePos.y, -1);
        selectionIndicator.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
    }

    public void hide()
    {
        this.gameObject.SetActive(false);
        selectionIndicator.gameObject.SetActive(false);
    }

    void selectNewTarget()
    {
        toolManager.selectTiles<Triggerable>(addTargets);
    }

    void removeTargets()
    {
        TileSettings newSettings = new TileSettings(tile.Settings);
        newSettings.triggerTargets = new int[0];

        ConfigureTileCommand command = new ConfigureTileCommand(creator.Level, tile, newSettings);
        creator.Commands.perform(command);
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
    }
}
