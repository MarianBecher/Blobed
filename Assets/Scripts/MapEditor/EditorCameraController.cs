using UnityEngine;

public class EditorCameraController : MonoBehaviour {
    private Camera cam;
    private float screenHeight;
    private float screenWidth;
    private Vector2 minPosition;
    private Vector2 maxPosition;
    [SerializeField]
    private RectOffset padding;

    void Awake()
    {
        cam = Camera.main;
        screenHeight = cam.orthographicSize * 2;
        screenWidth = screenHeight * cam.aspect;
        Vector2 offset = new Vector2(0.5f, 0.5f);
        minPosition = new Vector2(screenWidth/2.0f, screenHeight / 2.0f) - offset - new Vector2(padding.left, padding.bottom);
        maxPosition = new Vector2(MapCreator.MAP_WIDTH - screenWidth / 2.0f, MapCreator.MAP_HEIGHT - screenHeight / 2.0f) - offset + new Vector2(padding.right, padding.top);
    }

    void Update()
    {
        restrictMovement();
    }

    void restrictMovement()
    {
        Vector3 newPos = this.transform.position;

        if (this.transform.position.x < minPosition.x)
            newPos.x = minPosition.x;
        if (this.transform.position.x > maxPosition.x)
            newPos.x = maxPosition.x;
        if (this.transform.position.y < minPosition.y)
            newPos.y = minPosition.y;
        if (this.transform.position.y > maxPosition.y)
            newPos.y = maxPosition.y;

        this.transform.position = newPos;
    }
}
