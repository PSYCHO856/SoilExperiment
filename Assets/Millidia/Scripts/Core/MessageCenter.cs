using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageTagType
{
    SwampEffect
}


public delegate void DoSomething(params object[] parms);
/// <summary>
/// 全局消息类
/// </summary>
public class MessageCenter : APP.Core.Singleton<MessageCenter> {

    Dictionary<string, Dictionary<object, DoSomething>> messageCache = new Dictionary<string, Dictionary<object, DoSomething>>();

    public Dictionary<string, Dictionary<object, DoSomething>> MessageCache
    {
        get
        {
            return messageCache;
        }

        set
        {
            messageCache = value;
        }
    }

    internal void RegiseterMessage(string v, object onChangeWeapon)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 注册消息机制
    /// </summary>
    /// <param name="message">消息头</param>
    /// <param name="self">注册的对象</param>
    /// <param name="dosomething">注册干嘛事</param>
    public void RegiseterMessage(string message, object self,DoSomething dosomething)
    {
        if (!MessageCache.ContainsKey(message))
            messageCache.Add(message, new Dictionary<object, DoSomething>());
 
        if(!MessageCache[message].ContainsKey(self))
            MessageCache[message].Add(self, dosomething);

        if (MessageCache[message].ContainsKey(self))
            MessageCache[message][self] = dosomething;
    }

    /// <summary>
    /// 单发
    /// </summary>
    /// <param name="message"></param>
    /// <param name="self"></param>
    /// <param name="prams"></param>
    public void FrenchMessage(string message, object self,params object[] prams )
    {
        if (MessageCache.ContainsKey(message))
        {
            if(self == null)
                MessageCache[message].Remove(self);

            if (MessageCache[message].ContainsKey(self))
            {
                MessageCache[message][self](prams);
            }
        }
    }

    /// <summary>
    /// 广播 
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="prams"></param>
    public void BoradCastMessage(string message,params object[] prams)
    {
        List<KeyValuePair<object,DoSomething>> objects = new List<KeyValuePair<object, DoSomething>>();
        if (MessageCache.ContainsKey(message))
        {
            foreach(var dos in messageCache[message])
            {
                if(dos.Key != null)
                {
                    dos.Value(prams);
                }
                else
                {
                    objects.Add(dos);
                }
            }

            for(int i = 0; i < objects.Count; i++)
            {
                MessageCache.Remove(objects[i].Key as String);
            }
        }


    }

    /// <summary>
    /// 删除指定对象的注册委托
    /// </summary>
    /// <param name="message"></param>
    /// <param name="self"></param>
    public void RemoveMessageByTarget(string message, object self)
    {
        if (MessageCache.ContainsKey(message))
        {
            if (MessageCache[message].ContainsKey(self))
            {
                MessageCache[message].Remove(self);
            }
        }
    }

    /// <summary>
    /// 批量移除所有指定消息的逻辑
    /// </summary>
    /// <param name="message"></param>
    public void RemoveAllMessageByName(string message)
    {
        if (MessageCache.ContainsKey(message))
        {
            MessageCache.Remove(message);
        }
    }

}
