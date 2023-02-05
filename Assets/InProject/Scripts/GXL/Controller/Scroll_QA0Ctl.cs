using UnityEngine.UI;
using UnityEngine;

public class Scroll_QA0Ctl : MonoBehaviour
{
    public GameObject obj_answer;
    public InputField inputField;
    private void OnDisable()
    {
        obj_answer.SetActive(false);
        inputField.text="";
    }
}
