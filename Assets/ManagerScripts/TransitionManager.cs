//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//namespace Transition
//{
//    public class TransitionManager : MonoBehaviour
//    {

//        private CanvasGroup fadeCanvasGroup;
//        private bool isFade;
//        public Rigidbody2D Player;
//        public string startSceneName = string.Empty;


//        private void OnEnable()
//        {
//            EventHandler.TransitionEvent += OnTransitionEvent;
//        }


//        private void OnDisable()
//        {
//            EventHandler.TransitionEvent -= OnTransitionEvent;
//        }

//        private void Start()
//        {
//            // 通过标签查找
//            GameObject fadeObject = GameObject.FindWithTag("FadeCanvas");
//            if (fadeObject != null)
//            {
//                fadeCanvasGroup = fadeObject.GetComponent<CanvasGroup>();
//            }
//            //StartCoroutine(LoadSceneSetActive(startSceneName));
//        }

//        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
//        {
//            if (!isFade)
//            {
//                StartCoroutine(Transition(sceneToGo, positionToGo));
//            }
//        }

//        /// <summary>
//        /// 场景切换
//        /// </summary>
//        /// <param name="sceneName">目标场景</param>
//        /// <param name="targetPosition">目标位置</param>
//        /// <returns></returns>
//        private IEnumerator Transition(string sceneName, Vector3 targetPosition)
//        {
//            EventHandler.CallBeforeSceneUnloadEvent();
//            yield return Fade(1);
//            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

//            yield return LoadSceneSetActive(sceneName);

//            //移动人物坐标
//            EventHandler.CallMoveToPosition(targetPosition);
//            yield return Fade(0);
//            EventHandler.CallAfterSceneLoadEvent();
//        }

//        /// <summary>
//        /// 加载场景并设置为激活
//        /// </summary>
//        /// <param name="sceneName">场景名</param>
//        /// <returns></returns>
//        private IEnumerator LoadSceneSetActive(string sceneName)
//        {
//            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

//            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

//            SceneManager.SetActiveScene(newScene);
//        }

//        /// <summary>
//        /// 淡入淡出场景
//        /// </summary>
//        /// <param name="targetAlpha">1是黑，0是透明</param>
//        /// <returns></returns>
//        private IEnumerator Fade(float targetAlpha)
//        {
//            isFade = true;

//            fadeCanvasGroup.blocksRaycasts = true;

//            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / 2f;

//            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
//            {
//                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
//                yield return null;
//            }

//            fadeCanvasGroup.blocksRaycasts = false;

//            isFade = false;
//        }

//        public void StartButton()
//        {

//            UIManager.Instance.StartButton();
//            StartCoroutine(LoadSceneSetActive("Level1Scene"));
//            Player.bodyType = RigidbodyType2D.Dynamic;
//        }
//    }
//}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [Header("关卡背景音乐设置")]
        [SerializeField] private List<AudioClip> levelBackgroundMusics; // 关卡背景音乐列表
        [SerializeField] private AudioSource backgroundMusicSource; // 背景音乐播放器
        [SerializeField] private float musicFadeDuration = 1.0f; // 音乐淡入淡出时间

        [Header("音量设置")]
        [SerializeField] private float musicVolume = 0.7f; // 音乐音量

        private CanvasGroup fadeCanvasGroup;
        private bool isFade;
        public Rigidbody2D Player;
        public string startSceneName = string.Empty;

        private string currentSceneName;

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnload;
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoad;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnload;
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoad;
        }

        private void Start()
        {
            // 通过标签查找
            GameObject fadeObject = GameObject.FindWithTag("FadeCanvas");
            if (fadeObject != null)
            {
                fadeCanvasGroup = fadeObject.GetComponent<CanvasGroup>();
            }

            // 初始化音频组件
            InitializeAudioComponents();

            // 获取当前场景并播放对应音乐
            currentSceneName = SceneManager.GetActiveScene().name;
            PlayLevelMusic(currentSceneName);
        }

        /// <summary>
        /// 初始化音频组件
        /// </summary>
        private void InitializeAudioComponents()
        {
            // 如果没有分配backgroundMusicSource，创建或获取一个
            if (backgroundMusicSource == null)
            {
                // 先尝试查找现有的背景音乐播放器
                GameObject musicObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
                if (musicObject != null)
                {
                    backgroundMusicSource = musicObject.GetComponent<AudioSource>();
                }

                // 如果还没找到，创建一个新的
                if (backgroundMusicSource == null)
                {
                    GameObject newMusicObject = new GameObject("BackgroundMusicPlayer");
                    backgroundMusicSource = newMusicObject.AddComponent<AudioSource>();
                    backgroundMusicSource.loop = true;
                    backgroundMusicSource.playOnAwake = false;
                    backgroundMusicSource.volume = musicVolume;
                    DontDestroyOnLoad(newMusicObject); // 场景切换时不销毁
                }
            }
        }

        /// <summary>
        /// 播放对应关卡的音乐
        /// </summary>
        /// <param name="sceneName">场景名</param>
        private void PlayLevelMusic(string sceneName)
        {
            if (backgroundMusicSource == null || levelBackgroundMusics.Count == 0)
                return;

            int levelIndex = GetLevelIndex(sceneName);

            // 检查索引是否有效
            if (levelIndex >= 0 && levelIndex < levelBackgroundMusics.Count)
            {
                AudioClip musicToPlay = levelBackgroundMusics[levelIndex];

                if (musicToPlay != null)
                {
                    StartCoroutine(SwitchBackgroundMusic(musicToPlay));
                }
            }
        }

        /// <summary>
        /// 根据场景名获取关卡索引
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <returns>关卡索引（0=Level1, 1=Level2, 2=Level3）</returns>
        private int GetLevelIndex(string sceneName)
        {
            if (sceneName.Contains("Level1") || sceneName.Contains("level1"))
                return 0;
            else if (sceneName.Contains("Level2") || sceneName.Contains("level2"))
                return 1;
            else if (sceneName.Contains("Level3") || sceneName.Contains("level3"))
                return 2;
            else if (sceneName.Contains("Menu") || sceneName.Contains("Start"))
                return -1; // 菜单场景，不播放音乐或播放特殊音乐

            return 0; // 默认当作Level1
        }

        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
            if (!isFade)
            {
                StartCoroutine(Transition(sceneToGo, positionToGo));
            }
        }

        /// <summary>
        /// 场景切换前执行
        /// </summary>
        private void OnBeforeSceneUnload()
        {
            // 淡出当前音乐
            if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
            {
                StartCoroutine(FadeMusicOut());
            }
        }

        /// <summary>
        /// 场景加载后执行
        /// </summary>
        private void OnAfterSceneLoad()
        {
            // 更新当前场景名
            currentSceneName = SceneManager.GetActiveScene().name;

            // 播放新关卡的音乐
            PlayLevelMusic(currentSceneName);
        }

        /// <summary>
        /// 场景切换协程
        /// </summary>
        /// <param name="sceneName">目标场景</param>
        /// <param name="targetPosition">目标位置</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName, Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneSetActive(sceneName);

            // 移动人物坐标
            EventHandler.CallMoveToPosition(targetPosition);
            yield return Fade(0);
            EventHandler.CallAfterSceneLoadEvent();
        }

        /// <summary>
        /// 加载场景并设置为激活
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            SceneManager.SetActiveScene(newScene);
        }

        /// <summary>
        /// 淡入淡出场景
        /// </summary>
        /// <param name="targetAlpha">1是黑，0是透明</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;

            fadeCanvasGroup.blocksRaycasts = true;

            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / 2f;

            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }

            fadeCanvasGroup.blocksRaycasts = false;

            isFade = false;
        }

        /// <summary>
        /// 切换背景音乐（带淡入淡出效果）
        /// </summary>
        /// <param name="newMusic">新音乐</param>
        /// <returns></returns>
        private IEnumerator SwitchBackgroundMusic(AudioClip newMusic)
        {
            // 如果正在播放相同的音乐，不重复播放
            if (backgroundMusicSource.clip == newMusic && backgroundMusicSource.isPlaying)
                yield break;

            // 如果当前正在播放音乐，先淡出
            if (backgroundMusicSource.isPlaying)
            {
                yield return StartCoroutine(FadeMusic(backgroundMusicSource.volume, 0f));
            }

            // 设置新音乐并播放
            backgroundMusicSource.clip = newMusic;
            backgroundMusicSource.Play();

            // 淡入新音乐
            yield return StartCoroutine(FadeMusic(0f, musicVolume));
        }

        /// <summary>
        /// 淡出音乐
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeMusicOut()
        {
            yield return StartCoroutine(FadeMusic(backgroundMusicSource.volume, 0f));
        }

        /// <summary>
        /// 淡入音乐
        /// </summary>
        private IEnumerator FadeMusicIn()
        {
            yield return StartCoroutine(FadeMusic(0f, musicVolume));
        }

        /// <summary>
        /// 音乐淡入淡出
        /// </summary>
        /// <param name="startVolume">起始音量</param>
        /// <param name="targetVolume">目标音量</param>
        /// <returns></returns>
        private IEnumerator FadeMusic(float startVolume, float targetVolume)
        {
            float elapsedTime = 0f;

            while (elapsedTime < musicFadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / musicFadeDuration;
                backgroundMusicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
                yield return null;
            }

            backgroundMusicSource.volume = targetVolume;
        }

        /// <summary>
        /// 设置背景音乐音量
        /// </summary>
        /// <param name="volume">音量（0-1）</param>
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            if (backgroundMusicSource != null)
            {
                backgroundMusicSource.volume = musicVolume;
            }
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopMusic()
        {
            if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Stop();
            }
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public void PauseMusic()
        {
            if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Pause();
            }
        }

        /// <summary>
        /// 继续播放背景音乐
        /// </summary>
        public void ResumeMusic()
        {
            if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Play();
            }
        }

        public void StartButton()
        {
            UIManager.Instance.StartButton();
            StartCoroutine(LoadSceneSetActive("Level1Scene"));
            Player.bodyType = RigidbodyType2D.Dynamic;

            // 延迟播放Level1音乐
            StartCoroutine(DelayedPlayLevel1Music(0.5f));
        }

        private IEnumerator DelayedPlayLevel1Music(float delay)
        {
            yield return new WaitForSeconds(delay);
            PlayLevelMusic("Level1Scene");
        }
    }
}