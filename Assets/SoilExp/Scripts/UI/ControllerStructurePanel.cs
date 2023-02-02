using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    private CanvasGroup expFinishObjCG;
    
    [SerializeField] private TypeWriter typeWriter;
    
    private void Awake()
    {
        Messenger.AddListener(GameEvent.ON_STEPS_UPDATE, UpdateSteps);
        Init();
        expFinishObjCG = expFinishObj.GetComponent<CanvasGroup>();
        expFinishObjCG.alpha = 0;
    }

    void Init()
    {
        structureSteps = new List<string>();

        
        foreach (var experimentInstructionConfigInfoData 
            in ControllerExperiment.Instance.GetCurrentExpInstructionConfig())
        {
            structureSteps.Add(experimentInstructionConfigInfoData.Value.Instruction);
        }
        // foreach (var experimentInstructionConfigInfoData in ExperimentInstructionConfigInfo.Datas)
        // {
        //     structureSteps.Add(experimentInstructionConfigInfoData.Value.Instruction);
        // }
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
            // structureText.text = structureSteps[currentStepsIndex];
            if (ControllerExperiment.Instance.stepsIndex < 11)
            {
                typeWriter.Run(structureSteps[currentStepsIndex], typeWriter.text);
            }
            
        }
        else if (ControllerExperiment.Instance.stepsIndex == structureSteps.Count)
        {
            expFinishObjCG.DOFade(1, 0.5f);
            UserHelper.SetHighlightOff();
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ON_STEPS_UPDATE, UpdateSteps);
        typeWriter.RecycleOnClose();
    }
}
