using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BossDefeated : MonoBehaviour
{
    public Image blackoutImage;
    public Epilogue epilogueScript;
    private bool epilogueStarted = false; // 에필로그가 시작되었는지 추적하는 변수

    public BossActive bossActive;

    void Start()
    {
        if (blackoutImage != null)
        {
            blackoutImage.color = new Color(0, 0, 0, 0);
        }
    }

    private void Update()
    {
        if (bossActive.bossActive == true && !bossActive.stendBoss.activeSelf)
        {
            OnBossDefeated();
        }
    }

    public void OnBossDefeated()
    {
        if (!epilogueStarted) // 에필로그가 아직 시작되지 않았다면
        {
            StartCoroutine(BossDefeatSequence());
            epilogueStarted = true; // 에필로그 시작 표시
        }
    }

    IEnumerator BossDefeatSequence()
    {
        yield return new WaitForSeconds(2f);
        FadeOutEffect();
        yield return new WaitForSeconds(2f);
        FadeInEffect();
        epilogueScript.EpilogueStart();
        Debug.Log("에필로그 시작");
    }

    void FadeOutEffect()
    {
        if (blackoutImage != null)
        {
            blackoutImage.DOFade(1f, 1f);
        }
    }

    // 화면 암전 효과 해제
    void FadeInEffect()
    {
        if (blackoutImage != null)
        {
            blackoutImage.DOFade(0f, 1f); // 1초 동안 화면을 밝게 만듦
        }
    }
}