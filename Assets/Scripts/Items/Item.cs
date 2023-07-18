using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    protected virtual void OnInit() {
    
    }

    protected virtual void OnTriggerEffect(Player player)
    {
        
    }

    public virtual void OnDestroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.transform.CompareTag("Player")) return;
        OnTriggerEffect(col.GetComponent<Player>());
    }
}
