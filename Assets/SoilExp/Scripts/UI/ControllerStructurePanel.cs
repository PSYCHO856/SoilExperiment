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
    private Tween finishTween;
    
    [SerializeField] private TypeWriter typeWriter;
    
    private void Awake()
    {
        Messenger.AddListener(GameEvent.ON_STEPS_UPDATE, UpdateSteps);
        Init();
        // expFinishObj.SetActive(false);
        // expFinishObjCG = expFinishObj.GetComponent<CanvasGroup>();
        // expFinishObjCG.alpha = 0;
        // finishTween = expFinishObj.transform.DOLocalMove(new Vector2(
        //     expFinishObj.transform.position.x - expFinishObj.GetComponent<RectTransform>().sizeDelta.x,
        //     expFinishObj.transform.position.y), 2f);

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
            if (structureSteps[currentStepsIndex].Equals(typeWriter.text.text)) return;
            
            switch (ToolManager.Instance.sceneNumber)
            {
                case 0:
                    if (ControllerExperiment.Instance.stepsIndex < 11)
                    {
                        typeWriter.Run(structureSteps[currentStepsIndex], typeWriter.text);
                    }
                    break;
                        
                case 1:
                    typeWriter.Run(structureSteps[currentStepsIndex], typeWriter.text);
                    break;
                
                case 2:
                    typeWriter.Run(structureSteps[currentStepsIndex], typeWriter.text);
                    break;
                    
                default: 
                    typeWriter.Run(structureSteps[currentStepsIndex], typeWriter.text);
                    break;
            }
            
        }
        else if (ControllerExperiment.Instance.stepsIndex == structureSteps.Count)
        {
            // expFinishObj.SetActive(true);
            
            
            RectTransform rt = expFinishObj.GetComponent<RectTransform>();
            finishTween = rt.DOLocalMoveX(
                rt.anchoredPosition.x-rt.sizeDelta.x, 
                2f).Pause().SetAutoKill(false);
            finishTween.Play();
            
            // expFinishObjCG.DOFade(1, 0.5f);
            UserHelper.SetHighlightOff();
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ON_STEPS_UPDATE, UpdateSteps);
        typeWriter.RecycleOnClose();
    }
}
