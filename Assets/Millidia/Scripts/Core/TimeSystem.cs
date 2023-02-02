﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeSystem : APP.Core.Singleton<TimeSystem> {

    /// <summary>
    /// 获取服务器时间数据
    /// </summary>
    public static DateTime ServerNow
    {
        get {
            return new DateTime((DateTime.Now.Ticks/10000 + offset) * 10000); //本地tick时间 + 差值实时获取服务器时间
        }
    }

    private static long server;
    /// <summary>
    /// 服务器时间
    /// </summary>
    public static long ServerTime
    {
        set
        {
            server = value;
            offset = server - DateTime.Now.Ticks / 10000;
            Debug.Log("差时" + offset);
        }
        get { return DateTime.Now.Ticks / 10000 + offset; }
    }

    /// <summary>
    /// 本地时间差
    /// </summary>
    public static long offset;

    /// <summary>
    /// 这个月有多少天
    /// </summary>
    /// <returns></returns>
    public static int DaysInMonth()
    {
        return DateTime.DaysInMonth(ServerNow.Year, ServerNow.Month);
    }
 
    /// <summary>
    /// 获取本月第几天
    /// </summary>
    public static int GetCurrentDay 
    {
        get
        {
            return ServerNow.Day;
        }
    
    }
}