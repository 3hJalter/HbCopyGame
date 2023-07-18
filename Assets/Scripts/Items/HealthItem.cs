using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    [SerializeField] private float health = 30f;
    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnTriggerEffect(Player player)
    {
        base.OnTriggerEffect(player);
        player.hp += health;
        if (player.hp > 100) player.hp = 100;
        player.healthBar.SetNewHp(player.hp);
        OnDestroy();
    }
}
