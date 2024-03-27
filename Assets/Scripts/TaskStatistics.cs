
using System;
using System.IO;

[Serializable]
public class TaskStatistics
{
    static char delimiter = ';';

    public StreamWriter writer;

    public int subjectID;
    public DateTime dateTime;

    // ---
    public int trialNum;
    public string taskName;
    public EInteractionType interactionType;
    public int distanceMeters;
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

    public void SetNewSubject(int id)
    {
        subjectID = id;
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
