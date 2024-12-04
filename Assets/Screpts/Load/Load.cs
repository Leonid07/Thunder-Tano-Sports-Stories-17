using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    // ������ �� Image, ������� ����� ���������
    public Image rotatingImage;

    // ����� ��� ����������� �������� ��������
    //public TMP_Text progressText;

    // �������� ��������
    public float loadingSpeed = 0.5f;

    // �������� ��������
    public float rotationSpeed = 360f;

    // ���������� ������� ��������
    private float progress = 0f;

    void Start()
    {
        // ��������� �������� ��������
        StartCoroutine(StartLoading());
    }

    // ������� ��� ��������� ��������
    IEnumerator StartLoading()
    {
        while (progress < 1f)
        {
            // ����������� �������� ��������
            progress += Time.deltaTime * loadingSpeed;
            // ������������ �������� �� 1
            progress = Mathf.Clamp01(progress);
            // ��������� ����� �������� ��������
            //progressText.text = Mathf.RoundToInt(progress * 100f) + "%";
            // ������� Image �� ��� Z
            rotatingImage.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);

            // ���� ���������� �����
            yield return null;
        }

        // ����� �������� ��������� (progress == 1)
        OnLoadingComplete();
    }

    // �����, ������� ����������, ����� �������� ���������
    void OnLoadingComplete()
    {
        // ������ ������ ����������
        gameObject.SetActive(false);
    }
}
