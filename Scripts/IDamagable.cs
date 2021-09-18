using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable 
{
    /// <summary>
    /// Inflicts damage to the parent of this interface.
    /// </summary>
    /// <param name="amount">The amount of damage being dealt.</param>
    public void TakeDamage(int amount);
}
