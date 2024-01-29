using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth instance;

    public int currentHealth;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamageEnemy(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Destroy(gameObject);

            UI.instance.killedEnemies++;

            UI.instance.killedEnemiesText.text = "Killed Enemies: " + UI.instance.killedEnemies;
        }
    }
}
