using System;
using System.Collections;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    /*
     * Speed : 고정된 속도
     * Power : 점차 줄어드는 속도
    */

    [Header("Move")]
    [SerializeField] 
     private float moveSpeed = 5;
    private int moveDistance = 0;
    public float runAccelAmount = 1.2f;
    public float runDeccelAmount = 1.2f;
    public float maxFallSpeed = 5;

    [Space]
    [Header("Run")]
    [SerializeField]
     private float runSpeed = 8;
    [HideInInspector]
     public bool isRunning = false;

    [Space]
    [Header("Crouch")]
    [SerializeField]
     private float crouchSpeed = 3;
    [HideInInspector]
     public bool isCrouch = false;

    [Space]
    [Header("Slid")]
    [SerializeField]
     private float slidPower = 7;
    [SerializeField]
     private float slidDeceleration = 0.98f; // 감속량
    //[SerializeField]
    // private float slidCoolTime = 0.2f;
    //private float slidTime = 0.0f;

    [Header("Jump")]
    [HideInInspector]
     public bool isSliding = false;

    [Space]
    [Header("Dash")]
    [SerializeField]
     private float dashPower = 3;
    [SerializeField]
     private float dashDeceleration = 0.98f; // 감속량
    [SerializeField]
     private float dashCoolTime = 1.2f; // 대쉬 쿨타임
    private float dashTime = 0.0f;
    [SerializeField]
     private float dashGravityScale = 1.0f; // 대쉬 중 중력 크기
    private float defaultGravityScale; // 기본 중력 크기
    [HideInInspector]
     public bool isDashing = false;

    [Space]
    [Header("Jump")]
    [SerializeField]
     private float jumpPower = 5;
    [SerializeField]
     private float groundCheckRayDis = 0.6f;
    [SerializeField]
     private Vector2 groundRayBoxSize = new Vector2(0.8f, 0.01f);
    public float coyoteTime;
    public float jumpInputBufferTime;
    public float isJumpCutGravityMult = 1;
    public float fallGravityMult = 1;

    private float LastOnGroundTime;
    private float LastPressedJumpTime;
    private bool isJumping = false;
    public bool isJumpCut = false;


    [Space]
    [Header("Parkour")]
    [SerializeField]
     private Vector2 handPos;
    [SerializeField]
     private Vector2 parkourRayBoxSize = new Vector2(0.3f, 0.3f);
    [SerializeField]
     private float parkourMoveSpeed = 5;
    [SerializeField]
     private float parkourRayDis = 0.6f;
    [SerializeField] 
     private Collider2D playerCollider;
    [HideInInspector]
     public bool isParkour = false;

    private float speed; // 현재 이동 속도
    private int lookAtDistance = 1; // 보고 있는 방향 : (왼쪽)-1 / (중앙)0 / (오른쪽)1
    [HideInInspector]
     public bool onGround = true; // 플랫폼 위에 서있는가 or 파쿠프 중인가
    private bool stayPlatform = false; // 플랫폼 안에 머물러 있는가

    // 
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 기본 속도 지정
        speed = moveSpeed;

        defaultGravityScale = rb.gravityScale;
    }

    // 설정 값에 따라 일정한 간격으로 호출
    // 물리 효과가 적용된(Rigidbody) 오브젝트를 조정할 때 사용됩니다(Update는 불규칙한 호출임으로 물리엔진 충돌검사 등이 제대로 안될 수 있음)
    void FixedUpdate()
    {
        //rb.linearVelocity = new Vector2(lookAtDistance * speed, rb.linearVelocityY);
        float targetSpeed = moveDistance * speed;
        targetSpeed = Mathf.Lerp(rb.linearVelocityX, targetSpeed, 1);

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;

        float speedDif = targetSpeed - rb.linearVelocityX;
        float movement = speedDif * accelRate;

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

        if (!stayPlatform && !isParkour)
        {
            // 플랫폼 위에 있는 지
            RaycastHit2D rayHit = Physics2D.BoxCast(
                transform.position, groundRayBoxSize,
                0, Vector3.down,
                groundCheckRayDis,
                LayerMask.GetMask("Platform")
            );

            if (rayHit.collider != null)
            {
                if (rb.linearVelocityY <= 0)
                {
                    onGround = true;
                    isJumping = false;
                    isJumpCut = false;
                    LastOnGroundTime = coyoteTime;
                }
            }
            else onGround = false;
        }

        if (rb.linearVelocityY <= 0)
        {
            RaycastHit2D rayHit = Physics2D.BoxCast(
                transform.position + new Vector3(handPos.x * lookAtDistance, handPos.y, 0), parkourRayBoxSize,
                0, new Vector3(lookAtDistance, 0, 0),
                parkourRayDis,
                LayerMask.GetMask("ParkourPlatform")
            );

            if (rayHit.collider != null)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                playerCollider.isTrigger = true;
                isParkour = true;
                onGround = true;
                isJumpCut = false;
                isJumping = false;
                LastOnGroundTime = coyoteTime;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                isParkour = false;
            }
        }

        if (!stayPlatform && !isParkour)
            playerCollider.isTrigger = false;

        stayPlatform = false; // FixedUpdate() 이후 OnTriggerStay2D()를 실행하기 때문에 플랫폼에 겹쳐 있는지 확인가능
    }

    private void Update()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;

        // move
        moveDistance = 0;
        if (Input.GetKey(KeyList.move_left))
        {
            lookAtDistance = -1;
            moveDistance -= 1;
        }
        if (Input.GetKey(KeyList.move_right))
        {
            lookAtDistance = 1;
            moveDistance += 1;
        }

        // run
        isRunning = Input.GetKey(KeyList.run);

        // crouch
        if (Input.GetKeyDown(KeyList.crouch))
        {
            if (!isParkour) {
                if (moveDistance != 0)
                {
                    if (isRunning && !isSliding) // 슬라이딩
                    {
                        speed = slidPower;

                        isSliding = true;
                    }
                }

                if (!isSliding)
                {
                    isCrouch = true;
                }
            }
            else // 파쿠르 플랫폼에서 내려오기
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                isParkour = false;
                onGround = false;
            }
        }
        else if (!Input.GetKey(KeyList.crouch))
        {
            isCrouch = false;
        }

        // jump
        if (Input.GetKeyDown(KeyList.jump))
        {
            LastPressedJumpTime = jumpInputBufferTime;
            if (onGround) // jump
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                onGround = false;
            }
        }
        if(Input.GetKeyUp(KeyList.jump))
        {
            if (CanisJumpCut()) isJumpCut = true;
        }

        // dash
        if (dashTime >= dashCoolTime) // [수정] - 점프 + 대쉬 같이 쓰면 대각선 상단으로 올라감
        {
            if (Input.GetKeyDown(KeyList.dash))
            {
                dashTime = 0.0f;

                //rb.gravityScale = dashGravityScale;
                speed = dashPower;

                isDashing = true;
            }
        }
        else
        {
            dashTime += Time.deltaTime;
        }

        // 행동에 따른 속도 감속 or 지정
        if (isDashing)         
        {
            moveDistance = lookAtDistance;
            speed *= dashDeceleration;
            if (speed <= moveSpeed)
            {
                isDashing = false;
                //rb.gravityScale = defaultGravityScale;
            }
        }
        else if (isSliding)
        {
            moveDistance = lookAtDistance;
            speed *= slidDeceleration;
            if (speed <= crouchSpeed + 0.02) // 0.02는 보정
            {
                isSliding = false;
                isCrouch = true;
            }
        }        
        else if (moveDistance == 0) speed = 0;
        else if (isParkour)         speed = parkourMoveSpeed;
        else if (isCrouch)          speed = crouchSpeed;   
        else if (isRunning)         speed = runSpeed;
        else                        speed = moveSpeed;

        if (CanJump() && LastPressedJumpTime > 0)
        {
            Jump();
        }

        #region GRAVITY
        if (isJumpCut)
        {
            //Higher gravity if jump button released
            SetGravityScale(defaultGravityScale * isJumpCutGravityMult);
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -maxFallSpeed));
        }
        else if (rb.linearVelocityY < 0)
        {
            //Higher gravity if falling
            SetGravityScale(defaultGravityScale * fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -maxFallSpeed));
        }
        else
        {
            //Default gravity if standing on a platform or moving upwards
            SetGravityScale(defaultGravityScale);
        }
        #endregion
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            stayPlatform = true;
        }
    }

    void Jump()
    {
        isJumping = true;
        isJumpCut = false;

        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        //떨어지는 경우 적용되는 힘을 증가시킵니다. 
        //이것은 항상 같은 양만큼 점프하는 것처럼 느낄 수 있다는 것을 의미합니다.
        //(플레이어의 Y 속도를 미리 0으로 설정하면 같은 효과가 있을 가능성이 있지만, 저는 이것이 더 우아하다고 생각합니다 :D)
        float force = jumpPower;
        if (rb.linearVelocityY < 0)
            force -= rb.linearVelocityY;

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    bool CanJump()
    {
        return LastOnGroundTime > 0 && !isJumping;
    }
    bool CanisJumpCut()
    {
        return isJumping && rb.linearVelocityY > 0;
    }

    public void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }

    //IEnumerator Dash()
    //{
    //    isDashing = true;

    //    float defaultGravity = rb.gravityScale;

    //    rb.gravityScale = dashGravityScale;
    //    //rb.linearVelocity = new Vector2(lookAtDistance * dashPower, 0);

    //    //yield return new WaitForSeconds(dashingTime);

    //    //rb.gravityScale = defaultGravity;

    //    //isDashing = false;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // 점프 ray
        Gizmos.DrawRay(transform.position, new Vector3(0, -groundCheckRayDis, 0));
        Gizmos.DrawWireCube(transform.position + new Vector3(0, -groundCheckRayDis, 0), groundRayBoxSize);

        Gizmos.DrawRay(transform.position + new Vector3(handPos.x * lookAtDistance, handPos.y, 0), new Vector3(lookAtDistance * parkourRayDis, 0, 0));
        Gizmos.DrawWireCube(transform.position + new Vector3(handPos.x * lookAtDistance + parkourRayDis * lookAtDistance, handPos.y, 0), parkourRayBoxSize);
    }
}
