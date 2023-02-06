using UnityEngine;
using UnityEngine.UI;

public class Grid : MaskableGraphic
{
    /// <summary>
    /// 网格线间隔
    /// </summary>
    public int gridSpace = 50;
    /// <summary>
    /// 网格线的像素宽度
    /// </summary>
    public float gridLineWidth = 1.0f;
    //可以自定义网格线颜色、如渐变色等，这里我是直接使用基类的颜色

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        //取整数
        float width = Mathf.RoundToInt(rectTransform.rect.width);
        float height = Mathf.RoundToInt(rectTransform.rect.height);
        gridSpace = (int)Mathf.Clamp(gridSpace, 0, width);

        //先画水平方向上的线，从左到右绘制垂直线段
        for (int i = 0; i < width; i += gridSpace)
        {
            //四个点可以绘制一个矩形面片
            var horizontal_A = new Vector2(i, 0);
            var horizontal_B = new Vector2(i, height);
            var horizontal_C = new Vector2(i + gridLineWidth, height);
            var horizontal_D = new Vector2(i + gridLineWidth, 0);
            vh.AddUIVertexQuad(GetRectangleQuad(color, horizontal_A, horizontal_B, horizontal_C, horizontal_D));
        }
        //最后画垂直方向上的线，从下到上绘制水平线段
        for (int i = 0; i < height; i += gridSpace)
        {
            var vertical_A = new Vector2(0, i);
            var vertical_B = new Vector2(0, i + gridLineWidth);
            var vertical_C = new Vector2(width, i + gridLineWidth);
            var vertical_D = new Vector2(width, i);
            vh.AddUIVertexQuad(GetRectangleQuad(color, vertical_A, vertical_B, vertical_C, vertical_D));
        }
    }

    //得到一个矩形面片
    private UIVertex[] GetRectangleQuad(Color color, params Vector2[] points)
    {
        UIVertex[] vertexs = new UIVertex[points.Length];
        for (int i = 0; i < vertexs.Length; i++)
        {
            vertexs[i] = GetUIVertex(points[i], color);
        }
        return vertexs;
    }

    //得到一个顶点信息
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