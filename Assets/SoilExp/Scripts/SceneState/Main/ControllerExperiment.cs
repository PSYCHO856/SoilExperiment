using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Linq;
using Cinemachine;
using HighlightPlus;
using UnityEngine.EventSystems;

public partial class ControllerExperiment : preProject.Singleton<ControllerExperiment>
{
    public string QId1= "42101";
    public string QId2= "44201";
    
    private RaycastHit hit1;
    private Ray ray1;

    public Camera mainCam;
    public Camera uiCam;

    public int stepsIndex;

    public List<List<GameObject>> objectsInSteps;

    public CinemachineVirtualCamera _virtualCamera;

    public GameObject playerObj;
    /// <summary>
    /// 步骤中要操作的设备存储在config里
    /// 判定操作完成后stepsIndex++
    /// 
    /// </summary>
    private void Awake()
    {
        // mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        gotEquipments.Clear();
        InitPos();
    }

    void Start()
    {
        // MUIMgr.Instance.OpenUI(EMUI.MUI_Question, null, QId1);
        // MUIMgr.Instance.OpenUI(EMUI.MUI_QuestionSingle, null, QId);
        GetCurrentStepEquipment();
    }

    private string instructionString;
    private List<string> gotEquipments = new List<string>();

    private bool isSelect;
    private bool canOperate;
    private Transform selectedTrans;
    
    //移动类型
    //step里物体个数
    //特殊移动
    //移动分解
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CheckStep(testStepIndex);
        }
        
        if(Input.GetMouseButtonDown(0) /*&& !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()*/)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            ray1 = GetMyRay();
            if (Physics.Raycast(ray1, out hit1))
            {
                
                //点击高亮实现 实验器具的说明 操作ui
                if (hit1.collider.gameObject.CompareTag("ExpObject"))
                {
                    _virtualCamera.LookAt = hit1.collider.gameObject.transform;
                    // Debug.Log("设置高亮"+ hit1.collider.gameObject.name);
                    ToolManager.Instance.SetHighlightOn(hit1.collider.gameObject.transform, 20f);
                }                
                
                
                //限定当前步骤的可点设备 限定先后点击顺序
                if (!isSceneEquipmentObjects(hit1.collider.gameObject)) return;

                if (ToolManager.Instance.sceneNumber == 0) DensityStepsJudge();
                if (ToolManager.Instance.sceneNumber == 1) MoistureStepsJudge();
                if (ToolManager.Instance.sceneNumber == 2) BorderMoistureStepsJudge();
                

                if (hit1.collider.gameObject.CompareTag("ExpObject"))
                {
                    //获取实验器具的说明
                    if (!CheckEquipment(hit1.collider.gameObject.name))
                    {
                        if (hit1.collider.gameObject.name.Equals("电子秤"))
                        {
                            OpenTIANPING(hit1.collider.transform.GetChild(0));
                        }
                        
                        instructionString = GetEquipmentInstruction(hit1.collider.gameObject.name);
                        if (instructionString == "") return;
                        Messenger<string>.Broadcast(GameEvent.ON_INSTRUCTION_UPDATE,instructionString);
                        
                    }
                }    


            }
        }
        

        
        
    }


    
    public Ray GetMyRay()
    {
        return mainCam.ScreenPointToRay(Input.mousePosition);
    }

    public List<GameObject> currentStepEquipment;

    private string nowBtnText;


    public Dictionary<string,ExperimentInstructionConfig> GetCurrentExpInstructionConfig()
    {
        string _searchID = "";
        if (ToolManager.Instance.sceneNumber == 0)
        {
            _searchID="10";
        }
        else if (ToolManager.Instance.sceneNumber == 1)
        {
            _searchID="20";
        }
        else if (ToolManager.Instance.sceneNumber == 2)
        {
            _searchID="30";
        }
        
        var ansList =
            ExperimentInstructionConfigInfo.Datas.Where(c =>
                c.Value.Id.Substring(0, _searchID.Length) == _searchID);

        return ansList.ToDictionary(k => k.Key, 
            v => v.Value);

    }
    
    //获取与当前操作有关的物体
    void GetCurrentStepEquipment()
    {
        
        int objectInStepId = -1;
        foreach (var experimentInstructionConfigInfoData in GetCurrentExpInstructionConfig())
        {
            if (experimentInstructionConfigInfoData.Value.StepIndex.Equals(stepsIndex.ToString()))
            {
                objectInStepId = Int32.Parse(experimentInstructionConfigInfoData.Value.ObjectInStepId);
                nowBtnText = experimentInstructionConfigInfoData.Value.TextOnButton;
            }
        }

        if (objectInStepId == -1) return;
        currentStepEquipment = new List<GameObject>();
        foreach (var objectInStepConfigInfoData in ObjectInStepConfigInfo.Datas)
        {  
            if (objectInStepConfigInfoData.Value.Id.Equals(objectInStepId.ToString()))
            {
                currentStepEquipment.Clear();
                if(objectInStepConfigInfoData.Value.Name1!="")currentStepEquipment.Add(FindObjectInScene(objectInStepConfigInfoData.Value.Name1));
                if(objectInStepConfigInfoData.Value.Name2!="")currentStepEquipment.Add(FindObjectInScene(objectInStepConfigInfoData.Value.Name2));
                RefreshSuggestGobj(currentStepEquipment[0]);
            }
        }
    }

    void RefreshSuggestGobj(GameObject suggestGobj)
    {
        if (ControllerExperiment.Instance.stepsIndex >= 11 && ToolManager.Instance.sceneNumber==0) return;
        // ToolManager.Instance.SetHighlightOnSuggest(suggestGobj.transform);
    }

    //编辑器内赋值 当前场景与实验操作有关的物体
    public List<GameObject> sceneEquipmentObjects;
    GameObject FindObjectInScene(string gameobjectName)
    {
        foreach (var sceneEquipmentObject in sceneEquipmentObjects)
        {
            if (sceneEquipmentObject.name.Equals(gameobjectName))
                return sceneEquipmentObject;
        }

        Debug.LogWarning("GOBJ NOT FOUND");
        return null;
    }

    bool isSceneEquipmentObjects(GameObject gObj)
    {
        foreach (var sceneEquipmentObject in sceneEquipmentObjects)
        {
            // Debug.Log(sceneEquipmentObject.name);
            if (gObj.name == sceneEquipmentObject.name) return true;
        }
        return false;
    }

    bool CheckEquipment(string equipmentName)
    {
        if (gotEquipments.Contains(equipmentName))
        {
            return true;
        }
        gotEquipments.Add(equipmentName);
        return false;
    }
    
    string GetEquipmentInstruction(string equipmentName)
    {
        foreach (var experimentEquipmentConfigInfoData in ExperimentEquipmentConfigInfo.Datas)
        {
            if (experimentEquipmentConfigInfoData.Value.Name.Equals(equipmentName))
            {
                return experimentEquipmentConfigInfoData.Value.Instruction;
            }
        }

        return "";
    }

    void MoveEquipment(Transform selecteTrans, Transform targetTrans, TweenCallback callback = null)
    {
        if (!selecteTrans) return;

        var position = selecteTrans.position;
        var position1 = targetTrans.position;
        float selectedTransHeight = selecteTrans.GetComponent<BoxCollider>().size.y * selecteTrans.localScale.y;
        float targetTransHeight = targetTrans.GetComponent<BoxCollider>().size.y * targetTrans.localScale.y;
        float moveDuration = 0.4f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(selecteTrans.DOMove(new Vector3(position.x, position.y + 0.2f, position.z), moveDuration)) // 添加动画到队列中
            .AppendInterval(0.2f)
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position.y + 0.2f, position1.z), moveDuration))
            
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + (selectedTransHeight + targetTransHeight)/2, position1.z), moveDuration))
            .AppendInterval(0.1f)
            .AppendCallback(callback);
        
        //(selectedTransHeight + targetTransHeight)/2
    }
    

    


    public float xiaoKnifeMoveOffsetX = -0.1f;
    public float xiaoKnifeMoveDistanceZ = 0.2f;
    public float xiaoKnifeMoveOffsetZ = -0.1f;
    public Transform circleKnifeTrans;
    public GameObject soilObj;
    public GameObject soilObj2;
    
    
    public float guaKnifeMoveDistanceZ = 0.1f;
    

    void MoveEquipmentCallbackWithUIOption()
    {
        Messenger<Vector3,string,Action>.Broadcast(GameEvent.ON_OPERATE_UPDATE, hit1.collider.transform.position,nowBtnText,MoveEquipmentCallback);
    }
    //若可操作 显示ui

    void MoveEquipmentCallback()
    {
        RefreshSteps();
        GetCurrentStepEquipment();
    }

    public int testStepIndex = 6;
    void CheckStep(int testStepIndex)
    {
        Instance.stepsIndex= testStepIndex;
        //左侧实验步骤更新
        Messenger.Broadcast(GameEvent.ON_STEPS_UPDATE);
        GetCurrentStepEquipment();
    }

    float brushMoveHeight = 0.55f;
    
    
    void RefreshSteps()
    {
        Instance.stepsIndex++;
        //左侧实验步骤更新
        Messenger.Broadcast(GameEvent.ON_STEPS_UPDATE);
    }

    public List<GameObject> equipments = new List<GameObject>();
    public Dictionary<GameObject, Vector3> equipmentOriginPos = new Dictionary<GameObject, Vector3>();

    void InitPos()
    {
        foreach (var equipment in equipments)
        {
            equipmentOriginPos.Add(equipment,equipment.transform.position);
        }
    }

    void ReturnOriginPos(GameObject gObj, float duration, bool isRotated, bool isCallback = true)
    {
        Vector3 oriPos = Vector3.zero;
        foreach (var varable in equipmentOriginPos)
        {
            if (varable.Key.name.Equals(gObj.name))
            {
                oriPos = varable.Value;
            }
        }

        if (oriPos.Equals(Vector3.zero)) return;

        Transform gTrans = gObj.transform;
        float moveDuration = 0.5f;

        if (isCallback)
        {
            if (isRotated)
            {
                DOTween.Sequence() 
                    .AppendInterval(duration)
                    .Append(
                        gTrans.DOMove(new Vector3(oriPos.x, oriPos.y + 0.2f, oriPos.z),
                            moveDuration)) // 添加动画到队列中
                    .Append(gTrans.DORotate(Vector3.zero,moveDuration))
                    .Append(gTrans.DOMove(new Vector3(oriPos.x, oriPos.y, oriPos.z), moveDuration))
                    .AppendCallback(MoveEquipmentCallback);
            }
            else
            {
                DOTween.Sequence() 
                    .AppendInterval(duration)
                    .Append(
                        gTrans.DOMove(new Vector3(oriPos.x, oriPos.y + 0.2f, oriPos.z),
                            moveDuration)) // 添加动画到队列中
                    .Append(gTrans.DOMove(new Vector3(oriPos.x, oriPos.y, oriPos.z), moveDuration))
                    .AppendCallback(MoveEquipmentCallback);
            }
        }
        else
        {
            if (isRotated)
            {
                DOTween.Sequence()
                    .AppendInterval(duration)
                    .Append(
                        gTrans.DOMove(new Vector3(oriPos.x, oriPos.y + 0.2f, oriPos.z),
                            moveDuration)) // 添加动画到队列中
                    .Append(gTrans.DORotate(Vector3.zero, moveDuration))
                    .Append(gTrans.DOMove(new Vector3(oriPos.x, oriPos.y, oriPos.z), moveDuration));
            }
            else
            {
                DOTween.Sequence()
                    .AppendInterval(duration)
                    .Append(
                        gTrans.DOMove(new Vector3(oriPos.x, oriPos.y + 0.2f, oriPos.z),
                            moveDuration)) // 添加动画到队列中
                    .Append(gTrans.DOMove(new Vector3(oriPos.x, oriPos.y, oriPos.z), moveDuration));
            }
        }

    }

    void DeselectAllGobj()
    {
        HighlightManager.instance.internal_DeselectAll();
        HighlightManager.instance.SwitchesCollider (null);
    }
}
