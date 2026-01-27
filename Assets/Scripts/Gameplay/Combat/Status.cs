using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class Status : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BaseStatProfile baseStatProfile;

    [Header("Dynamic Stats")]
    [SerializeField] float m_currentHealth;
    [SerializeField] float m_currentStance = 100f;

    [Header("Static Stats")]
    [SerializeField] int m_level = 1;
    [SerializeField] float m_maxHealth;
    [SerializeField] float m_physDef;
    [SerializeField] float m_elemDef;
    [SerializeField] float m_physAtk;
    [SerializeField] float m_elemAtk;

    public int Level => m_level;
    public float MaxHealth => m_maxHealth;
    public float CurrentHealth => m_currentHealth;
    
    public UnityEvent HealthHitZero;
    public UnityEvent<Hitbox.Args> DeliverHitboxArgs;

    public void CalculateStats()
    {
        m_maxHealth = baseStatProfile.BaseHp * (1 + baseStatProfile.Constitution / 100 * m_level);

        m_physDef = baseStatProfile.PhysDef * (1 + baseStatProfile.Toughness / 100 * m_level);

        m_elemDef = baseStatProfile.ElemDef * (1 + baseStatProfile.Spirit / 100 * m_level);

        m_physAtk = baseStatProfile.PhysAtk * (1 + baseStatProfile.Strength / 100 * m_level);

        m_elemAtk = baseStatProfile.ElemAtk * (1 + baseStatProfile.Mysticism / 100 * m_level);
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
            if (HealthHitZero != null)
            {
                HealthHitZero.Invoke();
            }
        }
    }

    public void TakeDamage(Hitbox.Args args)
    {
        ChangeCurrentHealth(-args.power);
    }

    public void TakeDamage(float amount)
    {
        ChangeCurrentHealth(-amount);
    }

    public void GenericDie()
    {
        HealthHitZero.RemoveAllListeners();
        Destroy(this.gameObject);
    }

    public Hitbox.Args GenerateHbArgs(Hitbox.DamageType damageType)
    {
        float power = 0;

        CalculateStats();

        power = damageType == Hitbox.DamageType.Slash || damageType == Hitbox.DamageType.Blunt ? m_physAtk : m_elemAtk;

        //Hitbox.Args args = new Hitbox.Args(power, power / 10, _damageType: damageType);
        Hitbox.Args args = new Hitbox.ArgsBuilder()
            .WithPower(power)
            .WithKnockback(power / 10)
            .WithDamageType(damageType)
            .Build();

        DeliverHitboxArgs.Invoke(args);
        return args;
    }
}

[CustomEditor(typeof(Status))]
public class Status_Inspector : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Status status = (Status)target;

        if(GUILayout.Button("Calculate Stats"))
        {
            status.CalculateStats();
        }
    }
}