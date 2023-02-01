using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


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
        
        if(Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            ray1 = GetMyRay();
            if (Physics.Raycast(ray1, out hit1))
            {
                //点击高亮实现 实验器具的说明 操作ui
                if (hit1.collider.gameObject.CompareTag("ExpObject"))
                {
                    // Debug.Log("设置高亮"+ hit1.collider.gameObject.name);
                    ToolManager.Instance.SetHighlightOn(hit1.collider.gameObject.transform, 20f);
                }                
                
                
                //限定当前步骤的可点设备 限定先后点击顺序
                if (!isSceneEquipmentObjects(hit1.collider.gameObject)) return;

                //当前步骤中两个物体 选中了第一个时
                if (hit1.collider.gameObject.name.Equals(currentStepEquipment[0].name) &&
                    currentStepEquipment.Count == 2)
                {
                    isSelect = true;
                    selectedTrans = hit1.collider.transform;
                    RefreshSuggestGobj(currentStepEquipment[1]);
                }
                //当前步骤中两个物体 选中第二个时
                else if(currentStepEquipment.Count == 2 &&
                        hit1.collider.gameObject.name.Equals(currentStepEquipment[1].name) &&
                        isSelect)
                {
                    if (stepsIndex == 3 || stepsIndex == 11 )
                    {
                        //第三步 环刀刷凡士林 有特殊动画
                        BrushCircleKnife(selectedTrans, hit1.collider.transform);
                    }
                    else if (stepsIndex == 4 || stepsIndex == 12 )
                    {
                        MoveCircleKnifeToSoil(selectedTrans, hit1.collider.transform);
                    }
                    else if (stepsIndex == 6 || stepsIndex == 14 )
                    {
                        MoveXIAOKnifeToSoil(selectedTrans, hit1.collider.transform);
                    }
                    else if (stepsIndex == 7 || stepsIndex == 15)
                    {
                        MoveGUAKnifeToSoil(selectedTrans, hit1.collider.transform);
                    }
                    else
                    {
                        MoveEquipment(selectedTrans, hit1.collider.transform);
                    }
                    
                    isSelect = false;
                }
                //当前步骤中只有一个物体 选中时
                else if(!isSelect && currentStepEquipment.Count == 1 && hit1.collider.gameObject.name.Equals(currentStepEquipment[0].name))
                {
                    //清理环刀
                    if (stepsIndex == 10)
                    {
                        circleKnifeTrans.GetChild(1).gameObject.SetActive(false);
                    }
                    
                    if (stepsIndex == 5 || stepsIndex == 13)
                    {
                        //第五步 环刀插进土里
                        MoveCircleKnifeInSoil(selectedTrans);
                    }


                    //若出现需要点击确认的交互操作 ui点击消失后再读取下一步的信息
                    else if (!nowBtnText.Equals(""))
                    {
                        MoveEquipmentCallbackWithUIOption();
                    }
                    else
                    {
                        MoveEquipmentCallback();
                    }
                }

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

    private List<GameObject> currentStepEquipment;

    private string nowBtnText;

    //获取与当前操作有关的物体
    void GetCurrentStepEquipment()
    {
        int objectInStepId = -1;
        foreach (var experimentInstructionConfigInfoData in ExperimentInstructionConfigInfo.Datas)
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
        Debug.Log("refresh suggest");
        if (ControllerExperiment.Instance.stepsIndex >= 11) return;
        ToolManager.Instance.SetHighlightOnSuggest(suggestGobj.transform);
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

    void MoveEquipment(Transform selecteTrans, Transform targetTrans)
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
            .AppendCallback(MoveEquipmentCallback);
        
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
    
    void ReturnOriginPos(GameObject gObj,float duration,bool isRotated)
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
    
    
}
