using UnityEngine;

public abstract class Tool {
    protected Map map;
    protected CommandStack commandStack;

    public Tool(MapCreator creator)
    {
        map = creator.Level;
        commandStack = creator.Commands;
    }

    protected bool isValidPos(Vector2 pos)
    {
        bool invalid =  ((int)pos.x < 0 || (int)pos.x >= map.Width || (int)pos.y < 0 || (int)pos.y >= map.Height);
        return !invalid;
    }

    public abstract void perform(Vector2 startPos, Vector2 endPos);
}
