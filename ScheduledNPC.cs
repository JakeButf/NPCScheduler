using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledNPC : MonoBehaviour
{
    [SerializeField] GameObject parent; //The object to activate/deactivate
    [SerializeField] Transform feet;    //Bottom of the NPC
    private GameMaster GM;
    [SerializeField] List<NPCPath> npcPath = new List<NPCPath>();     //Object containing the multiple points for the NPC.
    NPCPath currentPath; //The current path the NPC is on.
    StartEndTimes currentStartEndTime;
    [SerializeField] List<StartEndTimes> startEndTime = new List<StartEndTimes>(); //The time that the npc starts the path and the time the NPC ends the path.
    bool shouldSpawn = false;
    int closestNPCPoint = 0;

    private void Awake()
    {
        GM = MasterCache._GameMaster;
    }
    void Update()
    {
        CheckIfNPCShouldSpawn();
        if (!shouldSpawn) //Remove unnecessary computations by just breaking out of the update loop when player isnt spawned.
            return;
        FloorNPC();
        this.transform.position = CalculateNPCPosition(currentPath, currentStartEndTime, GM.currentTime);
        RotateNPC();
    }

    void FloorNPC()
    {
        Vector3 rayPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        RaycastHit hit;
        if (Physics.Raycast(rayPos, Vector3.down, out hit, Mathf.Infinity, Physics.AllLayers))
        {
            //Place the NPC directly on the ground
            this.transform.position = new Vector3(this.transform.position.x, hit.point.y, this.transform.position.z);
        }
    }
    
    void CheckIfNPCShouldSpawn()
    {
        //Check if NPC should be undergoing a path on scene load
        for (int i = 0; i < startEndTime.Count; i++)
        {
            if (startEndTime[i].startEndTimes[0] <= GM.currentTime && startEndTime[i].startEndTimes[1] >= GM.currentTime)
            {
                shouldSpawn = true;
                currentPath = npcPath[i];
                currentStartEndTime = startEndTime[i];
                break;
            } else
            {
                shouldSpawn = false;
            }
        }
        //Activate this object accordingly
        parent.SetActive(shouldSpawn);
    }

    Vector3 CalculateNPCPosition(NPCPath path, StartEndTimes times, float time)
    {
        float normalizedTime = (time - times.startEndTimes[0]) / (times.startEndTimes[1] - times.startEndTimes[0]); //Returns time value between 0-1
        float lineLength = path.LineLength; //Total length of the line
        float dividedLength = lineLength / path.GetPathPoints().Count; //Line length between points 
        float timeDividedLength = lineLength * normalizedTime; //Player point on line

        //Get Closest Point
        int closestPoint = 0;
        for(int i = 0; i < path.GetPathPoints().Count; i++)
        {
            if(timeDividedLength >= dividedLength * (i + 1))
            {
                closestPoint = i;
            }
        }
        closestNPCPoint = closestPoint;
        //Get value between 0-1 representing how far along the line the player should be (0 being point a, 1 being point b)
        float pointOnLine = (timeDividedLength - (dividedLength * (closestPoint + 1))) / ((dividedLength * (closestPoint + 2)) - (dividedLength * (closestPoint + 1)));
        //Return Linear Interpolation of Point A and Point B against time t (pointOnLine)
        return Vector3.Lerp(path.GetPathPoints()[closestPoint].position, path.GetPathPoints()[closestPoint + 1].position, pointOnLine);
    }

    void RotateNPC()
    {
        float x = this.transform.rotation.x;
        float z = this.transform.rotation.z;
        this.transform.LookAt(currentPath.GetPathPoints()[closestNPCPoint + 1].position);

        this.transform.eulerAngles = new Vector3(x, this.transform.eulerAngles.y, z);
    }
}

[System.Serializable]
public class StartEndTimes
{
    public List<float> startEndTimes = new List<float>();
}
