using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    public int attempts = 2; // Максимальное количество попыток
    public Button restoreButton; // Кнопка для ручного восстановления попыток
    public GameObject buttonBack;

    private const string LastCheckKey = "LastCheckDate"; // Ключ для времени последней проверки
    private const string AttemptsKey = "Attempts"; // Ключ для сохранения попыток
    private const int MaxAttempts = 2; // Максимум попыток
    public int restoreIntervalSeconds = 86400; // Интервал восстановления в секундах (по умолчанию 1 день)
    //86400 один день

    void Start()
    {
        LoadAttempts(); // Загружаем количество попыток при запуске
        CheckAndRestoreAttempts(); // Проверяем, нужно ли восстановить попытки
        StartCoroutine(CheckRestoreCoroutine()); // Запускаем проверку таймера

        // Привязываем кнопку к методу восстановления попыток
        restoreButton.onClick.AddListener(OnRestoreButtonClick);

        UpdateButtonState(); // Обновляем состояние кнопки при запуске
    }

    public void UseAttempt()
    {
        if (attempts > 0)
        {
            attempts--;
            SaveAttempts(); // Сохраняем количество попыток
            UpdateButtonState(); // Обновляем состояние кнопкиa
        }
    }

    private void CheckAndRestoreAttempts()
    {
        if (attempts >= MaxAttempts) return; // Если попыток 2 или больше, ничего не делаем

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

                    // Обновляем время последней проверки только при восстановлении
                    PlayerPrefs.SetString(LastCheckKey, now.ToString("O"));
                    PlayerPrefs.Save();
                }
            }
        }
        else
        {
            // Если времени последней проверки не было, устанавливаем его сейчас
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
            CheckAndRestoreAttempts(); // Проверяем, нужно ли восстановить попытки
            yield return new WaitForSeconds(1); // Проверка раз в минуту для оптимизации
        }
    }

    // Обработчик нажатия на кнопку восстановления
    private void OnRestoreButtonClick()
    {
        if (DataManager.InstanceData.coin >= 50)
        {
            DataManager.InstanceData.coin -= 50;
            DataManager.InstanceData.SaveCoin();
            DataManager.InstanceData.ApplyCoinToText();

            attempts = MaxAttempts; // Устанавливаем максимальное количество попыток
            SaveAttempts(); // Сохраняем новые попытки

            // Сбрасываем таймер
            PlayerPrefs.SetString(LastCheckKey, DateTime.Now.ToString());
            PlayerPrefs.Save();

            UpdateButtonState(); // Обновляем состояние кнопки
        }
    }

    // Обновление состояния кнопки (активна, если попыток меньше 2)
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