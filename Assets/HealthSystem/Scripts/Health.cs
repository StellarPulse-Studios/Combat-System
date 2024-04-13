using UnityEngine;
using System;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public UnityEvent onHealthZero;
    public UnityEvent<float> onHealthChanged;

    private float currentHealth;


    private void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }
  
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth);
        
        if (currentHealth <= 0)
        {
            onHealthZero?.Invoke();          
        }
    }


    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        // Ensure health doesn't exceed maxHealth
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        onHealthChanged?.Invoke(currentHealth);
    }

    public float ReturnHealth()
    {
        return currentHealth;
    }

}
