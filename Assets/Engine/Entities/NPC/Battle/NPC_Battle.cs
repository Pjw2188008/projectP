using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;

[RequireComponent(typeof(NPC_Controller))]
public class NPC_Battle : MonoBehaviour
{
    [Header("Attack")]
    public List<AttackInfo> attack_list;

    public void init(Player_Battle player)
    {
        for (int i = 0; i < attack_list.Count; i++)
        {
            attack_list[i].player = player;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AttackInfo getAttackInfo()
    {
        return attack_list[0];
    }
}
