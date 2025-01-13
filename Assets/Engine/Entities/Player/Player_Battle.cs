using NUnit.Framework.Internal;
using UnityEngine;
using static NPC_Controller;

public class Player_Battle : Entity
{
    public EntityManger entityManger;

    [Header("Health Bar")]
    public Transform curHealth;
    public Transform curHealthEffect;
    public Transform curHealthPreserve;
    private bool onPreserve;
    private Vector3 lastHealthSize = Vector3.one;
    public float healthPreserveTime; 
    private float time;

    [Header("Assassination")]
    public float range = 10;
    private NPC_Controller target;

    [Header("Combat")]
    private float a;

    public bool onGuard;

    // Update is called once per frame
    void Update()
    {
        target = findTarget();

        if (Input.GetKeyDown(KeyList.attack))
        {
            if (target)
            {
                if (target.state < NPC_STATE.TRACKING)
                {
                    transform.position = target.transform.position;
                }
            }
        }
        onGuard = Input.GetKey(KeyList.attack);


        float health_bar_size = (float)health / maxHealth;

        curHealth.localScale = new Vector3(health_bar_size, 1, 1);
        curHealthEffect.localScale = Vector3.Lerp(curHealthEffect.localScale, new Vector3(health_bar_size, 1, 1), 5.5f * Time.deltaTime);

        if (time <= healthPreserveTime)
        {
            time += Time.deltaTime;
            onPreserve = true;
        }
        else
        {
            curHealthPreserve.localScale = Vector3.Lerp(curHealthPreserve.localScale, lastHealthSize, 5.5f * Time.deltaTime);
            onPreserve = false;
            if (curHealthPreserve.localScale.x <= lastHealthSize.x + 0.001f) // 0.001f 은 보정
            {
                onPreserve = true;
            }
        }
    }

    NPC_Controller findTarget()
    {
        float minDis = float.MaxValue;
        int targetIndex = -1;
        for (int i = 0; i < entityManger.npc_list.Count; i++)
        {
            float npcDis = Vector3.Distance(transform.position, entityManger.npc_list[i].transform.position); // npc와 거리
            if (npcDis < range)
            {
                if (npcDis < minDis)
                {
                    minDis = npcDis;
                    targetIndex = i;
                }
            }
        }

        if (targetIndex == -1)
            return null;

        return entityManger.npc_list[targetIndex];
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (onPreserve)
        {
            time = 0;
            float health_bar_size = (float)health / maxHealth;
            lastHealthSize = new Vector3(health_bar_size, 1, 1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.gray;

        if (target)
            Gizmos.DrawLine(transform.position, target.transform.position);
    }
}
