using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class NPCPathVisualizer : Node
{

    [Export] public Node3D pointsParent;

    [Export] public bool recalculateLines;

	J_Debug debug;

	List<MeshInstance3D> meshCache = new List<MeshInstance3D>();

	public override void _Ready()
	{
		foreach(Node child in this.GetChildren())
			if (child is J_Debug)
			{
				debug = child as J_Debug;
			}

		if(debug == null)
		GD.PrintErr("Path must inheret J_Debug class.");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (recalculateLines)
		{
			//Clear Mesh Cache
			foreach(MeshInstance3D mesh in meshCache)
			{
				mesh.QueueFree();
			}
			meshCache.Clear();
			//Draw Points
            foreach (Node3D node in pointsParent.GetChildren())
            {
                meshCache.Add(debug.PointRenderer(node.Position, .5f, Color.Color8(255, 255, 255, 255)));
            }
			//Draw Lines
			for(int i = 0; i < pointsParent.GetChildren().Count; i++)
			{
				if(i != 0)
				{
					Node3D p1 = pointsParent.GetChildren()[i - 1] as Node3D;
                    Node3D p2 = pointsParent.GetChildren()[i] as Node3D;
                    meshCache.Add(debug.LineRender(p1.Position, p2.Position, Color.Color8(255, 255, 255, 255)));
                }
					
			}
        }
	}
}
