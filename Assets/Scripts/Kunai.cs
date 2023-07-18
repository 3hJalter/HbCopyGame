using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public GameObject vfx;
    public Rigidbody2D rb;

    public List<Vector3> posList;

    public float timer = 0.1f;

    public int posCount;
    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        Invoke(nameof(OnDeSpawn), 4f);
        posCount = posList.Count;
    }

    private void Update()
    {
        if (posCount <= 0) return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0.1f;
            posCount--;
            var posNum = posList.Count - posCount;
            Debug.Log(posNum);
            Debug.DrawLine(posList[posNum], posList[posNum+1]);
            Vector2 direction = ((Vector2)posList[posNum + 1] - (Vector2)transform.position).normalized;
            
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
            // var angle = Mathf.Atan2(posList[posNum].y, posList[posNum].x) * Mathf.Rad2Deg; 
            transform.Rotate(0, 0, angle);
            /*var offset = 90f;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));*/
            // transform.LookAt(posList[posNum + 1], Vector3.right);
        }
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
