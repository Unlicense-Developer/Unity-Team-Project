using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KoreanTyper; // 필요한 경우 이 부분을 에필로그에 맞게 조정하세요.
using UnityEngine.SceneManagement;

public class Epilogue : MonoBehaviour
{
    public TextMeshProUGUI[] epilogueTexts; // 에필로그 텍스트 배열
    public Image[] epilogueImages; // 에필로그 이미지 배열
    private string[] originalTexts; // 원본 텍스트를 저장하는 배열
    private int currentTextIndex = 0; // 현재 표시 중인 텍스트 인덱스
    private bool isEpilogueStarted = false;
    private bool isTyping = false; // 타이핑 중인지 추적하는 변수
    private bool isTypingComplete = false; // 타이핑 완료 여부 추적 변수

    public GameObject baseUI; // 기본 UI

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
                originalTexts[i] = epilogueTexts[i].text; // 원본 텍스트 저장
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
            currentText.text = originalText; // 현재 텍스트를 완전히 표시
            StopAllCoroutines(); // 모든 코루틴 중지
            isTyping = false; // 타이핑 상태 해제
            isTypingComplete = true; // 타이핑 완료 상태로 설정
        }
    }

    private void ShowCurrentTextAndImage()
    {
        if (currentTextIndex < epilogueTexts.Length)
        {
            if (epilogueTexts[currentTextIndex] != null)
                epilogueTexts[currentTextIndex].gameObject.SetActive(true); // 현재 텍스트 활성화
            if (epilogueImages[currentTextIndex] != null)
                epilogueImages[currentTextIndex].gameObject.SetActive(true); // 현재 이미지 활성화

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
        yield return new WaitForSeconds(5); // 5초간 대기
        SceneManager.LoadScene("Title"); // 에필로그 종료 후 타이틀 씬으로 전환
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