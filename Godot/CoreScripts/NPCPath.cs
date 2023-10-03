using Godot;
using System;
using System.Collections.Generic;

public partial class NPCPath : Node
{
	NPCPathVisualizer vis;
	public Node3D pathParent;
	public List<Node3D> points = new List<Node3D>();
	public float lineLength;
	
	public override void _Ready()
	{
		foreach(Node node in this.GetChildren())
		{
			if (node is NPCPathVisualizer)
				vis = node as NPCPathVisualizer;
		}

		if (vis == null)
			GD.PrintErr("NPCPath is missing a visualizer.");
		else
		{
            pathParent = vis.pointsParent;
			foreach (Node3D node in pathParent.GetChildren())
				points.Add(node);
        }

		lineLength = CalculateLineLength();
	}

	float CalculateLineLength()
	{
		float length = 0;
		for(int i = 0; i < points.Count; i++)
		{
			if (i != 0)
				length += (points[i].Position.Length() - points[i - 1].Position.Length());
		}
		return length;
	}

	public List<Node3D> GetPathPoints()
	{
		return points;
	}
}
