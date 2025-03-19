using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void TakeDamage(float damage);
    bool IsDead { get; }
    int GetScoreValue(); // M�todo para obtener el valor de puntaje del enemigo

}