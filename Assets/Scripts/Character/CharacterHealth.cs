using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    [field:SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public bool IsDead { get; set; }
    public Transform Transform { get; set; }

    public event Action<float, Transform> OnDamageTaken;
    public event Action OnDeath;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        IsDead = false;
        Transform = transform;
    }

    public void TakeDamage(float damage, Transform source)
    {
        if (IsDead || damage <= 0)
            return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        
        OnDamageTaken?.Invoke(damage, source);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }
}