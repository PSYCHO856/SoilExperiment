using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldListener : MonoBehaviour
{
    private string valueText;
    private string endValue;
    InputField inputField;
    bool isChange;
    public bool isWithAnswer;
    public string correctAnswer = "正确答案：";
    public Text questionText;

    void Awake()
    {
        inputField = transform.GetComponent<InputField>();
        inputField.onValueChanged.AddListener(ChangedValue);//用户输入文本时就会调用
        inputField.onEndEdit.AddListener(EndValue);//文本输入结束时会调用
        //isChange = false;
    }

    //用户输入时的变化
    private void ChangedValue(string value)
    {
        valueText = value;//将用户输入的值赋值给内部的空字符串，我们可以将其来进行后续的操作
        //Debug.Log("输入了" + value);
        //isChange = true;
        if (!inputField.text.Equals(""))
        {
            Messenger<GameObject>.Broadcast(GameEvent.INPUTFIELD_RECORD, gameObject);
        }
    }
    private void EndValue(string value)
    {
        endValue = value;//捕捉数据，方便后续操作
        //Debug.Log("最终内容" + value);
        if (isWithAnswer)
        {
            questionText.text = correctAnswer;
        }
    }

    public void ClearInputField()
    {
        inputField.text = "";
        //isChange = false;
    }

    public void Selected()
    {

    }
}
