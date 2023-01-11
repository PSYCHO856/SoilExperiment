using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ControllerExperiment : preProject.Singleton<ControllerExperiment>
{
    public string QId1= "42101";
    public string QId2= "44201";
    
    private RaycastHit hit1;
    private Ray ray1;

    public Camera mainCam;
    public Camera uiCam;

    public int stepsIndex;

    public List<List<GameObject>> objectsInSteps;
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
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ray1 = GetMyRay();
            if (Physics.Raycast(ray1, out hit1))
            {

                //限定当前步骤的可点设备 限定先后点击顺序
                if (!isSceneEquipmentObjects(hit1.collider.gameObject)) return;
                
                if (hit1.collider.gameObject.CompareTag("ExpObject"))
                {
                    if (isSelect && hit1.collider.gameObject.name.Equals("电子秤") && selectedTrans.gameObject.name.Equals("环刀"))
                    {
                        WeighCircleKnife(selectedTrans, hit1.collider.transform);
                    }

                    if (!isSelect && hit1.collider.gameObject.name.Equals("电子秤"))
                    {
                        RefreshSteps();
                    }
                    
                    if (isSelect && hit1.collider.gameObject.name.Equals("土样") && selectedTrans.gameObject.name.Equals("环刀"))
                    {
                        MoveCircleKnifeToSoil(selectedTrans, hit1.collider.transform);
                    }
                    

                }

                if (hit1.collider.gameObject.CompareTag("ExpMovableObject"))
                {
                    if (isSelect && hit1.collider.gameObject.name.Equals("环刀") && selectedTrans.gameObject.name.Equals("刷子"))
                    {
                        BrushCircleKnife(selectedTrans, hit1.collider.transform);
                    }
                    
                    isSelect = true;
                    selectedTrans = hit1.collider.transform;
                }
                
                if (hit1.collider.gameObject.CompareTag("ExpObject")||hit1.collider.gameObject.CompareTag("ExpMovableObject"))
                {
                    Debug.Log("设置高亮"+ hit1.collider.gameObject.name);
                    ToolManager.Instance.SetHighlightOn(hit1.collider.gameObject.transform, 4);

                    //获取实验器具的说明
                    if (!CheckEquipment(hit1.collider.gameObject.name))
                    {
                        instructionString = GetEquipmentInstruction(hit1.collider.gameObject.name);
                        if (instructionString == "") return;
                        Messenger<string>.Broadcast(GameEvent.ON_INSTRUCTION_UPDATE,instructionString);
                    }
                    //若可操作 显示ui
                    if (canOperate)
                    {
                        Messenger<Vector3>.Broadcast(GameEvent.ON_OPERATE_UPDATE, hit1.collider.transform.position);
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
    //获取与当前操作有关的物体
    void GetCurrentStepEquipment()
    {
        int objectInStepId = -1;
        foreach (var experimentInstructionConfigInfoData in ExperimentInstructionConfigInfo.Datas)
        {
            if (experimentInstructionConfigInfoData.Value.StepIndex.Equals(stepsIndex.ToString()))
            {
                objectInStepId = Int32.Parse(experimentInstructionConfigInfoData.Value.ObjectInStepId);
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
                if(objectInStepConfigInfoData.Value.Name2!="")currentStepEquipment.Add(FindObjectInScene(objectInStepConfigInfoData.Value.Name1));
            }
        }
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
            if (gObj == sceneEquipmentObject) return true;
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
        float moveDuration = 1f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(selecteTrans.DOMove(new Vector3(position.x, position.y + 0.4f, position.z), moveDuration)) // 添加动画到队列中
            .AppendInterval(moveDuration) // 添加时间间隔
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position.y + 0.4f, position1.z), moveDuration))
            .AppendInterval(moveDuration)
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + (selectedTransHeight + targetTransHeight)/2, position1.z), moveDuration))
            .AppendCallback(MoveEquipmentCallback);
        
        //(selectedTransHeight + targetTransHeight)/2
    }
    
    void MoveCircleKnifeToSoil(Transform selecteTrans, Transform targetTrans)
    {
        if (!selecteTrans) return;

        var position = selecteTrans.position;
        var position1 = targetTrans.position;
        float selectedTransHeight = selecteTrans.GetComponent<BoxCollider>().size.y * selecteTrans.localScale.y;
        float targetTransHeight = targetTrans.GetComponent<BoxCollider>().size.y * targetTrans.localScale.y;
        float moveDuration = 1f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(selecteTrans.DOMove(new Vector3(position.x, position.y + 0.4f, position.z), moveDuration)) // 添加动画到队列中
            .AppendInterval(moveDuration) // 添加时间间隔
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position.y + 0.4f, position1.z), moveDuration))
            .AppendInterval(moveDuration)
            .Append(selecteTrans.DORotate(new Vector3(180, 0, 0), moveDuration))
            .AppendInterval(moveDuration)
            
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position1.y + (selectedTransHeight + targetTransHeight)/2, position1.z), moveDuration))
            .AppendCallback(MoveEquipmentCallback);
    }

    void MoveEquipmentCallback()
    {
        
    }
    
    //盛环刀重量
    void WeighCircleKnife(Transform selecteTrans, Transform targetTrans)
    {
        MoveEquipment(selecteTrans, targetTrans);

        isSelect = false;
        selectedTrans = null;

        RefreshSteps();

    }

    float brushMoveHeight = 0.55f;
    //涂抹凡士林
    void BrushCircleKnife(Transform selecteTrans, Transform targetTrans)
    {
        if (!selecteTrans) return;
        var position = selecteTrans.position;
        var position1 = targetTrans.position;

        float moveDuration = 1f;
        DOTween.Sequence() // 返回一个新的Sequence
            .Append(
                selecteTrans.DOMove(new Vector3(position.x, position.y + brushMoveHeight, position.z),
                    moveDuration)) // 添加动画到队列中
            .AppendInterval(moveDuration) // 添加时间间隔
            .Append(targetTrans.DOMove(new Vector3(position1.x, position.y + 0.4f, position1.z), moveDuration))
            .AppendInterval(moveDuration)
            .Append(
                selecteTrans.DOMove(new Vector3(position1.x, position.y + brushMoveHeight, position1.z),
                    moveDuration))
            .AppendInterval(moveDuration)
            
            .Append(
                selecteTrans.DOMove(new Vector3(position1.x, position.y + brushMoveHeight - 0.05f, position1.z), moveDuration))
            .AppendInterval(moveDuration)
            
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position.y + brushMoveHeight - 0.05f, position1.z + 0.05f), moveDuration))
            .AppendInterval(moveDuration)
            .Append(selecteTrans.DOMove(new Vector3(position1.x, position.y + brushMoveHeight - 0.05f, position1.z - 0.02f), moveDuration))
            .AppendInterval(moveDuration)
            .Append(
                selecteTrans.DOMove(new Vector3(position1.x, position.y + brushMoveHeight, position1.z), moveDuration))
            .AppendInterval(moveDuration)
            .Append(
                selecteTrans.DOMove(new Vector3(position.x, position.y + brushMoveHeight, position.z), moveDuration))
            .AppendInterval(moveDuration)
            .Append(
                selecteTrans.DOMove(new Vector3(position.x, position.y, position.z), moveDuration))
            .AppendInterval(moveDuration)
            // .Append(targetTrans.DOMove(new Vector3(position1.x, position1.y, position1.z), moveDuration))
            ;
        ReturnOriginPos(targetTrans.gameObject);
        
        isSelect = false;
        selectedTrans = null;

        RefreshSteps();
        
    }
    
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
    
    void ReturnOriginPos(GameObject gObj)
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
        float moveDuration = 1f;
        DOTween.Sequence() // 返回一个新的Sequence
            .AppendInterval(18)
            .Append(
                gTrans.DOMove(new Vector3(oriPos.x, oriPos.y + 0.4f, oriPos.z),
                    moveDuration)) // 添加动画到队列中
            .AppendInterval(moveDuration) // 添加时间间隔
            .Append(gTrans.DOMove(new Vector3(oriPos.x, oriPos.y, oriPos.z), moveDuration))
            .AppendInterval(moveDuration);
    }
    
    
}
