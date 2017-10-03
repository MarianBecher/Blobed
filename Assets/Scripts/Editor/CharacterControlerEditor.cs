#if UNITY_EDITOR 
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterController))]
public class CharacterControlerEditor : Editor {

    bool showMovementStats  = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CharacterController myTarget = (CharacterController)target;

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Private Stats", EditorStyles.boldLabel);
        showMovementStats = EditorGUILayout.Foldout(showMovementStats, "Movement", true);
        if (showMovementStats)
        {
            addFieldInfo("moveSpeed");
            addFieldInfo("jumpPower");
            addFieldInfo("lookDirection");
            addFieldInfo("canWallJump");
            addFieldInfo("grounded");
            addFieldInfo("onWall");
            addFieldInfo("wallJumpDirection");
            addFieldInfo("lastFrameJumpedPressed");
            addFieldInfo("lastWallJumpTs");
            addFieldInfo("isCurrentlyWalljumping");
        }
    }

    private void addFieldInfo(string fieldName)
    {
        FieldInfo f = typeof(CharacterController).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        EditorGUILayout.LabelField(f.Name, f.GetValue(target).ToString());
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
#endif