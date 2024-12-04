using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    // Ссылка на Image, который будет вращаться
    public Image rotatingImage;

    // Текст для отображения процента загрузки
    //public TMP_Text progressText;

    // Скорость загрузки
    public float loadingSpeed = 0.5f;

    // Скорость вращения
    public float rotationSpeed = 360f;

    // Внутренний процент загрузки
    private float progress = 0f;

    void Start()
    {
        // Запускаем корутину загрузки
        StartCoroutine(StartLoading());
    }

    // Корутин для симуляции загрузки
    IEnumerator StartLoading()
    {
        while (progress < 1f)
        {
            // Увеличиваем прогресс загрузки
            progress += Time.deltaTime * loadingSpeed;
            // Ограничиваем прогресс до 1
            progress = Mathf.Clamp01(progress);
            // Обновляем текст процента загрузки
            //progressText.text = Mathf.RoundToInt(progress * 100f) + "%";
            // Вращаем Image по оси Z
            rotatingImage.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);

            // Ждем следующего кадра
            yield return null;
        }

        // Когда загрузка завершена (progress == 1)
        OnLoadingComplete();
    }

    // Метод, который вызывается, когда загрузка завершена
    void OnLoadingComplete()
    {
        // Делаем панель неактивной
        gameObject.SetActive(false);
    }
}
