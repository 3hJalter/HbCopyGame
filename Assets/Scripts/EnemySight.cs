using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public Enemy enemy;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            enemy.SetTarget(col.GetComponent<Character>());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        enemy.SetTarget(null);
    }
}
