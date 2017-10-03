using System.Collections.Generic;
using UnityEngine;

public class PostLineRenderer : MonoBehaviour
{
    private Material lineMaterial;
    [SerializeField]
    private Color lineColor;

    private List<Line> lines;

    void Awake()
    {
        this.lines = new List<Line>();
        createLineMaterial();
    }

    public void addLine(Line l)
    {
        this.lines.Add(l);
    }

    public void removeLine(Line l)
    {
        this.lines.Remove(l);
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
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(lineColor);

        foreach(Line l in lines)
        {
            GL.Vertex3(l.start.x, l.start.y, 0);
            GL.Vertex3(l.end.x, l.end.y ,0);
        }
        GL.End();
    }

    public struct Line
    {
        public Vector3 start;
        public Vector3 end;

        public Line(Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
        }
    }
}


