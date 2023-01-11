using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

public enum UIPageId
{
    LoadingPage,
    MainPage,
    DensityExperimentPage
}

public class UIController : preProject.Singleton<UIController>
{
    private static SpriteAtlas SpriteAtlas;

    private static SpriteAtlas CarIconAtlas;
    private static Canvas uiCanvas;
    
    [SerializeField] private UIBasePage LoadingPage;
    [SerializeField] private UIBasePage MainPage;
    [SerializeField] private UIBasePage ExperimentPage;

    [SerializeField] private SpriteAtlas spriteAtlas;
    //[SerializeField] private UIDefine uiDefine;
    //public static UIDefine UIDefine { get; set; }

    private static Dictionary<UIPageId, UIBasePage> pages { get; } = new();

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        uiCanvas = GetComponent<Canvas>();

        PagesInit();

        SpriteAtlas = spriteAtlas;
        //UIDefine = uiDefine;
    }

    void PagesInit()
    {
        if (LoadingPage)
            pages.TryAdd(UIPageId.LoadingPage, Instantiate(LoadingPage, transform));
        if (MainPage)
            pages.TryAdd(UIPageId.MainPage, Instantiate(MainPage, transform));
        if (ExperimentPage)
            pages.TryAdd(UIPageId.DensityExperimentPage, Instantiate(ExperimentPage, transform));
        
        foreach (var page in pages)
        {
            if (page.Value != null)
            {
                page.Value.gameObject.SetActive(false);
            }
        }
    }

    public void PagesInitAfterStateStart()
    {
        foreach (var page in pages)
        {
            if (page.Value != null)
            {
                page.Value.gameObject.SetActive(false);
            }
        }
    }

    public static UIBasePage Open(UIPageId id)
    {
        if (pages.TryGetValue(id, out var page))
        {

            if (page.gameObject != null) 
                page.gameObject.SetActive(true);
            page.OnOpen();
            return page;
        }

        return null;
    }

    public static void Close(UIPageId id)
    {
        if (pages.TryGetValue(id, out var page))
        {
            page.gameObject.SetActive(false);
            page.OnClose();
        }
    }

    public static bool IsOpened(UIPageId id)
    {
        return pages.TryGetValue(id, out var page) && page.gameObject.activeSelf;
    }

    public static Sprite GetUISprite(string name)
    {
        return string.IsNullOrEmpty(name) ? null : SpriteAtlas.GetSprite(name);
    }

    public static Sprite GetCarIcon(string name)
    {
        return string.IsNullOrEmpty(name) ? null : CarIconAtlas.GetSprite(name);
    }

    public static Sprite GetLevelScreenShot(int level)
    {
        var path = Application.persistentDataPath + "/ScreenShot" + level + ".png";

        if (File.Exists(path))
        {
            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }

        return null;
    }

    public static float GetUIScaleFactor()
    {
        return uiCanvas.scaleFactor;
    }

    //切换场景前回收gobj 清除引用 MissingReferenceException
    public static void Clear()
    {
        pages.Clear();
    }
}