using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using WindowsInput;

public class InventoryManager : MonoBehaviour
{
    public int gold;
    GameObject select_Item;

    [SerializeField] private TMP_Text sellPriceText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject select_Frame;
    [SerializeField] private GameObject invenSlotPrefab;

    List<Item> inven = new List<Item>();

    //�̱���
    public static InventoryManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        inven = PlayerData.Instance.GetInvenData();
        gold = PlayerData.Instance.GetGold();
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = gold.ToString();

        if (select_Item != null)
            select_Frame.transform.position = select_Item.transform.position;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerData.Instance.SaveInvenData(inven);
        PlayerData.Instance.SaveGold(gold);
    }

    public void AddItem(string item)
    {
        inven.Add(ItemDataManager.Instance.GetItem(item));
    }

    public void AddItem(GameObject item)
    {
        inven.Add(ItemDataManager.Instance.GetItem(item.transform.Find("Image_item").GetComponent<Image>().sprite.name));
    }

    public void RemoveItem(string item)
    {
        inven.Remove(ItemDataManager.Instance.GetItem(item));
    }

    public void UpdateInven()
    {
        foreach( Transform item in content)
        {
            Destroy(item.gameObject);
        }

        select_Item = null;
        select_Frame.SetActive(false);

        foreach (Item item in inven)
        {
            GameObject itemSlot = Instantiate(invenSlotPrefab, content.transform);
            Image itemIcon = itemSlot.transform.Find("Image_item").GetComponent<Image>();

            itemIcon.sprite = item.icon;
        }
    }

    public void SelectItem( GameObject item)
    {
        //select_Item = ItemDataManager.instance.GetItem(item.transform.Find("Image_item").GetComponent<Image>().sprite.name);
        select_Item = item;
        Debug.Log(select_Item.transform.Find("Image_item").GetComponent<Image>().sprite.name + " ����");
        sellPriceText.text = ( GetSelectItem().value / 2 ).ToString();
        select_Frame.SetActive(true);
    }

    public Item GetSelectItem()
    {
        return ItemDataManager.Instance.GetItem(select_Item.transform.Find("Image_item").GetComponent<Image>().sprite.name);
    }

    public void UseSelectItem()
    {
        if (select_Item == null)
            return;

        if (GetSelectItem().type == ItemType.Potion)
        {
            Debug.Log("������ ����߽��ϴ�.");
            RemoveItem(select_Item.transform.Find("Image_item").GetComponent<Image>().sprite.name);
            UpdateInven();
        }
    }

    public void DeleteSelectItem()
    {
        if (select_Item == null) 
            return;

        RemoveItem(select_Item.transform.Find("Image_item").GetComponent<Image>().sprite.name);
        UpdateInven();
    }

    public void SellItem()
    {
        if (select_Item == null)
            return;

        DeleteSelectItem();
        gold += GetSelectItem().value / 2;
    }
}
