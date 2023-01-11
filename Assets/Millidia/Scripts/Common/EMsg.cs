/// <summary>
/// 消息枚举
/// </summary>
public class EMsg {
    /*************引导消息************/
    public readonly static string Guide_Finish="Guide_Finish";
    public readonly static string Guide_Next="Guide_Next";

    public readonly static string Guide_Start = "Guide_Start";//开始任务
    public readonly static string Guide_Stop = "Guide_Stop";//停止任务-开启其他任务
    public readonly static string GuideBig_Emit = "GuideBig_Emit";//停止任务-开启其他任务

    /// <summary>
    /// 开始设置目标点坐标
    /// </summary>
    public readonly static string Nav_Start = "Nav_Start";
    /// <summary>
    /// 显隐Nav线条 
    /// </summary>
    public readonly static string Nav_IsActive = "Nav_IsActive";


    /****************UI消息*************/
    /*****************用户交互3D********/
    public readonly static string Player_IsMove="Player_IsMove";
    public readonly static string Player_tools="Player_tools";

}