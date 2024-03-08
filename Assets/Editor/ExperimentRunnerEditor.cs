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

        if (GUILayout.Button("BEGIN EXPERIMENTS", GUILayout.Width(300), GUILayout.Height(50)))
        {
            runner.BeginExperiments();
        }
    }
}