using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] float m_maxHealth;
    [SerializeField] float m_currentHealth;

    public float maxHealth => m_maxHealth;
    public float currentHealth => m_currentHealth;
    public UnityEvent healthHitZero;

    private void OnValidate()
    {
        m_currentHealth = m_maxHealth;
    }

    public void ChangeMaxHealth(float value)
    {
        m_maxHealth = Mathf.Clamp(m_maxHealth + value, 100, 9999);
    }

    public void ChangeCurrentHealth(float value)
    {
        m_currentHealth = Mathf.Clamp(m_currentHealth + value, 0, m_maxHealth);

        if (m_currentHealth <= 0)
        {
            if (healthHitZero != null)
            {
                healthHitZero.Invoke();
            }
        }
    }

    public void TakeDamage(Hitbox.Args args)
    {
        ChangeCurrentHealth(-args.power);
    }

    public void GenericDie()
    {
        healthHitZero.RemoveAllListeners();
        Destroy(this.gameObject);
    }
}