using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public static GameManager instance { get; private set; }

    public int explosiveAmmo = 10;
    public int health = 100;
    public int maxHealth = 100;
    private void Awake()
    {
        instance = this; 
    }

    private void Update()
    {
        ammoText.text = explosiveAmmo.ToString();
        healthText.text = health.ToString();
        CheckHealth();
    }

    public void LoseHealth(int healthToReduce) //para que se le reduzca la vida
    {
        health -= healthToReduce;
    }

    public void CheckHealth()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void HealthGain(int health)
    {
        if(this.health + health >= maxHealth) //que no se pase de 100
        {
            this.health = 100;
        }
        else
        {
            this.health += health;
        }
    }
}
