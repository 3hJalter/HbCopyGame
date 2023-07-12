using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image imageFill;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Transform target;
    private float hp;

    private float maxHp;

    // Start is called before the first frame update
    private void Start()
    {
    }


    // Update is called once per frame
    private void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp / maxHp, Time.deltaTime * 5f);
        transform.position = target.position + offset;
    }

    public void OnInit(float initHp, Transform initTarget)
    {
        target = initTarget;
        maxHp = initHp;
        hp = maxHp;
        imageFill.fillAmount = 1;
    }

    public void SetNewHp(float changeHp)
    {
        hp = changeHp;
    }
}