using LTGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class FloatEx {

    public static Vector3 ToVector3(this float[] source) {
        if (source == null || source.Length != 3) {
            LTDebug.LogError("无效的传入参数");
            return Vector3.zero;
        }
        return new Vector3(source[0], source[1], source[2]);
    }
    /// <summary>
    /// 保留N位不四舍5入 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static float FloatRound(float source,int n) {
        var powerNum=(float)Math.Pow(10,n);
        int i = (int)( source* powerNum);
        float f = (float)(i * 1.0) / powerNum;
        return f;
    }
}