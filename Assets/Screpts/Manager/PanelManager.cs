using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public static PanelManager InstancePanel { get; private set; }

    private void Awake()
    {
        if (InstancePanel != null && InstancePanel != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstancePanel = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public ButtonBonus[] buttonBonuses;
    public Character[] characters;

    [Header("Панели и кнопки взаимодействия")]
    public GameObject[] panelIsActive;
    public GameObject[] allPanel;
    public GameObject panelMain;
    public GameObject panelOption;
    public GameObject panelBonus;
    public GameObject panelCharacter;
    [Header("Buttons")]
    public Button[] buttonBack;
    public Button buttonBonus;
    public Button buttonCharacter;
    public Button buttonSetting;

    private void Start()
    {
        for (int i = 0; i < buttonBack.Length; i++)
        {
            int count = i;
            buttonBack[count].onClick.AddListener(DisablePanel);
        }

        buttonBonus.onClick.AddListener(()=> { SetActivePanel(panelBonus); });
        buttonCharacter.onClick.AddListener(() => { SetActivePanel(panelCharacter); });
        buttonSetting.onClick.AddListener(() => { SetActivePanel(panelOption); });

        buttonLosePanelNext.onClick.AddListener(CloseLoseAndWinPanel);
        buttonWinPanelNext.onClick.AddListener(CloseLoseAndWinPanel);

        initialPosition = mainCamera.transform.position;
        initialFOV = mainCamera.fieldOfView;
    }

    private void SetActivePanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void DisablePanel()
    {
        foreach (GameObject gm in allPanel)
        {
            gm.SetActive(false);
        }
    }
    public void SetDisActivePanel()
    {
        for (int i = 0; i < panelIsActive.Length; i++)
        {
            panelIsActive[i].SetActive(false);
        }
    }
    public void SetActivePanel(bool lose = false)
    {
        if (lose == false)
        {
            for (int i = 0; i < panelIsActive.Length; i++)
            {
                panelIsActive[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < panelIsActive.Length; i++)
            {
                panelIsActive[i].SetActive(false);
            }
        }
    }

    [Header("Настройки анимации камеры")]
    public Player player;
    public Camera mainCamera;           // Камера для анимации
    public RectTransform targetUI;      // Целевой UI-объект для фокусировки
    public float zoomSpeed = 1.0f;      // Скорость приближения
    public float targetFOV = 30f;       // Целевое значение FOV
    public float duration = 1.0f;       // Длительность анимации

    [Header("Настройки панели загрузки")]
    public GameObject loadingPanel;     // Панель, которая активируется
    public Education education;
    public Slider loadingSlider;        // Слайдер для имитации загрузки
    public float loadingTime = 5f;      // Длительность загрузки (5 секунд)

    public GameObject canvas;

    private Vector3 initialPosition;    // Исходное положение камеры
    private float initialFOV;           // Исходное значение FOV

    public GameObject[] canvasGame;

    // Запуск анимации приближения камеры
    public void StartZoom()
    {
            StartCoroutine(ZoomToUI());
    }

    // Корутина для плавного зума на UI-объект
    private IEnumerator ZoomToUI()
    {
        Vector3 targetWorldPosition = WorldPositionFromUI(targetUI);

        Vector3 startPosition = mainCamera.transform.position;
        float startFOV = mainCamera.fieldOfView;
        float elapsedTime = 0;

        // Плавное приближение камеры
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / duration);

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetWorldPosition, t);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);

            yield return null;
        }

        // Убедиться, что камера точно на позиции цели
        mainCamera.transform.position = targetWorldPosition;
        mainCamera.fieldOfView = targetFOV;

        // Активируем панель с загрузкой и запускаем анимацию слайдера
        loadingPanel.SetActive(true);
        ResetPositionCamera();
        StartCoroutine(LoadingRoutine());
    }

    // Корутина имитации загрузки
    private IEnumerator LoadingRoutine()
    {
        float elapsedTime = 0;

        // Сбрасываем значение слайдера
        loadingSlider.value = 0;

        // Плавное заполнение слайдера в течение loadingTime
        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            loadingSlider.value = Mathf.Clamp01(elapsedTime / loadingTime);
            yield return null;
        }

        // Деактивируем панель и возвращаем камеру на исходную позицию
        education.CheckEducation();
        //loadingPanel.SetActive(false);
    }

    public void ResetPositionCamera()
    {
        mainCamera.transform.position = initialPosition;
        mainCamera.fieldOfView = initialFOV;
    }

    // Преобразование позиции UI-объекта в мировые координаты
    private Vector3 WorldPositionFromUI(RectTransform uiElement)
    {
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, uiElement.position);
        float distanceFromCamera = (mainCamera.nearClipPlane + mainCamera.farClipPlane) / 2;
        return mainCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, distanceFromCamera));
    }

    public void RestartCanvas()
    {
        foreach (GameObject can in canvasGame)
        {
            can.SetActive(false);
        }
    }

    public void StartGame()
    {
        canvas.SetActive(false);
        player.gameObject.SetActive(true);
        player.RestartMovement();
    }

    [Header("Панель выйгрыша и проигрыша")]
    public GameObject panelLose;
    public GameObject panelWin;

    public Button buttonLosePanelNext;
    public Button buttonWinPanelNext;

    public void CloseLoseAndWinPanel()
    {
        panelLose.SetActive(false);
        panelWin.SetActive(false);
        player.gameObject.SetActive(false);
    }
}