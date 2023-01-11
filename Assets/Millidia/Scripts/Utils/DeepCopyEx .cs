using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
/// <summary>
/// 深拷贝工具类
/// </summary>
public static class DeepCopyEx {
    /// <summary>
    /// 反射实现
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T DeepCopyByReflection < T > (T obj) {
        if (obj is string || obj.GetType().IsValueType)
            return obj;
        
        object retval = Activator.CreateInstance(obj.GetType());
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        foreach(var field in fields) {
            try {
                field.SetValue(retval, DeepCopyByReflection(field.GetValue(obj)));
            } catch {}
        }

        return (T) retval;
    }
    /// <summary>
    /// 使用二进制序列化和反序列化时，在需要序列化的类上要加上[Serializable]
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T DeepCopyByBinary < T > (T obj) {
        object retval;
        using(MemoryStream ms = new MemoryStream()) {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            retval = bf.Deserialize(ms);
            ms.Close();
        }
        return (T) retval;
    }
}