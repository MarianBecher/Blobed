
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public delegate void TilesSelected(List<Tile> tiles);

public class ToolManager : MonoBehaviour {

    [Header("References")]
    [SerializeField]
    MapCreator mapCreator;
    [SerializeField]
    TilePrefabSelector tileSelector;
    [SerializeField]
    TileConfigurator tileConfigurator;
    [Header("UI-Components")]
    [SerializeField]
    Button eraseToolBtn;
    [SerializeField]
    Button configToolBtn;
    [SerializeField]
    Button buildToolBtn;
    [SerializeField]
    Image selectedTileSprite;
    [SerializeField]
    SpriteRenderer selectionCursor;

    [Header("Cameras")]
    [SerializeField]
    TextureAlphaRenderer backgroundCam;
    [SerializeField]
    TextureAlphaRenderer groundCam;

    private Tool selecedTool;
    private bool isMouseDown;
    private Vector2 startPos;

    void Awake()
    {
        tileSelector.onTileSelected += selectTileBrush;
        buildToolBtn.onClick.AddListener(openTileBar);
        eraseToolBtn.onClick.AddListener(selectEraserTool);
        configToolBtn.onClick.AddListener(selectConfigSelectionTool);

        groundCam.TargetAlpha = 1;
        backgroundCam.TargetAlpha = 1;
    }

    void Update()
    {
        if (selecedTool == null || tileSelector.gameObject.activeInHierarchy)
            return;

        Vector2 relativePosition = Input.mousePosition;
        relativePosition.Scale(new Vector2(1.0f / Screen.width, 1.0f / Screen.height));
        Vector3 camPos = Camera.main.transform.position;
        float viewport_half_height = Camera.main.orthographicSize;
        float viewport_half_width = viewport_half_height * Camera.main.aspect;
        float x = camPos.x + Mathf.Lerp(-viewport_half_width, viewport_half_width, relativePosition.x) + 0.5f;
        float y = camPos.y + Mathf.Lerp(-viewport_half_height, viewport_half_height, relativePosition.y) + 0.5f;
        Vector2 mousePosition = new Vector2(x, y);

        if (!isMouseDown && Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            startPos = mousePosition;
            selectionCursor.gameObject.SetActive(true);
        }
        else if(isMouseDown && Input.GetMouseButton(0))
        {
            float minX = (int) Mathf.Min(startPos.x, mousePosition.x) - 0.5f;
            float maxX = (int)Mathf.Max(startPos.x, mousePosition.x) + 0.5f;
            float minY = (int)Mathf.Min(startPos.y, mousePosition.y) - 0.5f;
            float maxY = (int)Mathf.Max(startPos.y, mousePosition.y) + 0.5f;
            float offsetX = (maxX - minX) / 2;
            float offsetY = (maxY - minY) / 2;
            selectionCursor.transform.position = new Vector3(minX+offsetX, minY+offsetY);
            selectionCursor.size = new Vector2(maxX - minX, maxY - minY);
        }
        else if (isMouseDown && !Input.GetMouseButton(0))
        {
            isMouseDown = false;
            Vector2 endPos = mousePosition;
            selecedTool.perform(startPos, endPos);
            selectionCursor.gameObject.SetActive(false);
        }
    }
    

    private void openTileBar()
    {
        tileSelector.gameObject.SetActive(true);
    }

    private void hideTileBar()
    {
        tileSelector.gameObject.SetActive(false);
    }
    
    private void selectEraserTool()
    {
        hideTileBar();
        resetCamAlpha();
        EraserTool eraser = new EraserTool(mapCreator);
        this.selecedTool = eraser;
    }
    
    private void selectTileBrush()
    {
        hideTileBar();
        resetCamAlpha();
        switch (tileSelector.SelectedTileLayer)
        {
            case MapLayer.BACKGROUND:
                groundCam.TargetAlpha = 0.5f;
                break;
            case MapLayer.GROUND:
                backgroundCam.TargetAlpha = 0.5f;
                break;
        }
        selectedTileSprite.sprite = tileSelector.SelectedTileSprite;
        TileBrush brush = new TileBrush(tileSelector.SelectedTilePrefabPath, tileSelector.SelectedTileLayer, mapCreator);
        this.selecedTool = brush;
    }
    
    private void selectConfigSelectionTool()
    {
        SelectionTool selector = new SelectionTool(mapCreator);
        selector.onSelected = delegate
        {
            tileConfigurator.tile = selector.slectedTiles[0]; //TODO
            tileConfigurator.gameObject.SetActive(true);
        };
        this.selecedTool = selector;
    }

    public void selectTiles(TilesSelected callback)
    {
        SelectionTool selector = new SelectionTool(mapCreator);
        selector.onSelected = delegate
        {
            if(callback != null)
                callback(selector.slectedTiles);
        };
        this.selecedTool = selector;
    }

    private void resetCamAlpha()
    {
        backgroundCam.TargetAlpha = 1;
        groundCam.TargetAlpha = 1;
    }

}
