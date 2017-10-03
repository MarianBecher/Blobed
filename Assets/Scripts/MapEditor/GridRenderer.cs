using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    private Material lineMaterial;
    [SerializeField]
    private Color gridColor;

    void Awake()
    {
        createLineMaterial();
    }

    private void createLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    private void OnPostRender()
    {
        Vector2 offset = new Vector2(0.5f, 0.5f);
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(gridColor);

        for (int x = 0; x <= MapCreator.MAP_WIDTH; x++)
        {
            GL.Vertex3(x - offset.x, 0 - offset.y, 0);
            GL.Vertex3(x - offset.x, MapCreator.MAP_HEIGHT - offset.y, 0);
        }

        for (int y = 0; y <= MapCreator.MAP_HEIGHT; y++)
        {
            GL.Vertex3(0 - offset.x, y - offset.y, 0);
            GL.Vertex3(MapCreator.MAP_WIDTH - offset.x, y - offset.y, 0);
        }

        GL.End();
    }
}
