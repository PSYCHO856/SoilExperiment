using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    [Header("打字速度")]
    public float Speed = 15;

    public Text text;
    IEnumerator typeText;

    void Start()
    {
        text = this.GetComponent<Text>();
        
    }
    public void Run(string textToType, Text textLabel)
    {
        StopAllCoroutines();
        text.text = "";
        
        typeText = TypeText(textToType, textLabel);
        StartCoroutine(typeText);
    }
    IEnumerator TypeText(string textToType, Text textLabel)
    {
        float t = 0;//经过的时间
        int charIndex = 0;//字符串索引值
        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * Speed;//简单计时器赋值给t
            charIndex = Mathf.FloorToInt(t);//把t转为int类型赋值给charIndex
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);
            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }
        textLabel.text = textToType;
    }

    public void RecycleOnClose()
    {
        StopCoroutine(typeText);
        text.text = "";
    }
}

