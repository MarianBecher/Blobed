using System.Collections.Generic;

public class CommandStack{
    private Stack<Command> commands;
    private Stack<Command> undoneCommands;


    public CommandStack()
    {
        commands = new Stack<Command>();
        undoneCommands = new Stack<Command>();
    }

    public void perform(Command c)
    {
        commands.Push(c);
        c.perform();
        undoneCommands.Clear();
    }

    public void undo()
    {
        if(commands.Count > 0)
        {
            Command c = commands.Pop();
            c.undo();
            undoneCommands.Push(c);
        }
    }

    public void redo()
    {
        if(undoneCommands.Count > 0)
        {
            Command c = undoneCommands.Pop();
            c.perform();
        }
    }
}
