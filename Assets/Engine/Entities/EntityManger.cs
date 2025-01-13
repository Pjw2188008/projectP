using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManger : MonoBehaviour
{
    public GameObject player;

    public List<NPC_Controller> npc_list;

    public NPC_BattleManager battle_manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] entity = GameObject.FindGameObjectsWithTag("Entity");
        for (int i = 0; i < entity.Length; i++)
        {
            npc_list.Add(entity[i].GetComponent<NPC_Controller>());
            npc_list[i].init(i, battle_manager, player);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;

        ////for (int i = 0; i < npc_list.Count - 1; i++) {
        ////    Gizmos.DrawLine(npc_list[i].transform.position, npc_list[i + 1].transform.position);
        ////}
    }
}
