using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 
using System;
  
public class PlayerDataMgr : APP.Core.Singleton<PlayerDataMgr>
{

    /// <summary>
    /// 账号Id
    /// </summary>
    private static int accountId;

    public static int AccountId 
    {
        get { return PlayerDataMgr.accountId; }
        set { PlayerDataMgr.accountId = value; }
    }

    ///// <summary>
    ///// 当前所角色登录的基本信息
    ///// </summary>
    //private static PlayerRoleInfo me;

    //public static PlayerRoleInfo Me
    //{
    //    get { return PlayerDataMgr.me; }
    //    set { PlayerDataMgr.me = value; }
    //}



    //private static SelectPlayerRes myInfo;

    //public static SelectPlayerRes MyInfo
    //{
    //    get { return PlayerDataMgr.myInfo; }
    //    set { PlayerDataMgr.myInfo = value; }
    //}


    ///// <summary>
    ///// 服务器反馈的角色注册信息
    ///// </summary>
    //private static List<PlayerInfo> info;

    //public static List<PlayerInfo> Info
    //{
    //    get { return PlayerDataMgr.info; }
    //    set { PlayerDataMgr.info = value; }
    //}


    /// <summary>
    /// 金币
    /// </summary>
    private static int gold;
    public static Action<int, int> OnGoldChanged;
    public static int Gold
    {
        get { return PlayerDataMgr.gold; }
        set
        {
            if (OnGoldChanged != null)
                OnGoldChanged(PlayerDataMgr.gold, value);
            PlayerDataMgr.gold = value;
        }
    }
    /// <summary>
    ///钻石
    /// </summary>
    private static int gem;
    public static Action<int, int> OnGemChanged;
    public static int Gem
    {
        get { return PlayerDataMgr.gem; }
        set
        {
            if (OnGemChanged != null)
                OnGemChanged(PlayerDataMgr.gem, value);
            PlayerDataMgr.gem = value;
        }
    }
    /// <summary>
    /// 血石
    /// </summary>
    private static int bloodStore;
    public static Action<int, int> OnBloodStoreChanged;
    public static int BloodStore
    {
        get { return PlayerDataMgr.bloodStore; }
        set
        {
            if (OnBloodStoreChanged != null)
                OnBloodStoreChanged(PlayerDataMgr.bloodStore, value);
            PlayerDataMgr.bloodStore = value;
        }
    }
    ///// <summary>
    ///// 签到的历史信息
    ///// </summary>
    //public static List<SignInInfo> signInHistory;

    ///// <summary>
    ///// 背包信息
    ///// </summary>
    //private static List<PropInfo> bagInfo;
    //public static Action OnBagChanged;
    //public static List<PropInfo> BagInfo
    //{
    //    get { return PlayerDataMgr.bagInfo; }
    //    set
    //    {
    //        if (OnBagChanged != null)
    //            OnBagChanged();
    //        PlayerDataMgr.bagInfo = value;
    //    }
    //}

}
