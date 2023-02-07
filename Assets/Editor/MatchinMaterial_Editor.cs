using System.IO;
using UnityEngine;
using UnityEditor;

public class MatchinMaterial_Editor : EditorWindow
{
    public string modelPath = "Assets";
    public Rect modelRect;
    // public string MainTexture2D = "_MainTexture2D";
    // public string Metallic = "_MetallicTexture2D";
    // public string NormalMap = "_NormalTexture2D";
    public string MainTexture2D = "_BaseMap";
    public string Metallic = "_MetallicGlossMap";
    public string NormalMap = "_BumpMap";
    // public string HeightMap = "_ParallaxMap";
    // public string OcclusionMap = "_OcclusionMap";
    // public string DetailMaskMap = "_DetailMask";
    private static MatchinMaterial_Editor _window;
    [SerializeField]
    public string m_MainTex = "_BaseMap", m_Metallic = "_Metallic", m_NormalMap = "_Normal"/*, m_HeightMap = "_Height", m_OcclusionMap = "_Occlusion"
        ,m_DetailMaskMap = "_DetailMask"*/;

    [MenuItem("Tools/材质匹配")]
    public static void showWindow()
    {
        Rect wr = new Rect(0, 0, 300, 300);
        // true 表示不能停靠的
        _window = (MatchinMaterial_Editor)GetWindowWithRect(typeof(MatchinMaterial_Editor), wr, true, "材质匹配");
        _window.Show();
    }

    public void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("模型路径 (鼠标拖拽文件夹到这里)");
        EditorGUILayout.Space();
        GUI.SetNextControlName("input1");//设置下一个控件的名字
        modelRect = EditorGUILayout.GetControlRect();
        modelPath = EditorGUI.TextField(modelRect, modelPath);
        EditorGUILayout.Space();
        m_MainTex = EditorGUILayout.TextField("MainTex图片后缀", m_MainTex);
        m_Metallic = EditorGUILayout.TextField("Metallic图片后缀", m_Metallic);
        m_NormalMap = EditorGUILayout.TextField("NormalMap图片后缀", m_NormalMap);
        // m_HeightMap = EditorGUILayout.TextField("HeightMap图片后缀", m_HeightMap);
        // m_OcclusionMap = EditorGUILayout.TextField("OcclusionMap图片后缀", m_OcclusionMap);
        // m_DetailMaskMap = EditorGUILayout.TextField("DetailMaskMap图片后缀", m_DetailMaskMap);
        DragFolder();

        EditorGUILayout.Space();

        // 导出材质
        if (GUILayout.Button("导出材质球"))
        {
            ForEachModels();
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("设置材质"))
        {
            ForEachMaterials();
        }
    }

    /// <summary>
    /// 获得拖拽文件
    /// </summary>
    void DragFolder()
    {
        //鼠标位于当前窗口
        if (mouseOverWindow == this)
        {
            //拖入窗口未松开鼠标
            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;//改变鼠标外观
                // 判断区域
                if (modelRect.Contains(Event.current.mousePosition))
                    GUI.FocusControl("input1");
            }
            //拖入窗口并松开鼠标
            else if (Event.current.type == EventType.DragExited)
            {
                string dragPath = string.Join("", DragAndDrop.paths);
                // 判断区域
                if (modelRect.Contains(Event.current.mousePosition))
                    this.modelPath = dragPath;

                // 取消焦点(不然GUI不会刷新)
                GUI.FocusControl(null);
            }
        }
    }

    /// <summary>
    /// 导出材质
    /// </summary>
    void ForEachModels()
    {
        string[] allPath = AssetDatabase.FindAssets("t:GameObject", new string[] { modelPath });
        //Debug.Log("-- allPath: " + allPath.Length);
        for (int i = 0, len = allPath.Length; i < len; i++)
        {
            string filePath = AssetDatabase.GUIDToAssetPath(allPath[i]);
            // 设置模型
            ExtractMaterialsFromFBX(filePath);
        }
        // 如果选取的是FBX模型文件
        if (allPath.Length == 0)
        {
            if (Path.GetExtension(modelPath) == ".fbx")
            {
                ExtractMaterialsFromFBX(modelPath);
            }
            else
            {
                Debug.LogError("当前选择目录未找到FBX文件: " + this.modelPath);
            }
        }

    }

    /// <summary>
    /// 导出材质球
    /// </summary>
    /// <param name="assetPath"></param>
    public void ExtractMaterialsFromFBX(string assetPath)
    {
        
        // 材质目录
        string materialFolder = Path.GetDirectoryName(assetPath) + "/Material";
        //Debug.Log(assetPath); 
        // 如果不存在该文件夹则创建一个新的
        if (!AssetDatabase.IsValidFolder(materialFolder))
            AssetDatabase.CreateFolder(Path.GetDirectoryName(assetPath), "Material");
        // 获取 assetPath 下所有资源。
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
        foreach (Object item in assets)
        {
            if (item.GetType() == typeof(Material))
            {
                string path = System.IO.Path.Combine(materialFolder, item.name) + ".mat";
                // 为资源创建一个新的唯一路径。
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                // 通过在导入资源（例如，FBX 文件）中提取外部资源，在对象（例如，材质）中创建此资源。
                string value = AssetDatabase.ExtractAsset(item, path);
                // 成功提取( 如果 Unity 已成功提取资源，则返回一个空字符串)
                if (string.IsNullOrEmpty(value))
                {
                    AssetDatabase.WriteImportSettingsIfDirty(assetPath);
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                    Debug.Log(Path.GetFileName(assetPath) + " 的 Material 导出成功!!");
                }
            }
        }

    }

    /// <summary>
    /// 得到所有材质球
    /// </summary>
    void ForEachMaterials()
    {
        string[] allPath = GetObjPath("Material");
        for (int i = 0, len = allPath.Length; i < len; i++)
        {
            string filePath = AssetDatabase.GUIDToAssetPath(allPath[i]);
            setMaterialShader(filePath);
        }
        Debug.Log("Material Shader 设置完成, 一共: " + allPath.Length + "个");
    }

    void setMaterialShader(string path)
    {
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        mat.shader=Shader.Find("Universal Render Pipeline/Lit");
        string[] allPath = GetObjPath("Texture2D");
        for (int i = 0, len = allPath.Length; i < len; i++)
        {
            string filePath = AssetDatabase.GUIDToAssetPath(allPath[i]);
            Texture2D t = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
            string objName = Path.GetFileName(modelPath);
            objName = objName.Substring(0, objName.Length - 4);

            if (t.name == objName + "_" + mat.name + m_MainTex)
            {
                mat.SetTexture(MainTexture2D, t);
                Debug.Log("Material: " + mat.name + " SetTexture: " + t.name);
            }
            // else if (t.name == objName + "_" + mat.name + m_HeightMap)
            // {
            //     mat.SetTexture(HeightMap, t);
            // }
            else if (t.name == objName + "_" + mat.name + m_Metallic)
            {
                mat.SetTexture(Metallic, t);
            }
            else if (t.name == objName + "_" + mat.name + m_NormalMap)
            {
                mat.SetTexture(NormalMap, t);
            }
            // else if (t.name == objName + "_" + mat.name + m_OcclusionMap)
            // {
            //     mat.SetTexture(OcclusionMap, t);
            // }
            // else if (t.name == objName + "_" + mat.name + m_DetailMaskMap)
            // {
            //     mat.SetTexture(DetailMaskMap, t);
            // }
            else
            {
                // Debug.Log(mat.name+"材质球无法匹配:" + t.name);
            }
        }
    }

    string[] GetObjPath(string mType)
    {
        //string path = Path.GetDirectoryName(modelPath) + "/Material";//得到此物体的文件目录,具体到Material文件
        string path = Path.GetDirectoryName(modelPath);//得到此物体的文件目录
        return AssetDatabase.FindAssets("t:"+ mType, new string[] { path });//在这个文件下找"t:Material"这个文件
    }
}

