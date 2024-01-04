using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    private string nextScene;
    [SerializeField] GameObject loadingSceneUI; // �ε� ȭ�� UI
    [SerializeField] Image progressBar; // ����� ǥ����

    public static LoadingSceneManager instance = null; // �̱��� �ν��Ͻ�

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
        // �̱��� ���� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ��� ��ȯ�� �ı����� �ʵ��� ����
        }
        else if (instance != null)
        {
            Destroy(this.gameObject); // �ߺ� �ν��Ͻ� ����
        }
    }

    private void OnEnable()
    {
        // ����� �ε�� �� ȣ��� �޼ҵ� ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ���ο� ��� �ε� ����
    public void StartLoadScene(string sceneName)
    {
        nextScene = sceneName;
        StartCoroutine(LoadScene());
    }

    // ��� �ε� �Ϸ�� ȣ��� �޼ҵ�
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadingSceneUI.SetActive(false); // �ε� ȭ�� UI ��Ȱ��ȭ
    }

    // ��� �ε� �ڷ�ƾ
    IEnumerator LoadScene()
    {
        yield return null;
        loadingSceneUI.SetActive(true); // �ε� ȭ�� UI Ȱ��ȭ
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; // ��� ��� Ȱ��ȭ ����
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
                    op.allowSceneActivation = true; // ��� Ȱ��ȭ ���
                    yield break;
                }
            }
        }
    }
}