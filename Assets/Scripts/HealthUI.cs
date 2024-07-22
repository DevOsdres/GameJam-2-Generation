using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] hearts;
    public int health = 3;

    public void TakeDamage()
    {
        if (health > 0)
        {
            health--;
            hearts[health].enabled = false;
        }
    }

    public void Heal()
    {
        if (health < hearts.Length)
        {
            hearts[health].enabled = true;
            health++;
        }
    }
}

