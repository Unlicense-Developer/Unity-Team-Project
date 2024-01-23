using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KoreanTyper; // �ʿ��� ��� �� �κ��� ���ʷα׿� �°� �����ϼ���.
using UnityEngine.SceneManagement;

public class Epilogue : MonoBehaviour
{
    public TextMeshProUGUI[] epilogueTexts; // ���ʷα� �ؽ�Ʈ �迭
    public Image[] epilogueImages; // ���ʷα� �̹��� �迭
    private string[] originalTexts; // ���� �ؽ�Ʈ�� �����ϴ� �迭
    private int currentTextIndex = 0; // ���� ǥ�� ���� �ؽ�Ʈ �ε���
    private bool isEpilogueStarted = false;
    private bool isTyping = false; // Ÿ���� ������ �����ϴ� ����
    private bool isTypingComplete = false; // Ÿ���� �Ϸ� ���� ���� ����

    public GameObject baseUI; // �⺻ UI

    void Start()
    {
        InitializeEpilogueTexts();
        InitializeEpilogueImages();
    }

    void Update()
    {
        if (isEpilogueStarted && currentTextIndex < epilogueTexts.Length && Input.anyKeyDown)
        {
            if (isTyping)
            {
                CompleteCurrentText();
            }
            else if (isTypingComplete && !isTyping)
            {
                NextText();
            }
        }
    }

    private void InitializeEpilogueTexts()
    {
        originalTexts = new string[epilogueTexts.Length];
        for (int i = 0; i < epilogueTexts.Length; i++)
        {
            if (epilogueTexts[i] != null)
            {
                originalTexts[i] = epilogueTexts[i].text; // ���� �ؽ�Ʈ ����
                epilogueTexts[i].gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Epilogue text is null in epilogueTexts array");
            }
        }
    }

    private void InitializeEpilogueImages()
    {
        foreach (var image in epilogueImages)
        {
            if (image != null)
                image.gameObject.SetActive(false);
            else
                Debug.LogError("Epilogue image is null in epilogueImages array");
        }
    }

    public void EpilogueStart()
    {
        baseUI.SetActive(false);

        if (!isEpilogueStarted)
        {
            isEpilogueStarted = true;
            if (currentTextIndex == 0)
            {
                ShowCurrentTextAndImage();
            }
        }
    }
    private void CompleteCurrentText()
    {
        if (currentTextIndex < epilogueTexts.Length && epilogueTexts[currentTextIndex] != null)
        {
            TextMeshProUGUI currentText = epilogueTexts[currentTextIndex];
            string originalText = originalTexts[currentTextIndex];
            currentText.text = originalText; // ���� �ؽ�Ʈ�� ������ ǥ��
            StopAllCoroutines(); // ��� �ڷ�ƾ ����
            isTyping = false; // Ÿ���� ���� ����
            isTypingComplete = true; // Ÿ���� �Ϸ� ���·� ����
        }
    }

    private void ShowCurrentTextAndImage()
    {
        if (currentTextIndex < epilogueTexts.Length)
        {
            if (epilogueTexts[currentTextIndex] != null)
                epilogueTexts[currentTextIndex].gameObject.SetActive(true); // ���� �ؽ�Ʈ Ȱ��ȭ
            if (epilogueImages[currentTextIndex] != null)
                epilogueImages[currentTextIndex].gameObject.SetActive(true); // ���� �̹��� Ȱ��ȭ

            StartCoroutine(TypingRoutine());
        }
    }

    IEnumerator TypingRoutine()
    {
        if (currentTextIndex < epilogueTexts.Length && epilogueTexts[currentTextIndex] != null)
        {
            TextMeshProUGUI currentText = epilogueTexts[currentTextIndex];
            string originalText = originalTexts[currentTextIndex];

            isTyping = true;
            isTypingComplete = false;
            int typingLength = originalText.GetTypingLength();
            for (int i = 0; i <= typingLength; i++)
            {
                currentText.text = originalText.Typing(i);
                yield return new WaitForSeconds(0.05f);
            }
            isTyping = false;
            isTypingComplete = true;
        }
    }
    private void NextText()
    {
        if (currentTextIndex < epilogueTexts.Length - 1 && isTypingComplete && !isTyping)
        {
            DeactivateCurrentTextAndImage();

            currentTextIndex++;

            ActivateNextTextAndImage();

            StartCoroutine(TypingRoutine());
        }
        else if (currentTextIndex == epilogueTexts.Length - 1 && isTypingComplete)
        {
            StartCoroutine(WaitAndLoadTitleScene());
        }
    }

    IEnumerator WaitAndLoadTitleScene()
    {
        yield return new WaitForSeconds(5); // 5�ʰ� ���
        SceneManager.LoadScene("Title"); // ���ʷα� ���� �� Ÿ��Ʋ ������ ��ȯ
    }

    private void DeactivateCurrentTextAndImage()
    {
        if (epilogueTexts[currentTextIndex] != null)
            epilogueTexts[currentTextIndex].gameObject.SetActive(false);
        if (epilogueImages[currentTextIndex] != null)
            epilogueImages[currentTextIndex].gameObject.SetActive(false);
    }

    private void ActivateNextTextAndImage()
    {
        if (epilogueTexts[currentTextIndex] != null)
            epilogueTexts[currentTextIndex].gameObject.SetActive(true);
        if (epilogueImages[currentTextIndex] != null)
            epilogueImages[currentTextIndex].gameObject.SetActive(true);
    }
}