using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
using System;
using System.IO;
using System.Linq;
using UnityEngine.Networking;

using UnityEngine.Video;

public delegate void OnPercentEventHandler(float percent, string message);
public delegate void OnResourceDoneEventHandler();

[Serializable]
public class URLConfig
{
    public string abserver;
    public string gateserver;
    public int port;
    public string manifestName;
    public string abextension;
}

public static class PathMgr
{

    public static string PersistentDataPath
    {
        get
        {
            return Application.persistentDataPath;
        }
    }
    public static string DataPath
    {
        get
        {
            return Application.dataPath;
        }
    }

    public static string StreamingAsset
    {
        get
        {
            return Application.streamingAssetsPath;
        }
    }
}


  
public class ResourceMgr : APP.Core.MonoSingleton<ResourceMgr>
{
    [Header("锁ID")]
    public int lockID=80;

    private static string _appDataPath;
    public static string AppDataPath
    {
        get
        {
            if (string.IsNullOrEmpty(_appDataPath))
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    _appDataPath = Application.persistentDataPath + "/NimUnityVirtualSocial";
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    string androidPathName = "com.Army.KTV";
                    if (System.IO.Directory.Exists("/sdcard"))
                        _appDataPath = Path.Combine("/sdcard", androidPathName);
                    else
                        _appDataPath = Path.Combine(Application.persistentDataPath, androidPathName);
                }
                else if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    _appDataPath = "NimUnityVirtualSocial";
                }
                Debug.Log("AppDataPath:" + _appDataPath);
            }
            return _appDataPath;
        }
    }
    public event OnPercentEventHandler OnPercent;
    public event OnResourceDoneEventHandler OnDone;

    public static Dictionary<string, AssetBundle> LoadedCaches = new Dictionary<string, AssetBundle>();

    public bool usingXlua;

    /// <summary>
    /// 打包目录路径 打包换了这个路径也会换
    /// </summary>
    public string ext = "millidia/abassets/resources/";
    public URLConfig config;

    /// <summary>
    /// 配置文件名字
    /// </summary>
    private string localManifestPath;

    public static string extension = ".boynext";

    /// <summary>
    /// CurrentABM
    /// </summary>
    public AssetBundle ManifestAssentBundle;
    /// <summary>
    /// 资源服务器网址
    /// </summary>
    AssetBundleManifest abm;

    protected override void Initialize()
    {
        Debug.Log("REsourceMgr.Initialize");
        try
        {
            /*
            localManifestPath = PathMgr.PersistentDataPath + "/" + "manifest";
            string content = "";
            if (Application.platform == RuntimePlatform.Android)
            {
                string serverippath = Path.Combine(PathMgr.StreamingAsset, "serverIp.json");
                UnityWebRequest readserveripfile = new UnityWebRequest(serverippath);
                readserveripfile.downloadHandler = new DownloadHandlerBuffer();
                readserveripfile.SendWebRequest();
                while (!readserveripfile.isDone) { }

                //content = System.Text.ASCIIEncoding.UTF8.GetString(System.Text.ASCIIEncoding.Default.GetBytes(readserveripfile.downloadHandler.text));
                content = (readserveripfile.downloadHandler.text);
                Debug.Log("content=" + content);
            }
            else
            {
                content = File.ReadAllText(PathMgr.StreamingAsset + "/serverIp.json");
                Debug.Log("content=" + content);
            }

            //config = LitJson.JsonMapper.ToObject<URLConfig>(content);
            config = JsonUtility.FromJson<URLConfig>(content);
            extension = config.abextension;
            */
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
        base.Initialize();
    }

    /// <summary>
    /// 入口方法
    /// </summary>
    protected void Start()
    {

        ///场景加载结束 
        OnDone += LoadDone;

        if (OnPercent != null)
        {
            OnPercent(0f, "开始读取资源包");
        }
        if (!IsLoadLocal)
        {
            StartCoroutine(DownLoadManifest(config.abserver, config.manifestName));
        }
        else
        {
            if(usingXlua){
                HaspLock.Instance?.LoginHasp(lockID);
            }
            //注册配置文件
            ConfigInfo.Instance.ReadConfig();
            //预加载资源到内存
            PreLoadResList();
            //其他全局脚本
            SceneMgr.Instance.Init();
            GlobalPropsMgr.Instance.Init();
            if (OnPercent != null)
                OnPercent(1f, "正在读取本地配置");
            if (OnDone != null)
                OnDone();
        }
    }
    private void PreLoadResList(){
        // Debug.Log(resV.name);
    }
    private void LoadDone(){
        Debug.Log("预加载完毕");
    }

    private void CompareManifestData(DownloadHandler dhab, out AssetBundleManifest abm, out AssetBundleManifest newAbm, out AssetBundle checkManifest, out Dictionary<string, Hash128> hashs, out List<string> wantDownload)
    {
        Debug.Log("读取本地配置文件路径" + localManifestPath);
        ManifestAssentBundle = AssetBundle.LoadFromFile(localManifestPath);
        abm = ManifestAssentBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
        ///Can't load two same name asset bundles in memory so delete the old one when main manifest was loaded.
        ManifestAssentBundle.Unload(false);
        Debug.Log("本地有配置文件");
        checkManifest = AssetBundle.LoadFromMemory(dhab.data);
        newAbm = checkManifest.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
        hashs = new Dictionary<string, Hash128>();
        wantDownload = new List<string>();

        foreach (var data in abm.GetAllAssetBundles())
        {
            hashs.Add(data, abm.GetAssetBundleHash(data));
        }

        foreach (var data in newAbm.GetAllAssetBundles())
        {
            if (hashs.ContainsKey(data))
            {
                if (hashs[data] != newAbm.GetAssetBundleHash(data))
                {
                    Debug.Log("有变动的资源包" + data);
                    //keep remember
                    wantDownload.Add(data);
                }
            }
            else
            {
                wantDownload.Add(data);
            }
        }

    }



    /// <summary>
    /// 下载配置文件 基于配置文件批量下载资源包
    /// </summary>
    /// <param name="Url"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    IEnumerator DownLoadManifest(string Url, string assetName)
    {
        if (OnPercent != null)
            OnPercent(0.1f, "开始分析资源包");

        ///Unity下载功能 传入指定的下载路径 开启一个下载链接
        Debug.LogWarning(Url + assetName);
        UnityWebRequest ab = UnityWebRequest.Get(Url + assetName);
        yield return ab.SendWebRequest();
        if (ab.isDone && ab.error == null)
        {
            ///获取本次下载的资源包
            DownloadHandler dhab = ab.downloadHandler;

            if (File.Exists(localManifestPath))
            {
                AssetBundleManifest newAbm;
                AssetBundle checkManifest;
                Dictionary<string, Hash128> hashs;
                List<string> wantDownload;
                ///compare two manifest
                CompareManifestData(dhab, out abm, out newAbm, out checkManifest, out hashs, out wantDownload);
                ///rewrite manifest assetbundle 
                WriteFile("manifest", dhab.data);
                abm = newAbm;
                ManifestAssentBundle = checkManifest;

                int count = abm.GetAllAssetBundles().Length;
                int index = 0;



                foreach (var down in wantDownload)
                {
                    if (OnPercent != null)
                        OnPercent(0.9f * UtilityTool.CTFloat(index) / UtilityTool.CTFloat(count), "正在下载资源" + down);
                    Debug.Log("开始下载更新的资源包" + Url + "/" + down);
                    yield return StartCoroutine(DownLoadFileToLocal(Url + "/" + down, down, abm.GetAssetBundleHash(down)));
                    yield return new WaitForEndOfFrame();
                    index++;
                }

            }
            else
            {

                WriteFile("manifest", dhab.data);
                ManifestAssentBundle = AssetBundle.LoadFromFile(localManifestPath);
                abm = ManifestAssentBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");

                int count = abm.GetAllAssetBundles().Length;
                int index = 0;

                foreach (var down in abm.GetAllAssetBundles())
                {
                    if (OnPercent != null)
                        OnPercent(0.9f * UtilityTool.CTFloat(index) / UtilityTool.CTFloat(count), "正在下载资源" + down);
                    Debug.Log("开始下载更新的资源包" + Url + "/" + down);
                    yield return StartCoroutine(DownLoadFileToLocal(Url + "/" + down, down, abm.GetAssetBundleHash(down)));
                    yield return new WaitForEndOfFrame();
                    index++;
                }
            }


            //下载完所有AB包之后
            //指定AB包中的所有资源路径
            //加载配置文件
            //专场

            //LuaConfigInfo.Instance.LoadLuas();
            if (OnPercent != null)
                OnPercent(1f, "正在读取本地配置");

            ConfigInfo.Instance.ReadConfig();


            if (OnDone != null)
                OnDone();

        }
        else
        {
            Debug.LogError(ab.error);
        }
    }


    /// <summary>
    /// download ab to local 
    /// </summary>
    /// <param name="downloadUrl"></param>
    /// <param name="assetBundleName"></param>
    /// <param name="hashcode"></param>
    /// <returns></returns>
    IEnumerator DownLoadFileToLocal(string downloadUrl, string assetBundleName, Hash128 hashcode)
    {
        UnityWebRequest abrequest = UnityWebRequest.Get(downloadUrl);

        yield return abrequest.SendWebRequest();
        if (abrequest.isDone && abrequest.error == null)
        {
            DownloadHandler dh = abrequest.downloadHandler;
            Debug.Log(downloadUrl + "  " + assetBundleName + "  " + hashcode);
            WriteFile(assetBundleName, dh.data);
        }
        else
        {
            Debug.Log(abrequest.error);
        }
    }
    /// <summary>
    /// 是否本地加载
    /// </summary>
    public bool IsLoadLocal = true;


    static string GetAssetBundleFullPath(string localPath)
    {
        return PathMgr.PersistentDataPath + "/" + localPath.ToLower() + extension;
    }

    /// <summary>
    /// load assets from ab
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetPath">loaded path</param>
    /// <param name="unloadAB">release ab memory</param>
    /// <returns></returns>
    static T LoadAssetsFromAssetBundle<T>(string assetPath, bool unloadAB = false) where T : UnityEngine.Object
    {
        string path = assetPath.Remove(assetPath.LastIndexOf("/"));
        path = GetAssetBundleFullPath(path);
        AssetBundle ab = LoadAssetBundle(path);

        if (ab != null)
        {
            List<string> paths = ab.GetAllAssetNames().ToList().Where(x => x.Contains(assetPath.ToLower())).ToList();
            if (paths.Count >= 1)
            {
                T obj = ab.LoadAsset<T>(paths[0]);
                return obj;
            }
            if (unloadAB)
            {
                UnLoadAssetBundle(path);
            }
            return null;
        }
        else
        {
            return default(T);
        }
    }


    /// <summary>
    /// LoadAllAssetsFromAB
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetPath"></param>
    /// <param name="unloadAB"></param>
    /// <returns></returns>
    static T[] LoadAllAssetsFromAssetBundle<T>(string assetPath, bool unloadAB = false) where T : UnityEngine.Object
    {
        string path = assetPath.Remove(assetPath.LastIndexOf("/"));
        path = PathMgr.PersistentDataPath + "/" + path.ToLower() + extension;
        AssetBundle ab = LoadAssetBundle(path);
        if (ab != null)
        {
            T[] temp = ab.LoadAllAssets<T>();
            if (unloadAB)
            {
                UnLoadAssetBundle(path);
            }
            return temp;
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// Load T
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="assetPath">local path</param>
    /// <param name="unloadAB">when loaded release the ab memory or not</param>
    /// <returns>return then loaded asset</returns>
    public static T Load<T>(string assetPath, bool unloadAB = false) where T : UnityEngine.Object
    {
        if (ResourceMgr.Instance.IsLoadLocal == false)
        {
            string totalPath = ResourceMgr.Instance.ext + assetPath.Remove(assetPath.LastIndexOf("/")).ToLower();

            //ResourceMgr.Instance.abm.GetAllAssetBundles().ToList().ForEach(x => Debug.Log(x));
            string[] allDependenciesPaths = ResourceMgr.Instance.abm.GetAllDependencies(totalPath + extension);

            foreach (var data in allDependenciesPaths)
            {
                LoadAssetBundle(PathMgr.PersistentDataPath + "/" + data);
            }

            T result = LoadAssetsFromAssetBundle<T>(ResourceMgr.Instance.ext + assetPath.ToLower(), unloadAB);
            Debug.LogError(result);

            return result;
        }
        else
        {
            // Debug.Log("Now:"+assetPath);
            return Resources.Load<T>(assetPath);
        }
    }
      /// <summary>
    /// 异步加载2d图片
    /// </summary>
    public void Load2DImg(string path, Image img)
    {
        if (img == null)
        {
            return;
        }
        var t = UtilityTool.Instance.LoadAsync(path, (res) =>
        {
            Texture2D tex = res as Texture2D;
            if (tex == null)
            {
                return;
            }
            Sprite pic = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            if (img == null)
            {
                return;
            }
            img.sprite = pic;
        });
        StartCoroutine(t);

    }
    public void LoadPrefab(string path)
    {
        //异步方法
        var t = UtilityTool.Instance.LoadAsync(path, (res) =>
        {

        });
        StartCoroutine(t);
    }
    /// <summary>
    /// 加载AB包 记录至缓存以防重复加载AB包
    /// </summary>
    /// <param name="path"></param>
    public static AssetBundle LoadAssetBundle(string path)
    {
        if (!LoadedCaches.ContainsKey(path))
        {
            AssetBundle temp = AssetBundle.LoadFromFile(path);
            LoadedCaches.Add(path, temp);
            return temp;
        }
        else
        {
            return LoadedCaches[path];
        }
    }

    /// <summary>
    /// 卸载AB包同时清除缓存记录
    /// </summary>
    /// <param name="path"></param>
    public static void UnLoadAssetBundle(string path, bool flag = false)
    {
        if (LoadedCaches.ContainsKey(path))
        {
            LoadedCaches[path].Unload(flag);
            LoadedCaches.Remove(path);

        }
    }

    /// <summary>
    /// Load Object
    /// </summary>
    /// <param name="path"></param>
    /// <param name="unloadAB"></param>
    /// <returns></returns>
    public static UnityEngine.Object Load(string assetPath, bool unloadAB = false)
    {
        UnityEngine.Object result = Load<UnityEngine.Object>(assetPath, unloadAB);
        return result;

    }

    public static T[] LoadAll<T>(string path, bool unloadAB = false) where T : UnityEngine.Object
    {
        if (Instance.IsLoadLocal)
        {
            return Resources.LoadAll<T>(path);//使用Reousrce.Load加载
        } ///通过AB包加载资源
        else
        {
            return LoadAllAssetsFromAssetBundle<T>(path, unloadAB);
        }
    }

    public static UnityEngine.Object[] LoadAll(string path, bool unloadAB = false)
    {
        return LoadAll<UnityEngine.Object>(path, unloadAB);

    }
    /// <summary>
    /// 生成指定道具
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static GameObject CreateObj(string path)
    {
        var res = ResourceMgr.Load<GameObject>(path);
        if (res == null)
            return null;
        return GameObject.Instantiate<GameObject>(res);
    }

    public static GameObject CreateObj(GameObject go)
    {
        if (go == null)
            return null;
        return GameObject.Instantiate<GameObject>(go);
    }


    public static GameObject CreateObj(GameObject go, Vector3 pos, Vector3 dir)
    {
        if (go == null)
            return null;
        return GameObject.Instantiate<GameObject>(go, pos, Quaternion.LookRotation(dir));
    }


    static Dictionary<int, GameObject> monsterGoDic = new Dictionary<int, GameObject>();

    /// <summary>
    /// 创建UI对象到指定父类下面
    /// </summary>
    /// <param name="path"></param>
    /// <param name="target"></param>
    /// <param name="worldStay"></param>
    /// <returns></returns>
    public static GameObject CreateUIPrefab(string path, Transform target, bool worldStay = false)
    {
        var go = CreateObj(path);
        go.transform.SetParent(target, worldStay);
        return go;
    }

    /// <summary>
    /// 创建UI对象到指定父类下面
    /// </summary>
    /// <param name="path"></param>
    /// <param name="target"></param>
    /// <param name="worldStay"></param>
    /// <returns></returns>
    public static GameObject CreateUIPrefab(GameObject uiGo, Transform target, bool worldStay = false)
    {
        var go = CreateObj(uiGo);
        go.transform.SetParent(target, worldStay);
        return go;
    }

    /// <summary>
    /// 创建特效
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="target"></param>
    /// <param name="worldStay"></param>
    /// <returns></returns>
    public static GameObject CreateEffect(string effectName, Transform target = null, bool worldStay = false)
    {
        var go = CreateObj("Effects/" + effectName);
        if (go == null)
            return null;
        go.transform.SetParent(target, worldStay);
        return go;
    }

    public static GameObject CreateEffect(string effectName, Vector3 worldPos)
    {
        var go = CreateObj("Effects/" + effectName);
        if (go == null)
            return null;
        go.transform.position = worldPos;
        return go;
    }

    /// <summary>
    /// 加载指定图集中的图片
    /// </summary>
    /// <param name="index"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Sprite LoadSpriteFromAtals(int index, string path)
    {
        return ResourceMgr.LoadAll(path)[index] as Sprite;
    }


    public static Sprite LoadSprite(string path)
    {
        return ResourceMgr.Load<Sprite>(path);
    }

    public static AudioClip LoadAudioClip(string path)
    {
        return ResourceMgr.Load<AudioClip>(path);
    }

    public static TextAsset LoadAudioTextAsset(string path)
    {
        return ResourceMgr.Load<TextAsset>(path);
    }

    /// <summary>
    /// write File Data
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="infos"></param>
    /// <returns></returns>
    public bool WriteFile(string fileName, byte[] infos)
    {
        try
        {
            string realpath = PathMgr.PersistentDataPath + "/" + fileName;
            if (fileName.Contains("/"))
            {
                string dir = realpath.Remove(realpath.LastIndexOf("/"));
                Debug.Log(dir);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }

            BinaryWriter bw = new BinaryWriter(new FileStream(realpath, FileMode.OpenOrCreate, FileAccess.Write));
            for (int i = 0; i < infos.Length; i++)
            {
                bw.Write(infos[i]);
            }
            bw.Flush();
            bw.Close();
            bw.Dispose();
            Debug.Log("写入" + realpath);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("IO错误" + e);
            return false;
        }
    }

    /// <summary>
    /// Clear all caches
    /// </summary>
    public void ReleaseAllAssetBundles(bool removeCachePool = false)
    {
        AssetBundle.GetAllLoadedAssetBundles().ToList().ForEach(x => x.Unload(removeCachePool));
        LoadedCaches.Clear();
    }
   
}
