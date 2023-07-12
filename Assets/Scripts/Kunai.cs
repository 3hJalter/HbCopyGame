using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public GameObject vfx;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        rb.velocity = transform.right * 5f;
        Invoke(nameof(OnDeSpawn), 4f);
    }

    public void OnDeSpawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy")) return;
        col.GetComponent<Character>().OnHit(30f);
        var transform1 = transform;
        Instantiate(vfx, transform1.position, transform1.rotation);
        OnDeSpawn();
    }
}
