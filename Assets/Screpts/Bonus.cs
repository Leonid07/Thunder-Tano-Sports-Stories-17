using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    public int attempts = 2; // ������������ ���������� �������
    public Button restoreButton; // ������ ��� ������� �������������� �������
    public GameObject buttonBack;

    private const string LastCheckKey = "LastCheckDate"; // ���� ��� ������� ��������� ��������
    private const string AttemptsKey = "Attempts"; // ���� ��� ���������� �������
    private const int MaxAttempts = 2; // �������� �������
    public int restoreIntervalSeconds = 86400; // �������� �������������� � �������� (�� ��������� 1 ����)
    //86400 ���� ����

    void Start()
    {
        LoadAttempts(); // ��������� ���������� ������� ��� �������
        CheckAndRestoreAttempts(); // ���������, ����� �� ������������ �������
        StartCoroutine(CheckRestoreCoroutine()); // ��������� �������� �������

        // ����������� ������ � ������ �������������� �������
        restoreButton.onClick.AddListener(OnRestoreButtonClick);

        UpdateButtonState(); // ��������� ��������� ������ ��� �������
    }

    public void UseAttempt()
    {
        if (attempts > 0)
        {
            attempts--;
            SaveAttempts(); // ��������� ���������� �������
            UpdateButtonState(); // ��������� ��������� ������a
        }
    }

    private void CheckAndRestoreAttempts()
    {
        if (attempts >= MaxAttempts) return; // ���� ������� 2 ��� ������, ������ �� ������

        string lastCheckString = PlayerPrefs.GetString(LastCheckKey, string.Empty);
        if (!string.IsNullOrEmpty(lastCheckString))
        {
            DateTime lastCheckTime;
            if (DateTime.TryParse(lastCheckString, null, System.Globalization.DateTimeStyles.RoundtripKind, out lastCheckTime))
            {
                DateTime now = DateTime.Now;

                if ((now - lastCheckTime).TotalSeconds >= restoreIntervalSeconds)
                {
                    attempts = MaxAttempts;
                    foreach (ButtonBonus bonus in PanelManager.InstancePanel.buttonBonuses)
                    {
                        bonus.RestartBonus();
                    }
                    SaveAttempts();

                    // ��������� ����� ��������� �������� ������ ��� ��������������
                    PlayerPrefs.SetString(LastCheckKey, now.ToString("O"));
                    PlayerPrefs.Save();
                }
            }
        }
        else
        {
            // ���� ������� ��������� �������� �� ����, ������������� ��� ������
            PlayerPrefs.SetString(LastCheckKey, DateTime.Now.ToString("O"));
            PlayerPrefs.Save();
        }
        UpdateButtonState();
    }

    private void SaveAttempts()
    {
        PlayerPrefs.SetInt(AttemptsKey, attempts);
        PlayerPrefs.Save();
    }

    private void LoadAttempts()
    {
        attempts = PlayerPrefs.GetInt(AttemptsKey, MaxAttempts);
    }

    private IEnumerator CheckRestoreCoroutine()
    {
        while (true)
        {
            CheckAndRestoreAttempts(); // ���������, ����� �� ������������ �������
            yield return new WaitForSeconds(1); // �������� ��� � ������ ��� �����������
        }
    }

    // ���������� ������� �� ������ ��������������
    private void OnRestoreButtonClick()
    {
        if (DataManager.InstanceData.coin >= 50)
        {
            DataManager.InstanceData.coin -= 50;
            DataManager.InstanceData.SaveCoin();
            DataManager.InstanceData.ApplyCoinToText();

            attempts = MaxAttempts; // ������������� ������������ ���������� �������
            SaveAttempts(); // ��������� ����� �������

            // ���������� ������
            PlayerPrefs.SetString(LastCheckKey, DateTime.Now.ToString());
            PlayerPrefs.Save();

            UpdateButtonState(); // ��������� ��������� ������
        }
    }

    // ���������� ��������� ������ (�������, ���� ������� ������ 2)
    private void UpdateButtonState()
    {
        if (attempts < 1)
        {
            restoreButton.gameObject.SetActive(true);
            buttonBack.SetActive(true);
        }
        else
        {
            restoreButton.gameObject.SetActive(false);
            buttonBack.SetActive(false);
        }
    }
}