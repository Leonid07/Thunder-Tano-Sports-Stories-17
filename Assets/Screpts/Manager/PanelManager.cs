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

    [Header("������ � ������ ��������������")]
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

    [Header("��������� �������� ������")]
    public Player player;
    public Camera mainCamera;           // ������ ��� ��������
    public RectTransform targetUI;      // ������� UI-������ ��� �����������
    public float zoomSpeed = 1.0f;      // �������� �����������
    public float targetFOV = 30f;       // ������� �������� FOV
    public float duration = 1.0f;       // ������������ ��������

    [Header("��������� ������ ��������")]
    public GameObject loadingPanel;     // ������, ������� ������������
    public Education education;
    public Slider loadingSlider;        // ������� ��� �������� ��������
    public float loadingTime = 5f;      // ������������ �������� (5 ������)

    public GameObject canvas;

    private Vector3 initialPosition;    // �������� ��������� ������
    private float initialFOV;           // �������� �������� FOV

    public GameObject[] canvasGame;

    // ������ �������� ����������� ������
    public void StartZoom()
    {
            StartCoroutine(ZoomToUI());
    }

    // �������� ��� �������� ���� �� UI-������
    private IEnumerator ZoomToUI()
    {
        Vector3 targetWorldPosition = WorldPositionFromUI(targetUI);

        Vector3 startPosition = mainCamera.transform.position;
        float startFOV = mainCamera.fieldOfView;
        float elapsedTime = 0;

        // ������� ����������� ������
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / duration);

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetWorldPosition, t);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);

            yield return null;
        }

        // ���������, ��� ������ ����� �� ������� ����
        mainCamera.transform.position = targetWorldPosition;
        mainCamera.fieldOfView = targetFOV;

        // ���������� ������ � ��������� � ��������� �������� ��������
        loadingPanel.SetActive(true);
        ResetPositionCamera();
        StartCoroutine(LoadingRoutine());
    }

    // �������� �������� ��������
    private IEnumerator LoadingRoutine()
    {
        float elapsedTime = 0;

        // ���������� �������� ��������
        loadingSlider.value = 0;

        // ������� ���������� �������� � ������� loadingTime
        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            loadingSlider.value = Mathf.Clamp01(elapsedTime / loadingTime);
            yield return null;
        }

        // ������������ ������ � ���������� ������ �� �������� �������
        education.CheckEducation();
        //loadingPanel.SetActive(false);
    }

    public void ResetPositionCamera()
    {
        mainCamera.transform.position = initialPosition;
        mainCamera.fieldOfView = initialFOV;
    }

    // �������������� ������� UI-������� � ������� ����������
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

    [Header("������ �������� � ���������")]
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