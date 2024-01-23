using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BossDefeated : MonoBehaviour
{
    public Image blackoutImage;
    public Epilogue epilogueScript;
    private bool epilogueStarted = false; // ���ʷαװ� ���۵Ǿ����� �����ϴ� ����

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
        if (!epilogueStarted) // ���ʷαװ� ���� ���۵��� �ʾҴٸ�
        {
            StartCoroutine(BossDefeatSequence());
            epilogueStarted = true; // ���ʷα� ���� ǥ��
        }
    }

    IEnumerator BossDefeatSequence()
    {
        yield return new WaitForSeconds(2f);
        FadeOutEffect();
        yield return new WaitForSeconds(2f);
        FadeInEffect();
        epilogueScript.EpilogueStart();
        Debug.Log("���ʷα� ����");
    }

    void FadeOutEffect()
    {
        if (blackoutImage != null)
        {
            blackoutImage.DOFade(1f, 1f);
        }
    }

    // ȭ�� ���� ȿ�� ����
    void FadeInEffect()
    {
        if (blackoutImage != null)
        {
            blackoutImage.DOFade(0f, 1f); // 1�� ���� ȭ���� ��� ����
        }
    }
}