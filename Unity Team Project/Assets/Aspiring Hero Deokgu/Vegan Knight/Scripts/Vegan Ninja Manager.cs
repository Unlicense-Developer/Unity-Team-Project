using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VeganNinjaManager : MonoBehaviour
{
    public static VeganNinjaManager Instance { get; private set; }

    [SerializeField] private Blade blade;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameOverScoreText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image awardImage;
    [SerializeField] private GameObject gameOverUI;

    public List<AudioClip> sliceSounds;
    AudioSource sliceSound;

    private int score;
    public int playerLife = 5;
    public bool isSliceFruit = false;

    public int Score => score;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start()
    {
        NewGame();
        sliceSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        scoreText.text = score.ToString();
    }

    private void NewGame()
    {
        Time.timeScale = 1f;

        ClearScene();

        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruits) {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs) {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int points)
    {
        sliceSound.PlayOneShot(sliceSounds[Random.Range(0, 3)]);

        score += points;
        scoreText.text = score.ToString();

        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }
    }

    public void Explode()
    {
        blade.enabled = false;
        spawner.enabled = false;

        StartCoroutine(ExplodeSequence());
    }

    void CheckScoreAward()
    {
        if( score >= 500)
        {
            awardImage.sprite = ItemDataManager.instance.GetItem("Apple").icon;
            PlayerData.instance.AddItemData("Apple");
        }
        else if (score >= 200)
        {
            awardImage.sprite = ItemDataManager.instance.GetItem("Watermelon").icon;
            PlayerData.instance.AddItemData("Watermelon");
        }
        else if (score >= 100)
        {
            awardImage.sprite = ItemDataManager.instance.GetItem("Avocado").icon;
            PlayerData.instance.AddItemData("Avocado");
        }
        else if (score >= 30)
        {
            awardImage.sprite = ItemDataManager.instance.GetItem("Grape").icon;
            PlayerData.instance.AddItemData("Grape");
        }
        else
        {
            awardImage.sprite = ItemDataManager.instance.GetItem("Orange").icon;
            PlayerData.instance.AddItemData("Orange");
        }
    }

    public void ReturnWorldScene()
    {
        SceneManager.LoadScene("WorldMap");
        Time.timeScale = 1.0f;
    }

    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        // Fade to white
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        CheckScoreAward();
        gameOverScoreText.text = "�޼� ���� : " + score.ToString();
        gameOverUI.SetActive(true);

        //NewGame();

        //elapsed = 0f;

        //// Fade back in
        //while (elapsed < duration)
        //{
        //    float t = Mathf.Clamp01(elapsed / duration);
        //    fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

        //    elapsed += Time.unscaledDeltaTime;

        //    yield return null;
        //}
    }

}
