using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}
