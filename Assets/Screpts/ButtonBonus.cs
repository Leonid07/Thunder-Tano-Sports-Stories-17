using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBonus : MonoBehaviour
{
    public Bonus bonus;
    public int minBonus = 10;
    public int maxBonus = 200;

    public GameObject secondState;
    public TMP_Text textCountBonus;

    Button thisButton;

    private void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(TakeBonus);
    }

    private void TakeBonus()
    {
        if (bonus.attempts <= 0)
            return;

        bonus.UseAttempt();
        secondState.SetActive(true);
        int rand = UnityEngine.Random.Range(minBonus, maxBonus);
        textCountBonus.text = rand.ToString();
        DataManager.InstanceData.coin += rand;
        DataManager.InstanceData.SaveCoin();
        DataManager.InstanceData.ApplyCoinToText();
    }
    public void RestartBonus()
    {
        secondState.SetActive(false);
    }
}
