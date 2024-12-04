using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopPoint : MonoBehaviour
{
    public bool requiresPause = true; // Определяет, нужно ли останавливать персонажа на этой точке
    public bool lastEnemy = false;
    public Enemy enemy;

    public float fadeDuration = 1.0f;

    [Header("Параметры игрока")]
    public GameObject spawnPoint_1;

    [Header("Управление канвасом")]
    public Button buttonCorrectAnsver;
    public Button buttonInCorrectAncver;
    public int canvasIndex;

    public void Start()
    {
        buttonCorrectAnsver.onClick.AddListener(() =>
        {
            StartCoroutine(FadeAndPlaySoundCorrect());
            OnReached();
        });
        buttonInCorrectAncver.onClick.AddListener(() =>
        {
            StartCoroutine(FadeAndPlaySoundInCorrect());
            OnReached();
        });
    }

    public void ActiveCanvas()
    {
        PanelManager.InstancePanel.canvasGame[canvasIndex].SetActive(true);
    }

    // Метод для обработки достижения точки (можно добавить другие параметры и действия)
    public void OnReached()
    {
        if (requiresPause)
        {
            if (enemy != null)
            {
                if (lastEnemy == true)
                {
                    if (DataManager.InstanceData.mapNextLevel.mapNextLevel.isLoad == 0)
                    {
                        PanelManager.InstancePanel.SetActivePanel(true);
                        DataManager.InstanceData.mapNextLevel.OpenLevel();
                    }
                    else
                    {
                        Debug.Log("прохождение одного и тогоже уровня");
                        PanelManager.InstancePanel.SetActivePanel(false);
                    }

                    if (DataManager.InstanceData.countGood > DataManager.InstanceData.countNeedToMoveNextLevel)
                    {
                        PanelManager.InstancePanel.panelWin.SetActive(true);
                        DataManager.InstanceData.coin += 500;
                        DataManager.InstanceData.SaveCoin();
                        DataManager.InstanceData.ApplyCoinToText();

                        PanelManager.InstancePanel.player.gameObject.SetActive(false);
                        PanelManager.InstancePanel.canvas.SetActive(true);

                        SoundManager.InstanceSound.musicFon_1.Stop();
                        SoundManager.InstanceSound.musicFon_2.Stop();
                        SoundManager.InstanceSound.musicLevelMainMenu.Play();
                        PanelManager.InstancePanel.RestartCanvas();

                        Debug.Log("End Game");
                    }
                    else
                    {
                        PanelManager.InstancePanel.panelLose.SetActive(true);

                        PanelManager.InstancePanel.canvas.SetActive(true);
                        PanelManager.InstancePanel.player.gameObject.SetActive(false);

                        SoundManager.InstanceSound.musicFon_1.Stop();
                        SoundManager.InstanceSound.musicFon_2.Stop();
                        SoundManager.InstanceSound.musicLevelMainMenu.Play();

                        Debug.Log("End Game");
                    }
                }
            }
        }
    }

    private IEnumerator FadeAndPlaySoundCorrect()
    {
        DataManager.InstanceData.countGood++;
        float elapsedTime = 0f;
        Color originalColor = enemy.spriteEnemy.color;

        SoundManager.InstanceSound.CorrectQuestion();

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(originalColor.a, 0, t);

            enemy.spriteEnemy.color = newColor;
            yield return null;
        }

        PanelManager.InstancePanel.canvasGame[canvasIndex].SetActive(false);
        // Снятие флага ожидания и продолжение движения
        PanelManager.InstancePanel.player.OnContinueButtonPressed();
    }

    private IEnumerator FadeAndPlaySoundInCorrect()
    {
        float elapsedTime = 0f;
        Color originalColor = enemy.spriteEnemy.color;

        SoundManager.InstanceSound.IncorectQuestion();

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(originalColor.a, 0, t);

            enemy.spriteEnemy.color = newColor;
            yield return null;
        }

        PanelManager.InstancePanel.canvasGame[canvasIndex].SetActive(false);

        // Снятие флага ожидания и продолжение движения
        PanelManager.InstancePanel.player.OnContinueButtonPressed();
    }
}