using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Aspose.Words;
using System.Text.RegularExpressions;
using Aspose.Words.Replacing;
using Aspose.Words.Tables;
using System.Drawing;
using Aspose.Words.Fields;

public class IOTool
{
    /// <summary>
    /// 检测Word文件是否存在 
    /// </summary>
    /// <param name="filepath">路径</param>
    /// <returns></returns>
    public static bool IsExists(string filepath)
    {
        if (File.Exists(filepath))
        {
            Debug.LogWarning(filepath + "文件已存在");
            return true;
        }
        Debug.LogWarning(filepath + "文件不存在");
        return false;
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="filepath">路径</param>
    /// <param name="filename">World名字</param>
    public static void Create(string filepath, string filename)
    {
        string path = filepath + "/" + filename + ".doc";
        if (!IsExists(path))
        {
            File.Create(path);
            Debug.LogWarning(path + " World文件创建完毕 ");
        }
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="filepath">路径</param>
    /// <param name="filename">World名</param>
    public static void Delete(string filepath, string filename)
    {
        string path = filepath + "/" + filename + ".doc";
        if (IsExists(path))
        {
            File.Delete(path);
            Debug.LogWarning(path + " World文件删除完毕 ");
        }
    }


    /// <summary>
    /// 利用替换法 快速替换word模板中的内容， (文字 或者图片)---前提 模板不 能是打开状态，不能被任何进程占用
    /// </summary>
    /// <param name="path"></param>
    /// <param name="outPath"></param>
    /// <param name="wordReplaceDic"></param>
    /// <param name="imgReplaceLis"></param>
    public static void ReplaceToWord(string path, string outPath, Dictionary<string, string> wordReplaceDic, Dictionary<string, string> imgReplaceDic = null, Dictionary<string, bool> toggleReplaceDic = null)
    {
        if (!IsExists(path))
            return;

        Document doc = new Document(path);
        
        //替换文字
        foreach (var v in wordReplaceDic)
        {
            //替换特殊标志形式   替换  &施工单位&     为 安筑1
            //doc.Range.Replace("&施工单位&", "安筑1", findReplaceOptions);
            //替换特殊标志形式   替换  &工程名称&     为 安筑
            //doc.Range.Replace("&工程名称&", "安筑", findReplaceOptions);
            doc.Range.Replace(v.Key, v.Value, new FindReplaceOptions(FindReplaceDirection.Forward));
        }


        //这里只是展示了  Regex 和string  两个参数的用法，并非是一定要用某个
        //下边展示了图片插入的三种方法。完整路径----字节byte[]-----文件流Stream
        //Regex regex = new Regex("&Image&");
        //FindReplaceOptions findReplaceOptions1 = new FindReplaceOptions(new ReplaceAndInsertImage(@"C:\Users\Miggle\Desktop\111.png"));
        //doc.Range.Replace(regex, "", findReplaceOptions1);

        //FindReplaceOptions findReplaceOptions2 = new FindReplaceOptions(new ReplaceAndInsertImage(File.ReadAllBytes(@"C:\Users\Miggle\Desktop\111.png")));
        //doc.Range.Replace("&Image1&", "", findReplaceOptions2);

        //FindReplaceOptions findReplaceOptions3 = new FindReplaceOptions(new ReplaceAndInsertImage(new FileStream(@"C:\Users\Miggle\Desktop\111.png", FileMode.Open)));
        //doc.Range.Replace("&Image2&", "", findReplaceOptions3);

        //插入图片
        // WriteImage(doc, imgReplaceDic);

        //插入复选框
        // ChangeCheckBox(doc, toggleReplaceDic);

        //--另存为
        doc.Save(outPath);

        Debug.Log("保存成功！");
    }


    /// <summary>
    /// 插入图片
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="label"></param>
    /// <param name="imgPath"></param>
    /// <param name="w"></param>
    /// <param name="h"></param>
    private static void WriteImage(Document doc, Dictionary<string, string> imgReplaceDic)
    {
        //方法一
        //DocumentBuilder builder = new DocumentBuilder(doc);

        //foreach (var v in imgReplaceDic)
        //{           
        //    builder.MoveToBookmark(v.Key, true, true);              //定位特定字符的位置，插入图片                                                          
        //    FileStream fs = new FileStream(v.Value, FileMode.Open);   //读取图片，并以byte[]的方式写入
        //    byte[] imageByte = new byte[fs.Length];
        //    fs.Read(imageByte, 0, imageByte.Length);
        //    Image image = Image.FromStream(fs);
        //    builder.InsertImage(imageByte, 400d, 400d * image.Height / image.Width);
        //    Debug.Log("Width  400" + "Height  " + (400d * image.Height / image.Width).ToString());
        //    fs.Close();
        //}


        //方法二
        foreach (var v in imgReplaceDic)
        {                                                         
            FileStream fs = new FileStream(v.Value, FileMode.Open);   //读取图片 
            Image image = Image.FromStream(fs);
            Regex reg = new Regex(v.Key);
            doc.Range.Replace(reg, new ReplaceAndInsertImage(image, 400d, 400d * image.Height / image.Width), false);           
            Debug.Log("Width  400" + "Height  " + (400d * image.Height / image.Width).ToString());
            fs.Close();
        }

    }


    /// <summary>
    /// 添加表格
    /// </summary>
    /// <param name="doc"></param>
    private static void WriteExcel(Document doc)
    {
        DocumentBuilder builder = new DocumentBuilder(doc);
        NodeCollection allTables = doc.GetChildNodes(NodeType.Table, true);  //获取所有表格
        Table table = allTables[0] as Table;                                 //获取第一个表格
        var row = table.Rows[table.Rows.Count - 1];                          //在表格的最后一行插入数据
        builder.MoveToCell(0, table.Rows.Count - 1, 0, 0);                   //移动到单元格第几行第几列
        builder.Write("菜鸡");                                               //写入数据
        builder.MoveToCell(0, table.Rows.Count - 1, 1, 0);
        builder.Write("菜鸡");
        builder.MoveToCell(0, table.Rows.Count - 1, 2, 0);
        builder.Write("菜鸡");
        builder.MoveToCell(0, table.Rows.Count - 1, 3, 0);
        builder.Write("菜鸡");
        builder.MoveToCell(0, table.Rows.Count - 1, 4, 0);
        builder.Write("菜鸡");
        builder.MoveToCell(0, table.Rows.Count - 1, 5, 0);
        builder.Write("菜鸡");
        builder.MoveToCell(0, table.Rows.Count - 1, 6, 0);
        builder.Write("菜鸡");
    }


    /// <summary>
    /// 改变复选框的状态
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="toggleReplaceDic"></param>
    private static void ChangeCheckBox(Document doc, Dictionary<string, bool> toggleReplaceDic)
    {
        //方法一
        //DocumentBuilder builder = new DocumentBuilder(doc);

        //foreach(var item in toggleReplaceDic)
        //{
        //    foreach(Bookmark bookmark in doc.Range.Bookmarks)
        //    {
        //        if(bookmark.Name==item.Key)
        //        {
        //            bookmark.Text = "";
        //            builder.MoveToBookmark(bookmark.Name);
        //            builder.Font.Name = "Wingdings 2";
        //            if (item.Value)
        //            {
        //                Debug.Log(bookmark.Name + "勾选");//82      
        //                builder.Write(char.ConvertFromUtf32(82));
        //                //builder.InsertCheckBox("", true, true, 10);
        //            }
        //            else
        //            {
        //                Debug.Log(bookmark.Name + "不勾选");//163
        //                builder.Write(char.ConvertFromUtf32(163));
        //                //builder.InsertCheckBox("", true, false, 10);
        //            }
        //        }
        //    }
        //}



        //方法二
        foreach (var item in toggleReplaceDic)
        {
            Regex reg = new Regex(item.Key);
            if (item.Value)
            {
                doc.Range.Replace(reg, new ReplaceAndInsertSpecialSymbol("Wingdings 2", 82), false);
            }
            else
            {
                doc.Range.Replace(reg, new ReplaceAndInsertSpecialSymbol("Wingdings 2", 163), false);
            }                  
        }

    }

}


/// <summary>
/// 插入图片
/// </summary>
public class ReplaceAndInsertImage : IReplacingCallback
{
    /// <summary>
    /// 需要插入的图片
    /// </summary>
    public Image img { get; set; }

    /// <summary>
    /// 宽
    /// </summary>
    public double w { get; set; }

    /// <summary>
    /// 高
    /// </summary>
    public double h { get; set; }

    public ReplaceAndInsertImage(Image img, double w, double h)
    {
        this.img = img;
        this.w = w;
        this.h = h;
    }

    public ReplaceAction Replacing(ReplacingArgs e)
    {
        //获取当前节点
        var node = e.MatchNode;
        //获取当前文档
        Document doc = node.Document as Document;
        DocumentBuilder builder = new DocumentBuilder(doc);
        //将光标移动到指定节点
        builder.MoveTo(node);
        //插入图片
        builder.InsertImage(img, w, h);
        return ReplaceAction.Replace;
    }

}


/// <summary>
/// 插入特殊符号
/// </summary>
public class ReplaceAndInsertSpecialSymbol: IReplacingCallback
{
    /// <summary>
    /// 字体
    /// </summary>
    public string font { get; set; }

    /// <summary>
    /// 需要插入的特殊字符编码
    /// </summary>
    public int specialSymbolIndex { get; set; }

    public ReplaceAndInsertSpecialSymbol(string font, int specialSymbolIndex)
    {
        this.font = font;
        this.specialSymbolIndex = specialSymbolIndex;
    }

    public ReplaceAction Replacing(ReplacingArgs e)
    {
        //获取当前节点
        var node = e.MatchNode;
        //获取当前文档
        Document doc = node.Document as Document;
        DocumentBuilder builder = new DocumentBuilder(doc);
        //将光标移动到指定节点
        builder.MoveTo(node);
        builder.Font.Name = font;
        //插入特殊字符
        builder.Write(char.ConvertFromUtf32(specialSymbolIndex));
        return ReplaceAction.Replace;
    }

}
