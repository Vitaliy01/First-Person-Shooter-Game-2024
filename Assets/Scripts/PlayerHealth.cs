using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    public int maxHealth, currentHealth;

    public float timeUntilFinalScreen = 1f;

    public string finalScreenScene;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UI.instance.healthSlider.maxValue = maxHealth;
        UI.instance.healthSlider.value = currentHealth;
        UI.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(int damage)
    {
        currentHealth -= damage;

        UI.instance.ShowDamage();

        if (currentHealth <= 0)
        {           
            //gameObject.SetActive(false);

            currentHealth = 0;

            StartCoroutine(WaitingForFinalScreen());
        }

        UI.instance.healthSlider.value = currentHealth;
        UI.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    public void HealPlayer(int heal)
    {
        currentHealth += heal;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UI.instance.healthSlider.value = currentHealth;
        UI.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    public IEnumerator WaitingForFinalScreen()
    {
        yield return new WaitForSeconds(timeUntilFinalScreen);

        SceneManager.LoadScene(finalScreenScene);

        Cursor.lockState = CursorLockMode.None;
    }
}
