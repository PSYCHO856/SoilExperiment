using System;
using System.Collections.Generic;
using UnityEngine;
using Aladdin.HASP;


public class HaspLock : SingletonBase<HaspLock>
{	
    private Hasp Hasp;
    /// <summary>
    /// 功能ID
    /// </summary>
    public HaspFeature Feature = HaspFeature.Default;//HaspFeature.FromFeature(465)

    /// <summary>
    /// 模板
    /// </summary>
    public string Scope =
    "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
    "<haspscope/>";

    /// <summary>
    /// 开发商代码 
    /// </summary>
    public string VendorCode =
    "1frlTSz9jEbfgAzJ6ezs8ZcUscq/mZN4hDcKNlvqGAPj7nf8WmnDbHw3+Gfcs3BCN+43rwuAocvBE+Kz" +
    "fSlaY/O15CTIWeTwZ5SuZ7IDn+t/GlflCk9eH97O5YWZvfrECwGzLFb4sZBTvC7+7BhwT1BnEyvDmg1b" +
    "HN2FZFhHXqFKRIsmdYk2X8D49ebEb+y6FbIEBpfzlYlkqkcNeUBOugIh8ZkEUTs0OCF5TXj3Tp1SWO4a" +
    "0RZB2zcHwm6TS0/EiKhGfNiB7LJXvfG85bFACqmnCqwIJXieaASZRZB0td+reVQRHqnPD3gB7igyX3mP" +
    "i7PJGkX5I80F0Sf8+NGWg8FhdtpAyIqNLZaGHu760u6HUCEY7/8hfTCoepCUMwp+U2ED6fMOFh7n5wNU" +
    "t3yFZYJX6I3h61KmK0fMg/w9hsdWAgLpk7TNpBOT07RCVuwciQEHoHHraZUc6plYamk3Rsjk0AMUIuj3" +
    "5kDYCs56DEgBC0JRqX3BRRKcez3iLvRgaYJXQrhiwB5G+nl+0GhG63t80yyNXIqvRMHY1V0ehfahwn3+" +
    "IMIsY64vDMDLcjO9u3SzGBt2AxGfOw2WKNWnAdN8l9df89cr/aaC5RcTLrzH9yXLi80P6Afisvt4Ix8h" +
    "+j3b/Qj/KlLvjQ/6ImaigX9Us47H5UE19ycJEbTD2/B7Gny3FVYUZbEHFn9xr2teX5i4CcfHERXjmJXV" +
    "dR2TCb/NYvcnwc8N2ze+eAkMO2khcCJYPJ4kUpNjbvsPWnyT+PZVCEQZWvtNdfZlZ26WHjI+jwTMMlvs" +
    "8uycI1FOJnzZRPx4rFbSPYQvF3d0185W3t/AMCBPltWb8WnPkriEb2W2y5U29u91rEZa9bUXe6q3nNQ/" +
    "tlwiwuydvkZ4WWg3y6VStRgZglOqXmdGAzTWkOgsBSS+XucNrlrpjTn3oU9VUhIfv+ZR9BU0SVdXd+N9" +
    "0IVdmzTNWO4cqfp6KfwDkg==";

    /// <summary>
    /// 登录到某个功能，从而建立会话环境
    /// </summary>
    /// <param name="feature">功能ID</param>
    /// <param name="vendorCode">开发商代码</param>
    /// <returns></returns>
    public Hasp Login(HaspFeature feature, string vendorCode)
    {
        Hasp hasp = new Hasp(feature);
        HaspStatus status = hasp.Login(vendorCode);

        if (HaspStatus.StatusOk != status)
        {
            // Debug.LogError("登录失败");
            return null;
        }
        return hasp;
    }

    /// <summary>
    /// 根据预定义的搜索参数登录功能来建立会话
    /// </summary>
    /// <param name="feature">功能ID</param>
    /// <param name="scope">选择指定参数的模板，API 将在其中搜索所需数据</param>
    /// <param name="vendorCode">开发商代码</param>
    public Hasp LoginScope(HaspFeature feature, string scope, string vendorCode)
    {

        Hasp hasp = new Hasp(feature);
        HaspStatus status = hasp.Login(vendorCode, scope);
        if (HaspStatus.StatusOk != status)
        {
            Debug.LogError("登录失败");
            return null;
        }
        return hasp;
    }

    /// <summary>
    /// 从环境或会话中注销 hasp_logout 
    /// </summary>
    /// <param name="hasp"></param>
    public void LoginOut(Hasp hasp)
    {
        HaspStatus status = hasp.Logout();

        if (HaspStatus.StatusOk != status)
        {
            //handle error
        }
    }

    /// <summary>
    /// 获取时间
    /// </summary>
    /// <param name="hasp"></param>
    public void GetRtc(Hasp hasp)
    {
        DateTime time = DateTime.Now;
        HaspStatus status = hasp.GetRtc(ref time);
        if (HaspStatus.StatusOk != status)
        {
            //handle error
        }

    }

    /// <summary>
    /// 检索有关会话环境的信息
    /// </summary>
    /// <param name="hasp"></param>
    /// <returns></returns>
    public string GetSessionInfo(Hasp hasp)
    {
        string info = null;
        HaspStatus status = hasp.GetSessionInfo(Hasp.SessionInfo, ref info);

        if (HaspStatus.StatusOk != status)
        {
            //handle error
            return null;
        }
        return info;
    }

    /// <summary>
    /// (无需登录)根据可自定义的搜索参数检索有关系统组件的信息，并根据可自定义的格式进行显示。
    /// </summary>
    /// <param name="scope">选择指定参数的模板，API 将在其中搜索所需数据</param>
    /// <param name="vendorCode">开发商代码</param>
    public string GetInfo(string scope, string vendorCode)
    {
        string format =
   "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
   "<haspformat root=\"hasp_info\">" +
   "    <hasp>" +
   "        <attribute name=\"id\" />" +
   "        <attribute name=\"type\" />" +
   "        <feature>" +
   "            <attribute name=\"id\" />" +
   "        </feature>" +
   "    </hasp>" +
   "</haspformat>";
        string info = null;
        HaspStatus status = Hasp.GetInfo(scope, format, vendorCode, ref info);

        if (HaspStatus.StatusOk != status)
        {
            //handle error
            Debug.LogError("无锁");
            return null;
        }
        return info;
    }
    /// <summary>
    /// 检查锁是否存在
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="venderCode"></param>
    /// <returns></returns>
    public bool IsGetInfo(string scope = null, string venderCode = null)
    {
        scope = Scope;
        venderCode = VendorCode;
        if (GetInfo(scope, venderCode) == null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool LoginHasp(int id=80)
    {
        Debug.Log(id);
        if (!IsGetInfo())
        {
            LockPanel.Instance.Show("检测不到锁，即将退出");
            return false;
        }
        if (Hasp != null) return true;
        Hasp = Login(HaspFeature.FromFeature(id), VendorCode);
        if (Hasp == null)
        {
            Hasp = Login(HaspFeature.FromFeature(50), VendorCode);
            if (Hasp == null)
            {              
                LockPanel.Instance.Show("锁登陆失败，即将退出");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 检测是否有锁
    /// </summary>
    /// <returns></returns>
    public bool IsLogin()
    {
        if (!IsGetInfo() || GetSessionInfo(Hasp) == null)
        {
            LockPanel.Instance.Show("检测不到锁，即将退出");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    public void LoginOut()
    {
        if (GetSessionInfo(Hasp) != null)
            LoginOut(Hasp);
    }
}




