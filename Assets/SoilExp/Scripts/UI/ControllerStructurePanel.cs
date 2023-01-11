using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 控制左侧实验步骤的更新
/// </summary>
public class ControllerStructurePanel : MonoBehaviour
{
    List<string> structureSteps;
    
    public Text structureText;
    private StringBuilder structureTextString = new StringBuilder();
    private int currentStepsIndex = -1;
    
    private void Awake()
    {
        Messenger.AddListener(GameEvent.ON_STEPS_UPDATE, UpdateSteps);
        Init();
    }

    void Init()
    {
        structureSteps = new List<string>();
        foreach (var experimentInstructionConfigInfoData in ExperimentInstructionConfigInfo.Datas)
        {
            structureSteps.Add(experimentInstructionConfigInfoData.Value.Instruction);
        }
    }

    void UpdateSteps()
    {
        if (currentStepsIndex < structureSteps.Count)
        {
            if (currentStepsIndex == ControllerExperiment.Instance.stepsIndex) return;
            currentStepsIndex = ControllerExperiment.Instance.stepsIndex;
            structureTextString.Append(structureSteps[currentStepsIndex] + "\n");
            structureText.text = structureTextString.ToString();
        }
    }
}
