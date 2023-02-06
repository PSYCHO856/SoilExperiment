using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPageMenu : MonoBehaviour
{

    [SerializeField] private Button btnMenu;

    [SerializeField] private Button btnAppendixPanel;
    [SerializeField] private Button btnWithDraw;
    [SerializeField] private Button btnWithDrawInput;
    //[SerializeField] private Button btnWithDrawDrawing;
    public Button btnWithDrawAll;
    public Button BtnSave;


    [SerializeField] private GameObject menu;
    // [SerializeField] private GameObject AppendixPanel;
    [SerializeField] private GameObject withdrawMenu;

    //public Card1Page card1Page;


    private void Awake()
    {
        btnMenu.onClick.AddListener(OnMenu);
        btnAppendixPanel.onClick.AddListener(OnAppendixPanel);
        btnWithDrawInput.onClick.AddListener(OnWithDrawInput);
        btnWithDrawAll.onClick.AddListener(OnWithDrawAll);

        //btnTable.onClick.AddListener(OnTable);
        //btnTable.onClick.AddListener(OnTable);
        //btnTable.onClick.AddListener(OnTable);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && menu.activeSelf 
            && 
            !RectTransformUtility.RectangleContainsScreenPoint(btnMenu.GetComponent<RectTransform>(), Input.mousePosition, UIController._camera)
            &&
            !RectTransformUtility.RectangleContainsScreenPoint(btnAppendixPanel.GetComponent<RectTransform>(), Input.mousePosition, UIController._camera)
            &&
            !RectTransformUtility.RectangleContainsScreenPoint(BtnSave.GetComponent<RectTransform>(), Input.mousePosition, UIController._camera)
            &&

            !RectTransformUtility.RectangleContainsScreenPoint(btnWithDrawInput.GetComponent<RectTransform>(), Input.mousePosition, UIController._camera)
            &&
            !RectTransformUtility.RectangleContainsScreenPoint(btnWithDrawAll.GetComponent<RectTransform>(), Input.mousePosition, UIController._camera))
        {
            //Debug.Log("Update CardPageMenu SetActive");
            menu.SetActive(false);
        }
    }

    public void MenuInit()
    {
        menu.SetActive(false);
        // AppendixPanel.SetActive(false);
        withdrawMenu.SetActive(false);
    }

    void OnMenu()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }

    }

    void OnAppendixPanel()
    {
        // AppendixPanel.SetActive(true);
    }

    void OnWithDrawInput()
    {
        Messenger.Broadcast(GameEvent.INPUTFIELD_WITHDRAW);
        menu.SetActive(false);

    }

    void OnWithDrawAll()
    {
        menu.SetActive(false);
    }
    
}
