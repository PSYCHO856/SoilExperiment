﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager: APP.Core.MonoSingleton<AudioManager> {
    private bool isOut;
    private bool isIn;

     protected override void Awake() {
        base.Awake();
    }

    //audioClipl列表
    public List < AudioClip > audioList=new List<AudioClip>();
    //初始声音预设数量
    private int initAudioPrefabcount = 2;
    //记录静音前的音量大小
    [HideInInspector]
    public float tempVolume = 0;
    //是否静音
    private bool isMute = false;
    public bool IsMute {
        set {
            isMute = value;
            if (isMute) {
                tempVolume = AudioListener.volume;
                AudioListener.volume = 0;
            } else {
                AudioListener.volume = tempVolume;
            }
        }
        private get {
            return isMute;
        }
    }
    //声音大小系数
    private float volumeScale = 1;
    public float VolumeScale {
        set {
            volumeScale = Mathf.Clamp01(value);
            if (!IsMute) {
                AudioListener.volume = value;
            }
        }
        private get {
            return volumeScale;
        }
    }

    //audio字典
    private Dictionary < string, AudioClip > audioDic = new Dictionary < string, AudioClip > ();

    //背景音乐
    private AudioSource bgAudioSource;


    //声音对象池
    private AudioObjectPool audioObjectPool;

    private void Start() {
        //初始化话对象池
        // GameObject audioPrefab = new GameObject("AudioObjectPool");
        // audioPrefab.AddComponent < AudioSource > ();
        // audioPrefab.GetComponent < AudioSource > ().playOnAwake = false;
        // audioObjectPool = new AudioObjectPool(audioPrefab, initAudioPrefabcount);
        // audioPrefab.hideFlags = HideFlags.HideInHierarchy;
        // audioPrefab.transform.SetParent(this.transform);
        if (audioList.Count <= 0)
        {
            return;
        }
        foreach(AudioClip ac in audioList) {
            audioDic.Add(ac.name, ac);
        }
        bgAudioSource = this.transform.GetChild(0).GetComponent < AudioSource > ();
    }


    private void Update() {
        //测试代码
        if (Input.GetMouseButtonDown(0)) {
            //StartCoroutine(AudioFadeIn(0, "沈木-Sold Out(热播版)", true)); //音乐渐入
            //PlayBGMAudio(EAudio.bgm_login_01);//播放背景音乐
            // PlayAudio(0, EAudio.bgm_login_01);
        }
        if (Input.GetMouseButtonDown(1)) {
            //StartCoroutine(AudioFadedOut(0));        //音乐淡出

        }
    }

    /// <summary>
    /// 音频播放
    /// </summary>
    /// <param name="index">播放器序号（用第几个AudioSource播放）</param>
    public void PauseAudio(int index) {
        AudioSource audioSource = this.transform.GetChild(index).GetComponent < AudioSource > ();
        audioSource.Pause();
    }

    /// <summary>
    /// 继续播放
    /// </summary>
    /// <param name="index">播放器序号</param>
    public void ResumeAudio(int index) {
        AudioSource audioSource = this.transform.GetChild(index).GetComponent < AudioSource > ();
        audioSource.UnPause();
    }

    /// <summary>
    /// 0:默认背景音乐播放 1播放对应音效
    /// </summary>
    /// <param name="index">播放器序号</param>
    public void StopAudio(int index) {
        AudioSource audioSource = this.transform.GetChild(index).GetComponent < AudioSource > ();
        audioSource.Stop();
    }

    /// <summary>
    /// 播放背景音乐，这里直接固定0号播放器用来播放背景音乐
    /// </summary>
    /// <param name="audioNme"></param>
    public void PlayBGMAudio(string audioNme) {
        AudioClip audioClip;
        if (audioDic.TryGetValue(audioNme, out audioClip)) {
            bgAudioSource.gameObject.SetActive(true);
            bgAudioSource.clip = audioClip;
            this.transform.GetChild(0).gameObject.SetActive(true);
            bgAudioSource.Play();
            bgAudioSource.loop = true;
        }
    }
    public void RefreshVolume(){
        var audios=this.transform.GetComponentsInChildren<AudioSource>();
        // audios[0].volume=PersistentDataMgr.Instance.PlayerData.set_bgm;
        // audios[1].volume=PersistentDataMgr.Instance.PlayerData.set_se;  
    }
    //0-bgm 1-se
    public void SetVolume(float value,int index){
        var s=this.transform.GetChild(index).GetComponent < AudioSource > ();
        s.volume=value;
    }
    /// <summary>
    /// 直接播放声音
    /// </summary>
    /// <param name="index">播放器序号</param>
    /// <param name="audioName">音频文件名称</param>
    /// <param name="volume">音量大小</param>
    /// <param name="isLoop">是否循环</param>
    public void PlayAudio(int index, string audioName, float volume = 1, bool isLoop = false) {
        //Debug.Log("------------执行播放声音----------------");
        AudioSource audioSource = this.transform.GetChild(index).GetComponent < AudioSource > ();

        if (IsMute || audioSource == null) {
            return;
        }
        StopAllCoroutines();
        AudioClip audioClip;
        if (audioDic.TryGetValue(audioName, out audioClip)) {
            //Debug.Log("按钮点击的clip名字是："+audioClip.name);
            audioSource.gameObject.SetActive(true);
            audioSource.loop = isLoop;
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }

    /// <summary>
    /// 0:默认背景音乐播放 1播放对应音效
    /// </summary>
    /// <param name="index">播放器序号</param>
    /// <param name="audioName">音频的名称</param>
    public void PlayAudio(int index, string audioName) {
        //Debug.Log("------------执行播放声音----------------");
        AudioSource audioSource = this.transform.GetChild(index).GetComponent < AudioSource > ();

        if (IsMute || audioSource == null) {
            return;
        }
        StopAllCoroutines();
        AudioClip audioClip;
        audioSource.gameObject.SetActive(true);
        if (audioDic.TryGetValue(audioName, out audioClip)) {
            //Debug.Log("按钮点击的clip名字是："+audioClip.name);
            audioSource.clip = audioClip;
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }
            audioSource.Play();
        }else{
             audioSource.clip = Resources.Load<AudioClip>(ResPath.audio_scene+audioName);
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }

    /// <summary>
    /// 声音淡入
    /// </summary>
    /// <param name="index">播放器序号</param>
    /// <param name="audioName">音频名称</param>
    /// <param name="isLoop">是否循环</param>
    /// <returns></returns>
    public IEnumerator AudioFadeIn(int index, string audioName, bool isLoop) {
        float initVolume = 0;
        float preTime = 1.0f / 5; //渐变率
        AudioClip audioClip;
        if (audioDic.TryGetValue(audioName, out audioClip)) {
            AudioSource audioSource = this.transform.GetChild(index).GetComponent < AudioSource > ();
            if (audioSource == null) yield
            break;
            audioSource.gameObject.SetActive(true); //声音播放对象默认为false，播放时把对应的对象设为true
            audioSource.clip = audioClip;
            audioSource.volume = 0;
            audioSource.loop = isLoop;
            print("zhi 0");
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }
            audioSource.Play();
            audioSource.volume = 0;

            while (true) {
                initVolume += 1 * Time.deltaTime * preTime; //声音渐高
                audioSource.volume = initVolume; //将渐高变量赋值给播放器音量
                if (initVolume >= 1 - 0.02f) //如果很接近配置文件中的值，那么将其固定在配置文件中的值（最大值）
                {
                    audioSource.volume = 1;
                    break;
                }
                yield return 1;
            }
        }
    }

    /// <summary>
    /// 声音淡出
    /// </summary>
    /// <param name="index">淡出的播放器序号</param>
    /// <returns></returns>
    public IEnumerator AudioFadedOut(int index) {
        AudioSource audioSource = this.transform.GetChild(index).GetComponent < AudioSource > ();

        if (audioSource == null || audioSource.volume == 0 || audioSource == null) {
            yield
            break;
        }
        float initVolume = audioSource.volume;
        float preTime = 1.0f / 5;
        while (true) {
            initVolume += -1 * Time.deltaTime * preTime;
            audioSource.volume = initVolume;
            if (initVolume <= 0) {
                audioSource.volume = 0;
                audioSource.Stop();
                break;
            }
            yield
            return 1;
        }
    }

    /// <summary>
    /// 初始化音频 
    /// </summary>
    /// <param name="audioSource"></param>
    private void InitAudioSource(AudioSource audioSource) {
        if (audioSource == null) {
            return;
        }
        if (audioSource.isPlaying) {
            audioSource.Stop();
        }
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = 1;
        audioSource.clip = null;
        audioSource.name = "AudioObjectPool";
    }

    private void Destroy() {
        StopAllCoroutines();
    }
}



# region 声音对象池
/*声音对象池，待完善，可能存在同一时间多种声音源在播放，硬切或者播放完毕再切，无法判定是那种，无法准确释放AudioObjectPool
 这里只是在开局使用对象池生成了指定个数的播放器，没有用到获取和释放播放器对象*/
public class AudioObjectPool {

    //要生成的对象池预设
    private GameObject prefab;
    //对象池列表
    private List < GameObject > pool;
    //构造函数
    public AudioObjectPool(GameObject prefab, int initialSize) {
        this.prefab = prefab;
        this.pool = new List < GameObject > ();
        for (int i = 0; i < initialSize; i++) {
            //进行初始化
            AllLocateInstance();
        }
    }
    //获取实例 播放对应音效
    public GameObject GetInstance() {
        if (pool.Count == 0) {
            //创建实例
        }
        GameObject _item = pool[0];
        pool.RemoveAt(0);
        _item.SetActive(true);
        return _item;
    }
    //释放实力
    public void ReleaseInstance(GameObject _item) {
        _item.SetActive(false);
        pool.Add(_item);
    }
    //生成本地实力
    private GameObject AllLocateInstance() {
        GameObject _item = (GameObject) GameObject.Instantiate(prefab);
        _item.transform.SetParent(AudioManager.Instance.transform);
        _item.SetActive(false);
        pool.Add(_item);
        return _item;
    }
}
#endregion