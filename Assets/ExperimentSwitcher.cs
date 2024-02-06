using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentSwitcher : MonoBehaviour
{
    [SerializeField] GameObject[] experiments;
    GameObject currentExperiment;

    private void Start()
    {
        foreach (var experiment in experiments)
        {
            experiment.SetActive(false);
        }

        StartCoroutine(AutoCycler(120));
    }

    public void ChangeToExperiment(int n)
    {
        int idx = n - 1;

        if (idx >= 0 && idx < experiments.Length)
        {
            Debug.Log("Deactivating " + currentExperiment);
            currentExperiment?.SetActive(false);

            currentExperiment = experiments[idx];

            Debug.Log("Activating " + currentExperiment);
            currentExperiment.SetActive(true);
        }
    }

    IEnumerator AutoCycler(int durEach)
    {
        int next = 2;
        float elapsed = 0;

        ChangeToExperiment(1);

        while (true)
        {
            elapsed += Time.deltaTime;
            if (elapsed > durEach)
            {
                elapsed = 0;
                ChangeToExperiment(next);

                next = (next + 1) % experiments.Length;
            }

            yield return null;
        }
    }
}
