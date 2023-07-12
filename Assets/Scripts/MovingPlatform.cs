using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform aPoint, bPoint;

    [SerializeField] private Vector3 target;

    [SerializeField] private float speed;

    // Start is called before the first frame update
    private void Start()
    {
        transform.position = aPoint.position;
        target = bPoint.position;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, aPoint.position) < 0.1f)
            target = bPoint.position;
        else if (Vector2.Distance(transform.position, bPoint.position) < 0.1f)
            target = aPoint.position;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            col.transform.SetParent(transform);
            col.transform.GetComponent<Player>().OnMovingPlatform();
        }
            
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
            other.transform.SetParent(null);
    }
}
