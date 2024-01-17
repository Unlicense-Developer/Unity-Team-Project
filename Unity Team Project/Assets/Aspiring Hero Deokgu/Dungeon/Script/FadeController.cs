using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeController : MonoBehaviour
{
    public static FadeController instance;
    public Image fadePanel; // ���̵� ȿ���� ������ �г�

    private float fadeDuration = 1.0f; // ���̵� ��/�ƿ��� �ɸ��� �ð�

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0f);
    }

    public void FadeIn()
    {
        fadePanel.DOFade(0, fadeDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                Debug.Log("FadeIn Complete");
            });
    }

    public void FadeOut()
    {
        fadePanel.DOFade(1, fadeDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                Debug.Log("FadeOut Complete");
            });
    }
}
