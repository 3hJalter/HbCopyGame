using UnityEngine;
using UnityEngine.Serialization;

public class AttackArea : MonoBehaviour
{
    [FormerlySerializedAs("collider2D")] public EdgeCollider2D col2d;
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Trigger");
        if (col.CompareTag("Player") || col.CompareTag("Enemy"))
        {
            Debug.Log("Trigger");
            col.GetComponent<Character>().OnHit(30f);
            // col2d.enabled = false;
        }
    }
}
