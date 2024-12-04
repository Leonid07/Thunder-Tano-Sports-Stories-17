using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public int isBuy = 0;
    // 0 не куплено
    // 1 куплено
    // 2 одето

    public string idCharacter;
    public bool firstCharacter = false;

    [Header("Параметры для кнопки")]
    public Button buttonBuy;
    public Image buttonImage;
    public Sprite standartSprite;
    public Sprite notBuySprite;
    public TMP_Text textButton;
    public int price = 1000;
    public Color colorSelect = new Color(0,255,0,255);
    public Color colorNoySelect = new Color(255,255,255,255);

    public int bonusCharacter = 150;
    public bool isActiveExclusiveCharacter = false;

    private void Start()
    {
        if (firstCharacter == true)
        {
            SaveCharacter();
            Check();
        }

        buttonBuy.onClick.AddListener(BuyCharacter);

        idCharacter = gameObject.name;
        LoadCharacter();
    }

    public void BuyCharacter()
    {
        if (isBuy == 0)
        {
            if (DataManager.InstanceData.coin >= price)
            {
                DataManager.InstanceData.coin -= price;
                DataManager.InstanceData.SaveCoin();
                DataManager.InstanceData.ApplyCoinToText();
                isBuy++;
                Check();

                SaveCharacter();
                return;
            }
        }
        if (isBuy == 1)
        {
            for (int i = 0; i < PanelManager.InstancePanel.characters.Length; i++)
            {
                if (PanelManager.InstancePanel.characters[i].isBuy == 0)
                {
                    continue;
                }
                if (PanelManager.InstancePanel.characters[i].isBuy == 2)
                {
                    PanelManager.InstancePanel.characters[i].isBuy = 1;
                    PanelManager.InstancePanel.characters[i].Check();
                }
            }
        }
        isBuy = 2;
        Check();
        SaveCharacter();
    }

    public void Check()
    {
        switch (isBuy)
        {
            case 0:
                buttonImage.sprite = notBuySprite;
                textButton.text = price.ToString();
                break;
            case 1:
                buttonImage.sprite = standartSprite;
                buttonImage.color = colorNoySelect;
                textButton.text = "bought";

                SaveCharacter();
                break;
            case 2:
                buttonImage.sprite = standartSprite;
                buttonImage.color = colorSelect;
                textButton.text = "applied";

                SaveCharacter();
                break;
        }
    }

    public void SaveCharacter()
    {
        PlayerPrefs.SetInt(idCharacter, isBuy);
        PlayerPrefs.Save();
    }

    public void LoadCharacter()
    {
        if (PlayerPrefs.HasKey(idCharacter))
        {
            isBuy = PlayerPrefs.GetInt(idCharacter);
            Check();
        }
    }
}