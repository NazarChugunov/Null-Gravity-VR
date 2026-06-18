using UnityEngine;

// Це інтерфейс. У ньому немає логіки, лише правило: 
// "Кожен, хто використовує цей інтерфейс, зобов'язаний мати метод TakeDamage"
public interface IDamageable
{
    void TakeDamage(float damageAmount);
}