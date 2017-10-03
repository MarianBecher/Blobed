using UnityEngine;
using System.Collections.Generic;

public enum MoveInstructions { Left, Right, Jump};

public class InputController {
    private Rect leftRegion;
    private Rect rightRegion;
    private Rect jumpRegion;
    private float lastTimeNoTouch = 0;
    private Config config;
    public bool stopInput = false;

    private static InputController instance;
    public static InputController getController()
    {
        if (instance == null)
        {
            instance = new InputController();
        }
        return instance;
    }

    private InputController()
    {
        config = Config.getConfig();
        leftRegion = new Rect(0, 0, config.getInputMoveSize() / 2f * Screen.width, Screen.height);
        rightRegion = new Rect(config.getInputMoveSize() / 2f * Screen.width, 0, config.getInputMoveSize() / 2f * Screen.width, Screen.height);
        jumpRegion = new Rect(config.getInputMoveSize() * Screen.width, 0, (1 - config.getInputMoveSize()) * Screen.width, Screen.height);
    }

    private List<Vector2> getTouchPositions()
    {
        List<Vector2> positions = new List<Vector2>();
        positions.Clear();

        for (int i = 0; i < Input.touchCount; i++ )
        {
            positions.Add(Input.GetTouch(i).position);
        }

        if (positions.Count == 0 && Input.GetMouseButton(0))
        {
            positions.Add(Input.mousePosition);
        }

        if (positions.Count == 0)
        {
            lastTimeNoTouch = Time.time;
        }

        if (stopInput)
            positions.Clear();

        return positions;
    }

    private bool isTouchedInArea(Rect area)
    {        
        foreach(Vector2 position in this.getTouchPositions() )
        {
            if(position.x > area.position.x && position.x < area.position.x + area.width &&
                position.y > area.position.y && position.y < area.position.y + area.height)
            {
                return true;
            }
        
        }
        return false;
    }

    public HashSet<MoveInstructions> getMoveInstructions()
    {
        HashSet<MoveInstructions> instructions = new HashSet<MoveInstructions>();

        if (this.isTouchedInArea(this.leftRegion))
            instructions.Add(MoveInstructions.Left);

        if (this.isTouchedInArea(this.rightRegion))
            instructions.Add(MoveInstructions.Right);

        if (this.isTouchedInArea(this.jumpRegion))
            instructions.Add(MoveInstructions.Jump);

        if (Input.GetAxis("Horizontal") < 0)
            instructions.Add(MoveInstructions.Left);
        
        if (Input.GetAxis("Horizontal") > 0)
            instructions.Add(MoveInstructions.Right);

        if (Input.GetAxis("Jump") != 0)
            instructions.Add(MoveInstructions.Jump);

        if (Time.timeScale == 0 ||  stopInput)
            instructions.Clear();

        return instructions;
    }

    public bool isClickedOnCharacter(CharacterController character)
    {
        Camera camera = GameObject.Find("Camera").GetComponent<Camera>();
        Vector2 screenPosition = camera.WorldToScreenPoint(character.transform.position);
        float pixelSizeOfCharacter = camera.pixelHeight / camera.orthographicSize * character.GetComponent<CircleCollider2D>().radius*2;
        bool isClicked = this.isClickedOnUIElement(screenPosition, pixelSizeOfCharacter, pixelSizeOfCharacter);
        return isClicked;
    }

    public void clickActionPerformed()
    {
        this.lastTimeNoTouch = 0;
    }

    public bool currentlyTouched()
    {
        List<Vector2> positions = this.getTouchPositions();
        return positions.Count > 0;
    }

    public bool isClickedOnUIElement(Vector2 centeredScreenPosition, float pixelWidth, float pixelHeight)
    {

        bool result = isTouchedInArea(new Rect(centeredScreenPosition - 0.5f * new Vector2(pixelWidth, pixelHeight), new Vector2(pixelWidth, pixelHeight)));

        if (Time.time - lastTimeNoTouch > Time.deltaTime * 2)
        {
            return false; //Ist länger schon gedrückt => kein klick
        }

        return result;
    }
}
