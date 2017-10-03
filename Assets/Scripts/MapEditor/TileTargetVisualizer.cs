using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Trigger))]
public class TileTargetVisualizer : MonoBehaviour {

    private Trigger triggerComp;
    private List<PostLineRenderer.Line> lines;
    private PostLineRenderer lineRenderer;
    

    public void Awake()
    {
        this.lineRenderer = Camera.main.GetComponent<PostLineRenderer>();
        Debug.Log(Camera.main.name);
        this.lines = new List<PostLineRenderer.Line>();
        this.triggerComp = this.GetComponent<Trigger>();
        updateLines();
    }

    void OnDestroy()
    {
        deleteLines();
    }

    private void deleteLines()
    {
        foreach (PostLineRenderer.Line l in this.lines)
            lineRenderer.removeLine(l);
        lines.Clear();
    }

    public void updateLines()
    {
        deleteLines();
        
        foreach(Triggerable target in triggerComp.targets)
        {
            PostLineRenderer.Line line = new PostLineRenderer.Line(this.transform.position, target.transform.position);
            lineRenderer.addLine(line);
        }
    }
}
