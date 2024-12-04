using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager InstanceData { get; private set; }

    private void Awake()
    {
        if (InstanceData != null && InstanceData != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceData = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public int coin;
    public string idCoin = "coin";
    public TMP_Text[] textCountCoin;

    public Map[] levels;

    public Map mapNextLevel;

    private void Start()
    {
        LoadCoin();
        ApplyCoinToText();
        SetIndexLevel();
        LoadLevel();
        SetIndexTextMap();
    }
    public void SetIndexTextMap()
    {
        int count = 1;
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetTextIndexMap(count.ToString());
            count++;
        }
    }

    public void SaveCoin()
    {
        PlayerPrefs.SetInt(idCoin, coin);
        PlayerPrefs.Save();
    }
    public void LoadCoin()
    {
        if (PlayerPrefs.HasKey(idCoin))
        {
            coin = PlayerPrefs.GetInt(idCoin);
        }
    }
    public void ApplyCoinToText()
    {
        foreach (TMP_Text text in textCountCoin)
        {
            text.text = coin.ToString();
        }
    }
    public void SetIndexLevel()
    {
        int count = 1;
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].indexLevel = count;
            count++;
        }
    }
    public void SaveLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            PlayerPrefs.SetInt(levels[i].idLevel, levels[i].isLoad);
            PlayerPrefs.Save();
        }
    }
    public void LoadLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (PlayerPrefs.HasKey(levels[i].idLevel))
            {
                levels[i].isLoad = PlayerPrefs.GetInt(levels[i].idLevel);
                levels[i].CheckLevel();
            }
        }
    }

    [Header("Хороший результат для игры")]
    public int countGood;
    public int countNeedToMoveNextLevel = 2;
}