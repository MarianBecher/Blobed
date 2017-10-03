using UnityEngine;
using UnityEditor;

public class LayoutShortCuts : EditorWindow
{

    [MenuItem("Shortcuts/Layout-Animate %#i", false, 999)]
    static void Layout1()
    {
        EditorApplication.ExecuteMenuItem("Window/Layouts/_ANIMATE");
    }
    [MenuItem("Shortcuts/Layout-Default %#o", false, 999)]
    static void Layout2()
    {
        EditorApplication.ExecuteMenuItem("Window/Layouts/_DEFAULT");
    }

}