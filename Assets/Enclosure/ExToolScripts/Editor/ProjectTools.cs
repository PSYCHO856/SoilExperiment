using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class ProjectTools : MonoBehaviour
{
    
    //[MenuItem("ProjectTools/修改文件夹下的预制体名称")]
    public static void ModifyFile()
    {
        //int index = 0;
        UnityEngine.Object[] m_objects = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);//选择的所以对象
        foreach (UnityEngine.Object item in m_objects)
        {
            if (Path.GetExtension(AssetDatabase.GetAssetPath(item)) != "")//判断路径是否为空
            {
                string path = AssetDatabase.GetAssetPath(item);// item.name;//

                string OldName = item.name;
                int index2 = OldName.LastIndexOf('_');
                string IndexName = OldName.Substring(0, index2 - 1);
                string LastName = OldName.Substring(index2, OldName.Length - index2);
                string NewName = IndexName + "7" + LastName;
                AssetDatabase.RenameAsset(path, NewName);

            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
   // [MenuItem("ProjectTools/智能检测/Remove Missing-MonoBehavior Component")]
    static public void RemoveMissComponent()
    {
        string DataPath = "/Resources/PackageRes/Raw/Prefabs/Puzzle/A_ChuanPeiPre";

        string fullPath = Application.dataPath + DataPath;

        fullPath = fullPath.Replace("/", @"\");
        //List<string> pathList = GetAssetsPathByFullPath(fullPath, "*.prefab", SearchOption.AllDirectories);
        List<string> pathList = GetAssetsPathByRelativePath(new string[] { "Assets"+ DataPath }, "t:Prefab", SearchOption.AllDirectories);
        int counter = 0;
        for (int i = 0, iMax = pathList.Count; i < iMax; i++)
        {
            EditorUtility.DisplayProgressBar("处理进度", string.Format("{0}/{1}", i + 1, iMax), (i + 1f) / iMax);

            if (CheckMissMonoBehavior(pathList[i]))
                ++counter;
        }
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("处理结果", "完成修改，修改数量 : " + counter, "确定");
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 获取项目中某种资源的路径
    /// </summary>
    /// <param name="relativePath">unity路径格式，以 "/" 为分隔符</param>
    /// <param name="filter">unity的资源过滤模式 https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html </param>
    /// <param name="searchOption"></param>
    /// <returns></returns>
    static List<string> GetAssetsPathByRelativePath(string[] relativePath, string filter, SearchOption searchOption)
    {
        List<string> pathList = new List<string>();
        string[] guids = AssetDatabase.FindAssets(filter, relativePath);
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            pathList.Add(path);
        }
        return pathList;
    }

    /// <summary>  
    /// 删除一个Prefab上的空脚本  
    /// </summary>  
    /// <param name="path">prefab路径 例Assets/Resources/FriendInfo.prefab</param>  
    static bool CheckMissMonoBehavior(string path)
    {
        bool isNull = false;
        string textContent = File.ReadAllText(path);
        Regex regBlock = new Regex("MonoBehaviour");
        // 以"---"划分组件  
        //划分模块
        string[] strArray = textContent.Split(new string[] { "---" }, StringSplitOptions.RemoveEmptyEntries);
        
        //遍历模块
        for (int i = 0; i < strArray.Length; i++)
        {
            string blockStr = strArray[i];
            //找到需要的模块
            if (regBlock.IsMatch(blockStr))
            {
                // 模块是 MonoBehavior  
                //(?<名称>子表达式)  含义:将匹配的子表达式捕获到一个命名组中
                //将
                Match guidMatch = Regex.Match(blockStr, "m_Script: {fileID: (.*), guid: (?<GuidValue>.*?), type: [0-9]}");
                if (guidMatch.Success)
                {
                    string guid = guidMatch.Groups["GuidValue"].Value;
                    if (!File.Exists(AssetDatabase.GUIDToAssetPath(guid)))
                    {
                        isNull = true;
                        textContent = DeleteContent(textContent, blockStr);
                    }
                }

                Match fileIdMatch = Regex.Match(blockStr, @"m_Script: {fileID: (?<IdValue>\d+)}");
                if (fileIdMatch.Success)
                {
                    string idValue = fileIdMatch.Groups["IdValue"].Value;
                    if (idValue.Equals("0"))
                    {
                        isNull = true;
                        textContent = DeleteContent(textContent, blockStr);
                    }
                }
            }
        }
        if (isNull)
        {
            // 有空脚本 写回prefab  
            File.WriteAllText(path, textContent);
        }
        return isNull;
    }
    // 删除操作  
    static string DeleteContent(string input, string blockStr)
    {
        input = input.Replace("---" + blockStr, "");
        Match idMatch = Regex.Match(blockStr, "!u!(.*) &(?<idValue>.*?)\n");
        if (idMatch.Success)
        {
            // 获取 MonoBehavior的fileID 
            string fileID = idMatch.Groups["idValue"].Value;
            Regex regex = new Regex("  - (.*): {fileID: " + fileID + "}\n");
            input = regex.Replace(input, "");
        }
        return input;
    }
}
