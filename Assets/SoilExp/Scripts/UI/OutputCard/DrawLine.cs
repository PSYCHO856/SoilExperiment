using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawLine : MaskableGraphic, IPointerEnterHandler, IPointerExitHandler
{
    public Texture texture;
    public float lineWidth = 2;
    public override Texture mainTexture => texture;
    

    List<List<UIVertex>> vertexQuadList = new List<List<UIVertex>>();
    List<UIVertex> vertexQuad;
    bool bOver;
    Vector3 lastleftPoint;
    Vector3 lastrightPoint;
    Vector3 lastPos;

    private RectTransform rectParent;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        for (int i = 0; i < vertexQuadList.Count; i++)
        {
            vh.AddUIVertexQuad(vertexQuadList[i].ToArray());
        }
    }

    protected override void Awake()
    {
        base.Awake();
        rectParent = transform.GetComponent<RectTransform>();

    }

    void Update()
    {
        if (!bOver) return;

        if (Input.GetMouseButtonDown(0))
        {
            //vertexQuadList.Clear();
            lastPos = Input.mousePosition;

            //lastleftPoint = lastPos - new Vector3(Screen.width / 2, Screen.height / 2, 0) + Vector3.up * lineWidth;
            //lastrightPoint = lastPos - new Vector3(Screen.width / 2, Screen.height / 2, 0) - Vector3.up * lineWidth;
            Debug.Log("mouse " + lastPos);
            //lastleftPoint = lastPos - new Vector3(transform.position.x, transform.position.y, 0) + Vector3.up * lineWidth;
            //lastrightPoint = lastPos - new Vector3(transform.position.x, transform.position.y, 0) - Vector3.up * lineWidth;
            lastleftPoint = lastPos + Vector3.up * lineWidth;
            lastrightPoint = lastPos - Vector3.up * lineWidth;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, lastleftPoint, UIController._camera, out Vector2 localLeftPoint);
            lastleftPoint = localLeftPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, lastrightPoint, UIController._camera, out Vector2 localRightPoint);
            lastrightPoint = localRightPoint;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 newVec = Input.mousePosition - lastPos;
                if (newVec.magnitude < 0.1f)
                {
                    return;
                }

                vertexQuad = new List<UIVertex>();
                Vector3 vec = Vector3.Cross(newVec.normalized, Vector3.forward).normalized;

                //Vector3 newleftPoint = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0) + vec * lineWidth;
                //Vector3 newrightPoint = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0) - vec * lineWidth;

                //Vector3 newleftPoint = Input.mousePosition - new Vector3(transform.position.x, transform.position.y, 0) + vec * lineWidth;
                //Vector3 newrightPoint = Input.mousePosition - new Vector3(transform.position.x, transform.position.y, 0) - vec * lineWidth;
                Vector3 newleftPoint = Input.mousePosition + vec * lineWidth;
                Vector3 newrightPoint = Input.mousePosition - vec * lineWidth;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, newleftPoint, UIController._camera, out Vector2 localLeftPoint);
                newleftPoint = localLeftPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, newrightPoint, UIController._camera, out Vector2 localRightPoint);
                newrightPoint = localRightPoint;

                //uivertex是世界坐标 所以鼠标点击屏幕先转世界
                //Vector2 pos = UIController.Instance._camera.ScreenToWorldPoint(lastleftPoint);



                UIVertex uIVertex = new UIVertex();
                uIVertex.position = lastleftPoint;
                uIVertex.color = color;
                vertexQuad.Add(uIVertex);
                UIVertex uIVertex1 = new UIVertex();
                uIVertex1.position = lastrightPoint;
                uIVertex1.color = color;
                vertexQuad.Add(uIVertex1);
                UIVertex uIVertex3 = new UIVertex();
                uIVertex3.position = newrightPoint;
                uIVertex3.color = color;
                vertexQuad.Add(uIVertex3);
                UIVertex uIVertex2 = new UIVertex();
                uIVertex2.position = newleftPoint;
                uIVertex2.color = color;
                vertexQuad.Add(uIVertex2);
                lastleftPoint = newleftPoint;
                lastrightPoint = newrightPoint;
                vertexQuadList.Add(vertexQuad);

                lastPos = Input.mousePosition;

                SetVerticesDirty();
            }
        }
    }


    public void ClearAllLines()
    {
        vertexQuadList.Clear();
        SetVerticesDirty();
    }

    public void WithdrawLastLine()
    {
        vertexQuadList.RemoveAt(vertexQuadList.Count - 1);
        SetVerticesDirty();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        bOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bOver = false;
    }
}

