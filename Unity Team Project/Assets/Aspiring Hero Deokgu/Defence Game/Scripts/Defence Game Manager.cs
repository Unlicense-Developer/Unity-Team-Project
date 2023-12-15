using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DefenceGameManager : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;
    public Text gameOverScore;
    public Text scoreGoldText;

    int life = 5;
    public bool isPlaying = true;
    public GameObject goal;
    public PlayerLife playerLife;
    public GameObject gameOverUI;
    public GameObject gameStartUI;

    //½Ì±ÛÅæ
    public static DefenceGameManager instance = null;
    public static DefenceGameManager Instance
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
        if (instance == null)
        {
            instance = this;

            // ¾À ÀüÈ¯µÇ´õ¶óµµ ÆÄ±«µÇÁö ¾Ê°Ô ÇÔ
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerLife = GetComponent<PlayerLife>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();

        CheckGameOver();
    }

    void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }

    public int GetLifeCount()
    {
        return life;
    }

    public void CalculateLife(int value)
    {
        life += value;
    }

    void CheckGameOver()
    {
        if (life <= 0)
        {
            isPlaying = false;
            Cursor.lockState = CursorLockMode.None;
            gameOverScore.text = "´Þ¼º Á¡¼ö : " + score.ToString();
            scoreGoldText.text = "È¹µæ °ñµå : " + (score / 2).ToString();
            gameOverUI.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    public void ReturnWorldScene()
    {
        PlayerData.instance.AddGold(score / 2);
        PlayerData.instance.AddItemData("Ax");
        SceneManager.LoadScene("WorldMap");
    }
}
