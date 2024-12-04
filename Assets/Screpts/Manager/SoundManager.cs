using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip soundInCorrect;
    public AudioClip soundCorrect;
    
    [Space(10)]
    public AudioSource musicLevelMainMenu;
    public AudioSource soundAnsverquestion;
    public AudioSource musicFon_1;
    public AudioSource musicFon_2;

    public static SoundManager InstanceSound { get; private set; }

    private void Awake()
    {
        if (InstanceSound != null && InstanceSound != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceSound = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void CorrectQuestion()
    {
        soundAnsverquestion.clip = soundCorrect;
        soundAnsverquestion.Play();
    }
    public void IncorectQuestion()
    {
        soundAnsverquestion.clip = soundInCorrect;
        soundAnsverquestion.Play();
    }
}
