using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ExperimentRunner : MonoBehaviour
{
    [Tooltip("Make sure to change this!")]
    public int experimentNumber;

    public ExperimentTaskBlock[] taskBlocks;

    [SerializeField] GameObject raycastInterface;
    [SerializeField] GameObject gogoInterface;
    [SerializeField] GameObject ATOMInterface;

    // Task complete audio used:
    // https://pixabay.com/sound-effects/bloop-2-186531/
    public AudioSource audioSource;

    // Raycast = 0,
    // GoGo = 1,
    // ATOM = 2,

    int[][] presentationOrder =
    {
        new int[] { 0, 1, 2 }, // R, G, A
        new int[] { 0, 2, 1 }, // R, A, G
        new int[] { 1, 0, 2 }, // G, R, A
        new int[] { 1, 2, 0 }, // G, A, R
        new int[] { 2, 0, 1 }, // A, R, G
        new int[] { 2, 1, 0 }  // A, G, R
    };

    int nextBlockNum = 0;
    int[] selectedPresentationOrder;
    ExperimentTaskBlock currentBlock;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (taskBlocks.Length != 3)
        {
            Debug.LogError("There needs to be three task blocks in the experiment!");
        }

        // Loop every 6
        selectedPresentationOrder = presentationOrder[experimentNumber % 6];

    }

    public void BeginExperiments()
    {
        Debug.Log("Experiments begin!");
        RunNextExperimentBlock();
    }

    void RunNextExperimentBlock()
    {
        // Select the block
        int taskBlockIdx = selectedPresentationOrder[nextBlockNum];
        currentBlock = taskBlocks[taskBlockIdx];
        Debug.Log($"Running block {currentBlock}");

        // Change inputs
        ChangeInteractionMechanism(currentBlock.interactionType);

        // Start it
        currentBlock.StartTaskBlock(this);
    }

    void ChangeInteractionMechanism(EInteractionType interactionType)
    {
        switch (interactionType)
        {
            case EInteractionType.Raycast:
                {
                    raycastInterface.SetActive(true);
                    gogoInterface.SetActive(false);
                    ATOMInterface.SetActive(false);
                }
                break;
            case EInteractionType.GoGo:
                {
                    raycastInterface.SetActive(false);
                    gogoInterface.SetActive(true);
                    ATOMInterface.SetActive(false);
                }
                break;
            case EInteractionType.ATOM:
                {
                    raycastInterface.SetActive(false);
                    gogoInterface.SetActive(false);
                    ATOMInterface.SetActive(true);
                }
                break;
        }
    }

    public void CompleteBlock(ExperimentTaskBlock block)
    {
        nextBlockNum++;
        // still some to do
        if (nextBlockNum < taskBlocks.Length)
        {
            RunNextExperimentBlock();
        }
        else
        {
            // Done!
            ConcludeExperiment();
        }
    }

    void ConcludeExperiment()
    {
        Debug.Log("Experiment finished!");
        WriteExperimentResults();
    }

    void WriteExperimentResults()
    {
        DateTime localTimeNow = DateTime.Now.ToLocalTime();

        string docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string atomFolder = "ATOMExperimentResults";
        string finalFolder = Path.Combine(docsPath, atomFolder);
        if (!Directory.Exists(finalFolder))
        {
            Directory.CreateDirectory(finalFolder);
        }

        string filename = $"ATOM_exp{experimentNumber}_{localTimeNow:yyyyMMdd_HHmmss}.csv";
        string finalPath = Path.Combine(finalFolder, filename);

        StreamWriter sw = new StreamWriter(finalPath);

        // Experiment #
        sw.Write(experimentNumber + ", ");

        // Date+Time
        sw.Write(localTimeNow.ToString() + ", ");

        // Order that you ran in
        sw.Write(selectedPresentationOrder[0] + ", ");
        sw.Write(selectedPresentationOrder[1] + ", ");
        sw.Write(selectedPresentationOrder[2] + ", ");

        // The block results now
        taskBlocks[selectedPresentationOrder[0]].Write(sw);
        taskBlocks[selectedPresentationOrder[1]].Write(sw);
        taskBlocks[selectedPresentationOrder[2]].Write(sw);

        sw.Flush();
        sw.Close();

        Debug.Log("Finished writing stats!");

    }

    // Run them to here because apparently we can't assign them at runtime... woo
    public void OnSelectXRObject(SelectEnterEventArgs args)
    {
        currentBlock.SelectXRObject_BlockLevel(args);
    }

    public void OnDeselectXRObject(SelectExitEventArgs args)
    {
        currentBlock.DeselectXRObject_BlockLevel(args);
    }

}
