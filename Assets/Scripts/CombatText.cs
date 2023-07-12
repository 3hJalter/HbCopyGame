 using System.Collections;
using System.Collections.Generic;
 using System.Globalization;
 using TMPro;
 using UnityEngine;

public class CombatText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    public void OnInit(float damage)
    {
        text.text = damage.ToString(CultureInfo.InvariantCulture);
        Invoke(nameof(OnDeSpawn), 1f);
    }

    public void OnDeSpawn()
    {
       Destroy(gameObject); 
    }
}
