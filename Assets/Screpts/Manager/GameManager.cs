using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager InstanceGame { get; private set; }
    private void Awake()
    {
        if (InstanceGame != null && InstanceGame != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceGame = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [Header("Противники")]
    public Enemy[] enemies;
    public SpriteRenderer[] spriteRendererEnemy;
    public Sprite[] spriteEnemy;

    public void RestartEnemy()
    {
        for (int i = 0; i < spriteRendererEnemy.Length; i++)
        {
            Color currentColor = spriteRendererEnemy[i].color;
            float newAlpha = 1f;
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            spriteRendererEnemy[i].color = newColor;
        }
    }
    public void StartGame()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            int rand = Random.Range(0, spriteEnemy.Length);
            enemies[i].spriteEnemy.sprite = spriteEnemy[rand];
        }
    }
}
