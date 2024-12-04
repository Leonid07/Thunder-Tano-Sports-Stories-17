using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Education : MonoBehaviour
{
    public GameObject panelEducation;
    public Image panelEducationImage;
    public Sprite panelEducationIncorrect;
    public int educationIsPassed = 0;
    public string educationID = "Educaion";

    public Button firstAnswer;
    public Button secondAnswer;

    public GameObject firstHand;
    public GameObject secondHand;

    public AudioSource mainAudioSourse;
    public AudioClip correct;
    public AudioClip incorrect;

    public void CheckEducation()
    {
        LoadEducation();
        if (educationIsPassed >= 1)
        {
            panelEducation.SetActive(false);
            DataManager.InstanceData.mapNextLevel.LoadLevel();
        }
        else
        {
            panelEducation.SetActive(true);
        }

        firstAnswer.onClick.AddListener(FirstAnswer);
        secondAnswer.onClick.AddListener(SecondAnswer);

        secondAnswer.enabled = false;
    }

    public void FirstAnswer()
    {
        firstHand.SetActive(false);
        secondHand.SetActive(true);
        mainAudioSourse.clip = correct;
        mainAudioSourse.Play();
        secondAnswer.enabled = true;
    }

    public void SecondAnswer()
    {
        secondHand.SetActive(false);
        mainAudioSourse.clip = incorrect;
        mainAudioSourse.Play();
        panelEducationImage.sprite = panelEducationIncorrect;
        StartCoroutine(EducationIsPassed());
    }
    public IEnumerator EducationIsPassed()
    {
        educationIsPassed++;
        SaveEducation();
        yield return new WaitForSeconds(2);
        panelEducation.SetActive(false);
        gameObject.SetActive(false);
        DataManager.InstanceData.levels[0].LoadLevel();
        PanelManager.InstancePanel.StartGame();
    }
    public void SaveEducation()
    {
        PlayerPrefs.SetInt(educationID, educationIsPassed);
        PlayerPrefs.Save();
    }
    public void LoadEducation()
    {
        if (PlayerPrefs.HasKey(educationID))
        {
            educationIsPassed = PlayerPrefs.GetInt(educationID);
        }
    }
}