
using System;
using System.IO;

[Serializable]
public class TaskStatistics
{
    static char delimiter = ';';

    public StreamWriter writer;

    // Unique ID per participant
    public int subjectID;

    // What of the six orders was it?
    public int presentationOrderNum;
    public DateTime dateTime;

    // ---
    // What # trial/task are we on
    public int trialNum;

    // What is the name (pulled from prefab) of this trial/task?
    public string taskName;

    // GoGo, Raycast, ATOM
    public EInteractionType interactionType;

    // How far is the object group?
    public int distanceMeters;

    // How occluded is the object group?
    // Full occlusion - 90deg, No occlusion - 0deg
    public int occlutionAngleDegrees;

    // ---
    public float totalTaskTime = 0;
    public float timeHoldingCorrectObject = 0;
    public float timeHoldingIncorrectObject = 0;

    public int totalSelections = 0;
    public int incorrectSelections = 0;
    public bool wasTaskCompletedSuccessfully = false;

    // Movement
    public TaskStatistics(StreamWriter writer)
    {
        this.writer = writer; 
    }

    public void SetNewSubject(int id, int presentationOrderNumIn)
    {
        subjectID = id;
        presentationOrderNum = presentationOrderNumIn;
        dateTime = DateTime.Now.ToLocalTime();
    }

    public void StartNewTask(string taskName, EInteractionType interactionType, int distanceM, int occlusionDeg)
    {
        trialNum++;
        this.taskName = taskName;
        this.interactionType = interactionType;
        distanceMeters = distanceM;
        occlutionAngleDegrees = occlusionDeg;
        ResetInteractionStats();
    }

    void ResetInteractionStats()
    {
        totalTaskTime = 0;
        timeHoldingCorrectObject = 0;
        timeHoldingIncorrectObject = 0;

        totalSelections = 0;
        incorrectSelections = 0;
        wasTaskCompletedSuccessfully = false;
    }


    public void Write()
    {
        // ID;date;time;
        writer.Write(subjectID.ToString() + delimiter);
        writer.Write(dateTime.Date.ToString() + delimiter);
        writer.Write(dateTime.TimeOfDay.ToString() + delimiter);

        // Task settings - trial #;name;mechanism;distance;angle;
        writer.Write(trialNum.ToString() + delimiter);
        writer.Write(taskName.ToString() + delimiter);
        writer.Write(interactionType.ToString() + delimiter);
        writer.Write(distanceMeters.ToString() + delimiter);
        writer.Write(occlutionAngleDegrees.ToString() + delimiter);

        // Time spent - total, correct, incorrect;
        writer.Write(totalTaskTime.ToString() + delimiter);
        writer.Write(timeHoldingCorrectObject.ToString() + delimiter);
        writer.Write(timeHoldingIncorrectObject.ToString() + delimiter);

        // Total;incorrect;completed
        writer.Write(totalSelections.ToString() + delimiter);
        writer.Write(incorrectSelections.ToString() + delimiter);
        writer.Write(wasTaskCompletedSuccessfully.ToString() + Environment.NewLine);

        // movement


        writer.Flush();
    }
}
