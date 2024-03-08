
using System;
using System.IO;

[Serializable]
public class TaskStatistics
{
    public float totalTaskTime = 0;
    public float timeHoldingCorrectObject = 0;
    public float timeHoldingIncorrectObject = 0;

    public int totalSelections = 0;
    public int incorrectSelections = 0;

    public string taskName;

    public void Write(StreamWriter sw)
    {
        // task name
        sw.Write(taskName + ", ");

        // task stats
        sw.Write(totalTaskTime + ", ");
        sw.Write(timeHoldingCorrectObject + ", ");
        sw.Write(timeHoldingIncorrectObject + ", ");

        // Total, correct, incorrect
        sw.Write(totalSelections + ", ");
        sw.Write(totalSelections - incorrectSelections + ", ");
        sw.Write(incorrectSelections + ", ");

    }
}
