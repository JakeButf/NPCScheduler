using Godot;
using System;
using System.Collections.Generic;

public partial class ScheduledNPC : Node3D
{
    [Export] Transform3D feet; //Bottom of the NPC
    [Export] GameMaster GM;
    [Export] NPCPath[] paths; //Object containing path points
    [Export] float[] startTimes; //This is a workaround to Godot being unable to export structs. indexes of start and end times must match pathParent index.
    [Export] float[] endTimes;

    float[] currentSchedule = new float[2];
    NPCPath currentPath;
    bool npcShouldSpawn = false;
    int closestNPCPoint = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!CheckIfNPCShouldSpawn())
            return;
        this.Position = CalculateNPCPosition(currentPath, currentSchedule, GM.currentTime);
    }

    bool CheckIfNPCShouldSpawn()
    {
        for (int i = 0; i < startTimes.Length; i++)
        {
            if (startTimes[i] <= GM.currentTime && endTimes[i] >= GM.currentTime)
            {
                npcShouldSpawn = true;
                currentPath = paths[i];
                currentSchedule[0] = startTimes[i];
                currentSchedule[1] = endTimes[i];
                break;
            }
            else
            {
                npcShouldSpawn = false;
            }
        }

        this.Visible = npcShouldSpawn;
        return npcShouldSpawn;
    }

    Vector3 CalculateNPCPosition(NPCPath path, float[] times, float time)
    {
        float normalizedTime = (time - times[0]) / (times[1] - times[0]); // Returns time value between 0-1
        float lineLength = path.lineLength; // Total length of the line
        float dividedLength = lineLength / (path.GetPathPoints().Count - 1); // Line length between points 
        float timeDividedLength = lineLength * normalizedTime; // Player point on line

        // Find the two points that the timeDividedLength lies between
        int closestPoint = 0;
        for (int i = 0; i < path.GetPathPoints().Count - 1; i++)
        {
            if (timeDividedLength >= dividedLength * i && timeDividedLength <= dividedLength * (i + 1))
            {
                closestPoint = i;
                break;
            }
        }
        closestNPCPoint = closestPoint; // Assuming closestNPCPoint is a class field or property

        // Calculate how far along the segment the point is (0 being point A, 1 being point B)
        float pointOnLine = (timeDividedLength - (dividedLength * closestPoint)) / dividedLength;

        // Return Linear Interpolation of Point A and Point B against time t (pointOnLine)
        return path.GetPathPoints()[closestPoint].Position.Lerp(path.GetPathPoints()[closestPoint + 1].GlobalPosition, pointOnLine);
    }
}

