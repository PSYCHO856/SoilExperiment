using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
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
    public GameObject expFinishObj;
    
    private void Awake()
    {
        Messenger.AddListener(GameEvent.ON_STEPS_UPDATE, UpdateSteps);
        Init();
        expFinishObj.SetActive(false);
    }

    void Init()
    {
        structureSteps = new List<string>();
        foreach (var experimentInstructionConfigInfoData in ExperimentInstructionConfigInfo.Datas)
        {
            structureSteps.Add(experimentInstructionConfigInfoData.Value.Instruction);
        }
    }

    public RectTransform contentRect;
    public Scrollbar scrollbar;
    void UpdateSteps()
    {
        if (ControllerExperiment.Instance.stepsIndex < structureSteps.Count)
        {
            currentStepsIndex = ControllerExperiment.Instance.stepsIndex;
            // structureTextString.Append(structureSteps[currentStepsIndex] + "\n");
            // structureText.text = structureTextString.ToString();
            structureText.text = structureSteps[currentStepsIndex];

        }
        else if (ControllerExperiment.Instance.stepsIndex == structureSteps.Count)
        {
            expFinishObj.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ON_STEPS_UPDATE, UpdateSteps);
    }
}
