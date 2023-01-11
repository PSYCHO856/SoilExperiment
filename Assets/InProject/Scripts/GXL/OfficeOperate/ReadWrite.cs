using System.Collections.Generic;
using UnityEngine;
using NPOI.XWPF.UserModel;
using System.IO;
using System.Diagnostics;

public class ReadWrite : MonoBehaviour
{
    /// <summary>
    /// 存放word中需要替换的关键字以及对应要更改的内容
    /// </summary>
    public Dictionary<string, string> DicWord = new Dictionary<string, string>();

    /// <summary>
    /// Word模板路径
    /// </summary>
    private string path = @"D:\FrameProject\Industry-frame\Assets\InProject\Ex_word\水库洪水调节报告书模板.docx";

    /// <summary>
    /// 更改后的文件路径
    /// P.S. 当然可以直接修改模板文件，这里另外生成文件是个人为了方便测试。
    /// </summary>
    private string targetPath = @"D:\FrameProject\Industry-frame\Assets\InProject\水库洪水调节报告书模板.docx";


    private void Start()
    {
        DicWord.Add("{Z}", "{假如生活欺骗了1你}");//替换word文本

        ReplaceKeyword();
    }

    /// <summary>
    /// 替换关键字
    /// </summary>
    private void ReplaceKeyword()
    {
        using (FileStream stream = File.OpenRead(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            XWPFDocument doc = new XWPFDocument(fs);

            //遍历段落                  
            foreach (var para in doc.Paragraphs)
            {
                string oldText = para.ParagraphText;
                if (oldText != "" && oldText != string.Empty && oldText != null)
                {
                    string tempText = para.ParagraphText;

                    foreach (KeyValuePair<string, string> kvp in DicWord)
                    {
                        if (tempText.Contains(kvp.Key))
                        {
                            tempText = tempText.Replace(kvp.Key, kvp.Value);

                            para.ReplaceText(oldText, tempText);
                        }
                    }

                }
            }

            //遍历表格      
            var tables = doc.Tables;
            foreach (var table in tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        foreach (var para in cell.Paragraphs)
                        {
                            string oldText = para.ParagraphText;
                            if (oldText != "" && oldText != string.Empty && oldText != null)
                            {
                                //记录段落文本
                                string tempText = para.ParagraphText;
                                foreach (KeyValuePair<string, string> kvp in DicWord)
                                {
                                    if (tempText.Contains(kvp.Key))
                                    {
                                        tempText = tempText.Replace(kvp.Key, kvp.Value);

                                        //替换内容
                                        para.ReplaceText(oldText, tempText);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //生成指定文件
            FileStream output = new FileStream(targetPath, FileMode.Create);
            //将文档信息写入文件
            doc.Write(output);

            //一些列关闭释放操作
            fs.Close();
            fs.Dispose();
            output.Close();
            output.Dispose();

            Process.Start(targetPath);//打开指定文件
        }
    }
}