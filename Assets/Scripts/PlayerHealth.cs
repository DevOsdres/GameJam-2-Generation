using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5; // Vida máxima del jugador
    private int currentHealth;

    public Image[] healthHearts; // Imágenes de los corazones
    public Sprite fullHeart; // Sprite de corazón lleno
    public Sprite emptyHeart; // Sprite de corazón vacío

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                GetComponent<PlayerController>().TakeDamage(); // Reproduce la animación de ser golpeado
            }
        }
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < healthHearts.Length; i++)
        {
            if (i < currentHealth)
            {
                healthHearts[i].sprite = fullHeart;
            }
            else
            {
                healthHearts[i].sprite = emptyHeart;
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        GetComponent<PlayerController>().Die(); // Reproduce la animación de muerte
        Time.timeScale = 0f; // Pausa el juego
        SceneManager.LoadScene(5);
    }
}