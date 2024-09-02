using UnityEngine;
using System.Collections;
using Zenject;       
using Installers;  
using Entity.Abilities;       
using UnityEngine.UI;
using System.Collections.Generic;

public class hearts : MonoBehaviour
{
    /*public Image[] lives;

    [Inject] private PlayerInstallation player;

    public Sprite[] a;
    public Sprite[] b;

    public Image test;

    void Start() {
        if(player.Entity.GetComponent<Hp>().Health == 5) {
            Debug.Log("TEST");
        }
    }*/


    public List<Image> lives = new List<Image>(); // Список иконок здоровья
    [Inject] private PlayerInstallation player; // Ссылка на объект игрока

    public Sprite heartFull; // Спрайт полной жизни
    public GameObject heartPrefab; // Префаб иконки жизни
    public Transform heartsContainer; // Контейнер для иконок жизни
    public float spacing = 30f; // Расстояние между иконками

    private int currentHealth; // Текущее здоровье игрока

    void Start()
    {

        currentHealth = player.Entity.GetComponent<Hp>().Health;
        Debug.Log($"Start method called. Initial Health: {currentHealth}");
        UpdateLivesList(); // Обновляем список жизней при старте
    }

    void Update()
    {
        int newHealth = player.Entity.GetComponent<Hp>().Health;

        if (newHealth != currentHealth)
        {
            currentHealth = newHealth;
            Debug.Log($"Health changed. New Health: {currentHealth}");
            UpdateLivesList(); // Обновляем список жизней, если здоровье изменилось
        }
    }

    private void UpdateLivesList()
    {
        Debug.Log($"Updating lives list. Current health: {currentHealth}, Lives count: {lives.Count}");

        // Удаляем все иконки перед обновлением списка
        foreach (var life in lives)
        {
            Destroy(life.gameObject);
        }
        lives.Clear();

        // Добавляем иконки заново в зависимости от текущего здоровья
        for (int i = 0; i < currentHealth; i++)
        {
            if (heartPrefab == null || heartsContainer == null)
            {
                Debug.LogError("Heart Prefab or Hearts Container is not assigned.");
                return;
            }

            GameObject newHeart = Instantiate(heartPrefab, heartsContainer);
            Image heartImage = newHeart.GetComponent<Image>();

            if (heartImage == null)
            {
                Debug.LogError("Heart Prefab does not have an Image component.");
                return;
            }

            heartImage.sprite = heartFull;
            heartImage.enabled = true;

            RectTransform rectTransform = newHeart.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(i * spacing, 0);

            lives.Add(heartImage);
        }

        Debug.Log($"Lives list updated. Total lives: {lives.Count}");
    }

    public void OnTakeDamageButton() {
        AddDamage(3);
    }

    public void AddDamage(int d)
    {
        currentHealth -= Mathf.Max(d, 0);
    }

}
