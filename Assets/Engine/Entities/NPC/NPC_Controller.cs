using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class NPC_Controller : Entity
{
    [Space]
    [Space]
    public int id;
    public bool mainBattle = false;
    public float range = 2.0f;

    [Space]
    [Header("Move")]
    public float speed;
    public float backwardSpeed;
    private Rigidbody2D rb;

    //[HideInInspector]
    private NPC_BattleManager battle_manager;
    /*
     * WAIT     대기
     * PATROL   순찰  
     * FOUND    ? 
     * TRACKING ! 추적
     * BATTLE   전투
     */
    public enum NPC_STATE { WAIT, PATROL, FOUND = 2, TRACKING, BATTLE }
    public NPC_STATE state = NPC_STATE.BATTLE; // [수정]

    public SpriteRenderer sprite;

    //[Header("Target")]
    //[HideInInspector]
    private GameObject player;
    private Player_Controller playerController;
    [HideInInspector]
    public NPC_Battle battle;
    private bool initBattle = true;

    [ReadOnly]
    public int yLayer = 0;

    private int dir = 1;

    private bool onSwitch;
    private Transform switchTargetPosition;
    private int switchTargetDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void init(int _id, NPC_BattleManager _battle_manager, GameObject _player)
    {
        id = _id;
        battle_manager = _battle_manager;
        player = _player;
        initBattle = true;
        playerController = player.GetComponent<Player_Controller>();
        battle = GetComponent<NPC_Battle>();
        if (battle)
        {
            Player_Battle playerBattle = player.GetComponent<Player_Battle>();
            battle.init(playerBattle);
        }
        //Invoke("Think", 5);
    }

    private void FixedUpdate()
    {
        const int layer = 1;
        yLayer = (int)((transform.position.y / layer) + 0.2f);

        //if (onSwitch)
        //{
        //    rb.linearVelocity = new Vector2(switchTargetDir * speed, 0);

        //    if (switchTargetDir == -1) 
        //    {
        //        if (transform.position.x < switchTargetPosition.position.x - 0.1)
        //        {
        //            rb.linearVelocity = Vector2.zero;
        //            onSwitch = false;
        //        }
        //    }
        //    if (switchTargetDir == 1)
        //    {
        //        if (transform.position.x > switchTargetPosition.position.x + 0.1)
        //        {
        //            rb.linearVelocity = Vector2.zero;
        //            onSwitch = false;
        //        }
        //    }

        //    return;
        //}

        if (state == NPC_STATE.TRACKING)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (state == NPC_STATE.BATTLE)
        {
            if (initBattle)
            {
                battle_manager.joinBattle(id);
                initBattle = false;
            }

            float forwardEntityX = player.transform.position.x;
            float player_dis = forwardEntityX - transform.position.x;

            range = 2.0f;

            dir = player_dis < 0 ? -1 : 1;

            float min_max__ForwardEntityX = forwardEntityX;
            if (!mainBattle) {
                if (dir == -1)  min_max__ForwardEntityX = float.MinValue;
                else            min_max__ForwardEntityX = float.MaxValue;
                
                range = 1.5f;

                for (int i = 0; i < battle_manager.battle_list.Count; i++)
                {
                    NPC_Controller entity = battle_manager.battle_list[i];
                    if (entity.id == id) continue;
                    forwardEntityX = entity.transform.position.x;
                    if (dir == -1)
                    {
                        if (forwardEntityX > min_max__ForwardEntityX
                            && forwardEntityX < transform.position.x)
                        {
                            min_max__ForwardEntityX = forwardEntityX;
                        }
                    }
                    else
                    {
                        if (forwardEntityX < min_max__ForwardEntityX
                            && forwardEntityX > transform.position.x)
                        {
                            min_max__ForwardEntityX = forwardEntityX;
                        }
                    }
                }
            }

            float dis = min_max__ForwardEntityX - transform.position.x;
            dir = dis < 0 ? -1 : 1;
            transform.localScale = new Vector3(dir, 1, 1);

            if (Math.Abs(dis) > range + 0.5f)
                rb.linearVelocity = new Vector2(dir * speed, 0);
            else if(Math.Abs(dis) < range)
            {
                rb.linearVelocity = new Vector2(-dir * (backwardSpeed + (backwardSpeed * Math.Abs(player_dis) * 0.5f)), 0); 
            }
            else
            {
                rb.linearVelocity = Vector2.zero;   
            }

            int player_yLayer = (int)((player.transform.position.y / layer) + 0.2f);
            if (yLayer != player_yLayer && playerController.onGround)
                state = NPC_STATE.TRACKING;
        }
        else initBattle = true;

    }

    void Update()
    {
        switch (state)
        {
            case NPC_STATE.WAIT:
                sprite.color = Color.white;
                break;
            case NPC_STATE.PATROL:
                sprite.color = Color.black;
                break;
            case NPC_STATE.FOUND:
                sprite.color = Color.yellow;
                break;
            case NPC_STATE.TRACKING:
                sprite.color = Color.cyan;
                break;
            case NPC_STATE.BATTLE:
                if(mainBattle) sprite.color = Color.green;
                else
                    sprite.color = Color.red;
                break;
        }
    }
    
    public void PositionSwitch(Transform target)
    {
        onSwitch = true;
        switchTargetPosition = target;
        switchTargetDir = transform.position.x > target.position.x ? -1 : 1;
    }

    void Think()
    {


        //Invoke("Think", 5);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.position + new Vector3(dir * 0.5f, 0, 0), new Vector3(range, 0, 0) * dir);
    }
}
