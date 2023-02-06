using UnityEngine;
using UnityEngine.UI;

public class Grid : MaskableGraphic
{
    /// <summary>
    /// �����߼��
    /// </summary>
    public int gridSpace = 50;
    /// <summary>
    /// �����ߵ����ؿ��
    /// </summary>
    public float gridLineWidth = 1.0f;
    //�����Զ�����������ɫ���罥��ɫ�ȣ���������ֱ��ʹ�û������ɫ

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        //ȡ����
        float width = Mathf.RoundToInt(rectTransform.rect.width);
        float height = Mathf.RoundToInt(rectTransform.rect.height);
        gridSpace = (int)Mathf.Clamp(gridSpace, 0, width);

        //�Ȼ�ˮƽ�����ϵ��ߣ������һ��ƴ�ֱ�߶�
        for (int i = 0; i < width; i += gridSpace)
        {
            //�ĸ�����Ի���һ��������Ƭ
            var horizontal_A = new Vector2(i, 0);
            var horizontal_B = new Vector2(i, height);
            var horizontal_C = new Vector2(i + gridLineWidth, height);
            var horizontal_D = new Vector2(i + gridLineWidth, 0);
            vh.AddUIVertexQuad(GetRectangleQuad(color, horizontal_A, horizontal_B, horizontal_C, horizontal_D));
        }
        //��󻭴�ֱ�����ϵ��ߣ����µ��ϻ���ˮƽ�߶�
        for (int i = 0; i < height; i += gridSpace)
        {
            var vertical_A = new Vector2(0, i);
            var vertical_B = new Vector2(0, i + gridLineWidth);
            var vertical_C = new Vector2(width, i + gridLineWidth);
            var vertical_D = new Vector2(width, i);
            vh.AddUIVertexQuad(GetRectangleQuad(color, vertical_A, vertical_B, vertical_C, vertical_D));
        }
    }

    //�õ�һ��������Ƭ
    private UIVertex[] GetRectangleQuad(Color color, params Vector2[] points)
    {
        UIVertex[] vertexs = new UIVertex[points.Length];
        for (int i = 0; i < vertexs.Length; i++)
        {
            vertexs[i] = GetUIVertex(points[i], color);
        }
        return vertexs;
    }

    //�õ�һ��������Ϣ
    private UIVertex GetUIVertex(Vector2 point, Color color)
    {
        UIVertex vertex = new UIVertex
        {
            position = point,
            color = color,
            uv0 = Vector2.zero
        };
        return vertex;
    }
}