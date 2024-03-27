using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(ExperimentRunner))]
public class ExperimentRunnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ExperimentRunner runner = (ExperimentRunner)target;

        // Since behavior of "grabbing an object" depends on the block we have to set it here 
        // Not running a task so the block never starts or ends or anything, we just need the
        // handlers...

        if (GUILayout.Button("Test Raycast", GUILayout.Width(300), GUILayout.Height(50)))
        {
            runner.ChangeInteractionMechanism(EInteractionType.Raycast);
            runner.currentBlock = runner.taskBlocks[0];
        }

        if (GUILayout.Button("Test Go-Go", GUILayout.Width(300), GUILayout.Height(50)))
        {
            runner.ChangeInteractionMechanism(EInteractionType.GoGo);
            runner.currentBlock = runner.taskBlocks[1];

        }

        if (GUILayout.Button("Test ATOM", GUILayout.Width(300), GUILayout.Height(50)))
        {
            runner.ChangeInteractionMechanism(EInteractionType.ATOM);
            runner.currentBlock = runner.taskBlocks[2];

        }

        GUILayout.Space(20);

        if (GUILayout.Button("To Experiment Area", GUILayout.Width(300), GUILayout.Height(50)))
        {
            runner.MoveXROriginTo(Vector3.zero);
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Begin Experiments!", GUILayout.Width(300), GUILayout.Height(80)))
        {
            runner.BeginExperiments();
        }

    }
}