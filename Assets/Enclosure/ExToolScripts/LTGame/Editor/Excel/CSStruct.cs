using System;
using System.Collections.Generic;
using System.Text;

namespace LTGame
{
    class CSStruct
    {
        public List<CSFiled> fileds;
        public string className; // class名字
        public string nameSpace; // 命名空间
        public bool isConst = false;

        public const string IMPORT = @"using System.Collections.Generic;
using UnityEngine;
        ";
        public const string HEAD = "namespace {0}\n";
        public const string LEFT_C = "{";
        public const string right_C = "}";
        public const string CLASS_NAME = "public class {0}: ConfigBase\n";
        public const string CLASS_NAME_WITHIN_SUB = "    public class {0} : {1}\n";
        public const string CLASS_NAME_WITHIN_SUB2 = "    public class {0} : BaseConfig<{1}>\n";

        /*
         
         public int ID()
        {
            return id;
        }
         */
        /*
         public string GetLoadPath()
        {
            return CONFIG_PATH;
        }
         */

        public string GetTSClass()
        {
            var sb = new StringBuilder();
            sb.AppendLine("    export class config {");

            foreach (var filed in fileds)
            {
                if (filed.mtype == "skip" || filed.mtype == "") continue;
                sb.AppendLine("        " + filed.GetTSCode());
            }

            sb.AppendLine("    }");

            if (isConst)
            {
                sb.AppendLine("    export var data : {0}.config;".ReplaceAll("{0}", className));
            }
            else
            {
                sb.AppendLine("    export var data : {[key: number]: {0}.config};".ReplaceAll("{0}", className));
                sb.AppendLine("    export var dataList : {0}.config[];".ReplaceAll("{0}", className));
                sb.AppendLine("    export var lastData : {0}.config;".ReplaceAll("{0}", className));
            }
            sb.AppendLine("    export const path = \"res/config/{0}.json\";".ReplaceAll("{0}", className));
            sb.AppendLine("}");

            return sb.ToString();
        }
        //C#文本编辑
        public string GetString(string savePath)
        {
            var sb = new StringBuilder();
            // sb.AppendLine(IMPORT);//引入命名空间
            // sb.AppendFormat(HEAD, nameSpace);
            // sb.AppendLine(LEFT_C);

            // 生成数据class
            // sb.AppendLine("    [System.Serializable]");
            sb.AppendFormat(CLASS_NAME, className);
            sb.AppendLine("" + LEFT_C);
            foreach (var filed in fileds)
            {
                if (filed.mtype == "skip" || filed.mtype == "") continue;
                if (filed.name == "Id")continue;//跳过Id字段命名
                sb.AppendLine("        " + filed.ToString());
            }
            sb.AppendLine("" + right_C);

            // sb.AppendLine(right_C);

            return sb.ToString();
        }
    }

    class CSFiled
    {
        public string name; // 字段名字
        public string region; // 字段属性
        public string mtype; // 字段类型
        public int index; // 字段索引
        public bool IsTs; // 字段索引

      
        public const string STR = @"/// <summary>
        /// {2}
        /// </summary>
        public {0} {1};";
        public const string TS_FORMAT = "/** {0} */ readonly {1}: {2};";

        //TS
        public string GetTSCode()
        {
            var wrapType = mtype;
            if (IsTs)
            {
                if (wrapType == "int" || wrapType == "float")
                {
                    wrapType = "number";
                }
                else if (wrapType == "int[]" || wrapType == "float[]")
                {
                    wrapType = "number[]";
                }
            }
            else
            {
                if (wrapType == "number")
                {
                    wrapType = "int";
                }
                else if (wrapType == "number[]")
                {
                    wrapType = "int[]";
                }
            }
          
            return string.Format(TS_FORMAT, region, name, wrapType);
        }

        //C#
        public override string ToString()
        {
            var wrapType = mtype;
           
            if (IsTs)
            {
                if (wrapType == "int" || wrapType == "float")
                {
                    wrapType = "number";
                }
                else if (wrapType == "int[]" || wrapType == "float[]")
                {
                    wrapType = "number[]";
                }
            }
            else
            {
                if (wrapType == "number")
                {
                    wrapType = "int";
                }
                else if (wrapType == "number[]")
                {
                    wrapType = "int[]";
                }
            }
            return string.Format(STR, wrapType, name, region);
        }
    }

}
