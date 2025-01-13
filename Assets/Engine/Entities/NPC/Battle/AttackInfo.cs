using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AttackInfo : MonoBehaviour
{
    public enum AttackState { grade, parry, damage }

    //[HideInInspector]
    public Player_Battle player;
    private bool lastFrameGuard = false;
    private bool isHit = false;

    // 
    public float parringTerm; // 데미지 타임 몇초 전 패링 텀 / 패링 텀 이전의 방어는 패링이 아닌 방어로 취급
    public float attackTime; // 공격 길이, 끝나면 데미지가 들어감
    private float time;

    // 데미지 관련
    public int damage;
    public bool isStrongAttack; // 가드가 불가능한 공격인지
    public Vector2 areaSize;

    public SpriteRenderer test;

    private bool attack;

    private void Start()
    {
        //Attack();
    }

    // 이 공격 오브젝트가 활성화 되면 공격 시작으로 인지
    public void Attack() // 공격 시작 / 초기값
    {
        test.color = Color.white;
        time = 0;
        attack = true;
        isHit = false;
        //Invoke("Attack", 3.0f);
    }

    private void Update()
    {
        if (!attack) return;

        if (time <= attackTime)
        {
            time += Time.deltaTime;
            if (time >= attackTime - parringTerm)
            {
                test.color = Color.yellow;

                RaycastHit2D rayHit = Physics2D.BoxCast(
                    transform.position, areaSize,
                    0, Vector2.right,
                    0,
                    LayerMask.GetMask("Player")
                );

                if (rayHit.collider != null)
                {
                    isHit = true;
                    if (!lastFrameGuard && player.onGuard)
                    {
                        takeParrying();
                    }
                }
            }
            else
            {
                lastFrameGuard = player.onGuard;
            }
        }
        else if (isHit)
        {
            test.color = Color.red;
            
            if (player.onGuard && !isStrongAttack) 
                player.TakeGuard(damage);
            else           
                player.TakeDamage(damage);  

            attack = false;
        }
    }

    void takeParrying()
    {
        test.color = Color.white;
        lastFrameGuard = false;
        attack = false;
        print("parring");
        // player.parrying 추가
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
