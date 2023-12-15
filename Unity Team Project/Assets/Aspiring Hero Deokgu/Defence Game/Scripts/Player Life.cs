using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    public Canvas uiCanvas;
    public Image lifePrefab;
    public Sprite lifeOn;
    public Sprite lifeOff;
    List<Image> playerLife;
    Vector2 lifePos = new Vector2(-357.0f, -190.0f);
    float xPosPreset = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayerLife();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreatePlayerLife()
    {
        playerLife = new List<Image>();

        for (int i = 0; i < DefenceGameManager.instance.GetLifeCount(); i++)
        {
            Image temp = Instantiate(lifePrefab);
            temp.transform.SetParent(uiCanvas.transform);
            temp.GetComponent<RectTransform>().anchoredPosition = lifePos + new Vector2(xPosPreset * i, 0);
            temp.GetComponent<RectTransform>().localScale = Vector3.one;
            playerLife.Add(temp);
        }
    }

    public void IncreaseLife()
    {
        playerLife[DefenceGameManager.instance.GetLifeCount()].sprite = lifeOn;
        DefenceGameManager.instance.CalculateLife(1);
    }

    public void DecreaseLife()
    {
        DefenceGameManager.instance.CalculateLife(-1);
        playerLife[DefenceGameManager.instance.GetLifeCount()].sprite = lifeOff;
    }
}
