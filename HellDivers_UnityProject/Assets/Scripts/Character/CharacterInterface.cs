using UnityEngine;

public interface IDamager
{
    /// <summary>
    /// Get damage point.
    /// </summary>
    float Damage { get; }
}

public interface IDamageable
{
    /// <summary>
    /// Was the character already dead.
    /// </summary>
    bool IsDead { get; }

    /// <summary>
    /// Decrease current health point by damage point.
    /// </summary>
    /// <param name="dmg">Damage point</param>
    /// <param name="hitPoint">Hit point position</param>
    /// <returns>Was the current health point decreased or not ?</returns>
    bool TakeDamage(float dmg, Vector3 hitPoint);

    /// <summary>
    /// Decrease current health point by damage point.
    /// </summary>
    /// <param name="damager">The damager</param>
    /// <param name="hitPoint">Hit point position</param>
    /// <returns>Was the current health point decreased or not ?</returns>
    bool TakeDamage(IDamager damager, Vector3 hitPoint);
}

public interface IHealable
{
    /// <summary>
    /// Was the character already dead.
    /// </summary>
    bool IsDead { get; }

    /// <summary>
    /// Current health point.
    /// </summary>
    float CurrentHp { get; }

    /// <summary>
    /// Represent the current health point of max health point. ( 0 - 1 )
    /// </summary>
    float CurrentHpPercent { get; }

    /// <summary>
    /// Increase current health point HP.
    /// </summary>
    /// <param name="heal">Heal point</param>
    /// <returns>Was the current health point increased or not ?</returns>
    bool TakeHealth(float heal);
}