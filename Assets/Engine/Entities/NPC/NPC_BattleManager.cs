using System;
using System.Collections.Generic;
//using Unity.Mathematics;
using UnityEngine;
using static NPC_BattleManager;

[Serializable]
public struct BattlePattern
{
    public float term;
    public PatternState state;
}

[System.Serializable]
public class Pattern
{
    public List<BattlePattern> patterns_list;
}

public class NPC_BattleManager : MonoBehaviour
{
    public enum PatternState{ melee, strong, range }

    public EntityManger entityManger;

    public List<NPC_Controller> battle_list;
    public List<NPC_Controller> main_battle_list;
    public NPC_Controller battleNPC;

    [Space]
    public List<Pattern> battlePatterns;

    [SerializeField, ReadOnly]
    private List<int> event_list;

    private Player_Battle player_battle;


    void Start()
    {
        player_battle = entityManger.player.GetComponent<Player_Battle>();
        
        Invoke("UpdateBattle", 1);
        Invoke("Battle", 1);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

    }

    private void Update()
    {
        //
        //for (int i = 0; i < battle_list.Count; i++)
        //{

        //}
    }

    private void PositionSwitch(int target_index, int index)
    {
        battle_list[index].PositionSwitch(battle_list[target_index].transform);
    }

    void UpdateBattle()
    {
        main_battle_list.Clear();

        Vector3 playerPos = entityManger.player.transform.position;

        float dis_left = float.MaxValue, dis_right = float.MaxValue;
        int min_left_index = -1, min_right_index = -1;

        for (int i = 0; i < battle_list.Count; i++)
        {
            battle_list[i].mainBattle = false;
            //battle_list[i].state = NPC_Controller.NPC_STATE.TRACKING;
            if (battle_list[i].transform.position.x < playerPos.x) // 플레이어 기준 왼쪽
            {
                if (dis_left > playerPos.x - battle_list[i].transform.position.x) 
                { // 더 가까운
                    min_left_index = i;
                    dis_left = playerPos.x - battle_list[i].transform.position.x;
                }
            }
            if (playerPos.x < battle_list[i].transform.position.x) // 플레이어 기준 오른쪽
            {
                if (dis_right > battle_list[i].transform.position.x - playerPos.x)
                { // 더 가까운
                    min_right_index = i;
                    dis_right = battle_list[i].transform.position.x - playerPos.x;
                }
            }
        }

        if (min_left_index > -1)  joinMainBattle(battle_list[min_left_index]);
        if (min_right_index > -1) joinMainBattle(battle_list[min_right_index]);
        
        Invoke("UpdateBattle", 1);
    }

    void Battle()
    {
        print("next");

        Vector3 playerPos = entityManger.player.transform.position;
        
        int random = UnityEngine.Random.Range(0, battle_list.Count);
        int npc = battle_list[random].transform.position.x < playerPos.x ? -1 : 1;

        float dis_left = float.MaxValue, dis_right = float.MaxValue;
        int min_left_index = -1, min_right_index = -1;

        for (int i = 0; i < battle_list.Count; i++)
        {
            if (battle_list[i].transform.position.x < playerPos.x) // 플레이어 기준 왼쪽
            {
                if (dis_left > playerPos.x - battle_list[i].transform.position.x)
                { // 더 가까운
                    min_left_index = i;
                    dis_left = playerPos.x - battle_list[i].transform.position.x;
                }
            }
            if (playerPos.x < battle_list[i].transform.position.x) // 플레이어 기준 오른쪽
            {
                if (dis_right > battle_list[i].transform.position.x - playerPos.x)
                { // 더 가까운
                    min_right_index = i;
                    dis_right = battle_list[i].transform.position.x - playerPos.x;
                }
            }
        }

        if (min_left_index > -1 && npc == -1)
        {
            PositionSwitch(min_left_index, random);
        }
        if (min_right_index > -1 && npc == 1)
        {
            PositionSwitch(min_right_index, random);
        }

        //NPC_Controller lastNPC = battleNPC;

        //int random = UnityEngine.Random.Range(0, battle_list.Count);
        //battleNPC = battle_list[random];

        //if (lastNPC != null)
        //{
        //    if (lastNPC.id != battleNPC.id)
        //    {
        //        lastNPC.PositionSwitch(battleNPC.transform.position);
        //        battleNPC.PositionSwitch(lastNPC.transform.position);
        //    }
        //}

        Invoke("Battle", 5);
    }

    public void joinBattle(int index)
    {
        battle_list.Add(entityManger.npc_list[index]);
    }

    bool joinMainBattle(NPC_Controller npc)
    {
        for (int i = 0; i < main_battle_list.Count; i++)
        {
            int id = main_battle_list[i].id;
            if (id == npc.id) return false;
        }
        npc.mainBattle = true;
        npc.state = NPC_Controller.NPC_STATE.BATTLE;
        main_battle_list.Add(npc);
        return true;
    }

    
}
