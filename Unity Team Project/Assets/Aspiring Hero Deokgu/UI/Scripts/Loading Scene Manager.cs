using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    private string nextScene;
    [SerializeField] GameObject loadingSceneUI; // 로딩 화면 UI
    [SerializeField] Image progressBar; // 진행률 표시줄

    public static LoadingSceneManager instance = null; // 싱글턴 인스턴스

    public static LoadingSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    void Awake()
    {
        // 싱글턴 패턴 구현
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 장면 전환시 파괴되지 않도록 설정
        }
        else if (instance != null)
        {
            Destroy(this.gameObject); // 중복 인스턴스 제거
        }
    }

    private void OnEnable()
    {
        // 장면이 로드될 때 호출될 메소드 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 새로운 장면 로드 시작
    public void StartLoadScene(string sceneName)
    {
        nextScene = sceneName;
        StartCoroutine(LoadScene());
    }

    // 장면 로드 완료시 호출될 메소드
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadingSceneUI.SetActive(false); // 로딩 화면 UI 비활성화
    }

    // 장면 로딩 코루틴
    IEnumerator LoadScene()
    {
        yield return null;
        loadingSceneUI.SetActive(true); // 로딩 화면 UI 활성화
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; // 즉시 장면 활성화 방지
        float timer = 0.0f;
        progressBar.fillAmount = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true; // 장면 활성화 허용
                    yield break;
                }
            }
        }
    }
}