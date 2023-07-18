using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera _camera;
    public Player player;
    public Transform target;
    public Vector3 offset;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    private void Start()
    {
        _camera = GetComponent<Camera>();
        player = FindObjectOfType<Player>();
        target = player.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        player.mousePos = new Vector3(mousePos.x, mousePos.y, mousePos.z + 10f);
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}
