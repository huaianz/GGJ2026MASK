//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(BoxCollider2D))]
//public class Player : MonoBehaviour
//{
//    [Header("�ƶ�����")]
//    [SerializeField] private float moveSpeed = 8f;//ˮƽ�ƶ�����ٶ�
//    [SerializeField] private float acceleration = 15f;//���ٶ�
//    [SerializeField] private float deceleration = 10f;//���ٵ�ֹͣ�����ʣ�ֵԽ��ͣԽ�죩
//    [SerializeField] private float airControlMultiplier = 0.5f;//���п���ϵ��
//    [Header("��Ծ����")]
//    [SerializeField] private float jumpForce = 15f;//��Ծ��ʼ��        
//    [SerializeField] private float jumpCutMultiplier = 0.5f; //�ɿ���Ծ��ʱ��Ծ�߶ȵļ�Сϵ��
//    [SerializeField] private float coyoteTime = 0.1f;//��غ�������Ծ��ʱ�䴰�� 
//    [SerializeField] private float jumpBufferTime = 0.1f;//��ǰ����Ծ���Ļ���ʱ��  
//    [SerializeField] private int maxAirJumps = 1; //��������Ծ����,Ŀǰ�Ƕ�����
//    [Header("����Ч��")]
//    [SerializeField] private float maxFallSpeed = -25f;//��������ٶ�
//    [SerializeField] private float fastFallMultiplier = 1.5f;//�����½�����������
//    [Header("������")]
//    [SerializeField] private LayerMask groundLayer;
//    [SerializeField] private float groundCheckRadius = 0.2f;
//    [SerializeField] private Vector2 groundCheckOffset;

//    //���
//    private Rigidbody2D rb;
//    private BoxCollider2D col;
//    private Animator animator;
//    private SpriteRenderer spriteRenderer;

//    //����״̬
//    private float horizontalInput;
//    private bool isJumpPressed;
//    private bool isJumpHeld;
//    private bool isJumpReleased;
//    private bool isFastFalling;

//    //��Ծ��ؼ�ʱ��
//    private float coyoteTimeCounter;   
//    private float jumpBufferCounter;   
//    private int airJumpCounter;

//    public bool IsGround;
//    public bool IsFacingRight;
//    public float CurrentMoveSpeed => Mathf.Abs(rb.velocity.x);



//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        col = GetComponent<BoxCollider2D>();
//        animator = GetComponent<Animator>();
//        spriteRenderer = GetComponent<SpriteRenderer>();

//        //������������
//        rb.gravityScale = 3f;
//        rb.freezeRotation = true;
//    }

//    private void Update()
//    {
//        GetInput();                     
//        HandleCoyoteTime();      
//        HandleJumpBuffer();        
//        HandleJumpCut();          
//        HandleFastFall();              
//        UpdateAnimations();      
//        FlipSprite();

//    }

//    void FixedUpdate()
//    {
//        CheckGround();                 
//        HandleMovement();          
//        HandleJump();           
//        ClampFallSpeed();            
//    }
//    private void GetInput()
//    {
//        horizontalInput = Input.GetAxisRaw("Horizontal");

//        isJumpPressed = Input.GetButtonDown("Jump");   
//        isJumpHeld = Input.GetButton("Jump");     
//        isJumpReleased = Input.GetButtonUp("Jump");

//        if (isJumpPressed)
//        {
//            jumpBufferCounter = jumpBufferTime;
//        }
//        isFastFalling = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
//    }

//    // ����ɫ�Ƿ�Ӵ�����
//    void CheckGround()
//    {
//        // ��������λ�ã���ɫλ�� + ƫ����
//        Vector2 checkPosition = (Vector2)transform.position + groundCheckOffset;

//        // �ڼ������һ��Բ�Σ�����Ƿ������ͼ����ײ
//        IsGround = Physics2D.OverlapCircle(checkPosition, groundCheckRadius, groundLayer);

//        // ����ڵ�����û�������˶�����ֹ��Ծʱ������Ϊ���棩
//        if (IsGround && rb.velocity.y <= 0)
//        {
//            airJumpCounter = 0;                // ���ÿ�����Ծ����
//            coyoteTimeCounter = coyoteTime;    // ��������ʱ��
//        }
//    }
//    // ����ˮƽ�ƶ�
//    void HandleMovement()
//    {
//        float targetSpeed = horizontalInput * moveSpeed;

//        float speedDiff = targetSpeed - rb.velocity.x;

//        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

//        accelRate = IsGround ? accelRate : accelRate * airControlMultiplier;

//        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.5f) * Mathf.Sign(speedDiff);

//        rb.AddForce(movement * Vector2.right);
//    }

//    // ������Ծ�߼�
//    void HandleJump()
//    {
//        bool canNormalJump = IsGround && jumpBufferCounter > 0;
//        bool canCoyoteJump = coyoteTimeCounter > 0 && jumpBufferCounter > 0;
//        bool canAirJump = !IsGround && airJumpCounter < maxAirJumps && jumpBufferCounter > 0;

//        if (canNormalJump || canCoyoteJump || canAirJump)
//        {
//            rb.velocity = new Vector2(rb.velocity.x, 0);
//            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

//            jumpBufferCounter = 0;
//            coyoteTimeCounter = 0;

//            if (!IsGround)
//            {
//                airJumpCounter++;
//            }
//        }
//    }

//    // ��������ʱ�䣨��غ�������Ծ��ʱ�䴰�ڣ�
//    void HandleCoyoteTime()
//    {
//        if (IsGround)
//        {
//            coyoteTimeCounter = coyoteTime;
//        }
//        else
//        {
//            coyoteTimeCounter -= Time.deltaTime;
//        }
//    }

//    // ������Ծ���壨��ǰ����Ծ���Ļ��壩
//    void HandleJumpBuffer()
//    {
//        if (jumpBufferCounter > 0)
//        {
//            jumpBufferCounter -= Time.deltaTime;
//        }
//    }

//    // ������Ծ��;ȡ�����ɿ���Ծ��ʱ������Ծ�߶ȣ�
//    void HandleJumpCut()
//    {
//        if (isJumpReleased && rb.velocity.y > 0)
//        {
//            // ����Y���ٶȣ�ʹ��Ծ�߶Ƚ���
//            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
//        }
//    }

//    // �����������䣨��ס�·����ʱ�������䣩
//    void HandleFastFall()
//    {
//        if (isFastFalling && !IsGround && rb.velocity.y < 0)
//        {
//            rb.gravityScale = 3f * fastFallMultiplier;
//        }
//        else
//        {
//            rb.gravityScale = 3f;
//        }
//    }

//    // ������������ٶȣ���ֹ������죩
//    void ClampFallSpeed()
//    {
//        if (rb.velocity.y < maxFallSpeed)
//        {
//            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
//        }
//    }

//    // �����ƶ�����ת��ɫ
//    void FlipSprite()
//    {
//        if (horizontalInput > 0 && !IsFacingRight)
//        {
//            Flip();
//        }
//        else if (horizontalInput < 0 && IsFacingRight)
//        {
//            Flip();
//        }
//    }

//        // ��ת��ɫ��ʵ��ʵ��
//    void Flip()
//    {
//        IsFacingRight = !IsFacingRight;

//        Vector3 scale = transform.localScale;
//        scale.x *= -1;
//        transform.localScale = scale;
//    }

//    // ���¶����������������Animator�����
//    void UpdateAnimations()
//    {
//        if (animator == null) return;

//        // ���ö���������
//        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); // ˮƽ�ٶȾ���ֵ
//        animator.SetBool("IsGrounded", IsGround);           // �Ƿ��ڵ���
//        animator.SetFloat("VerticalVelocity", rb.velocity.y); // ��ֱ�ٶȣ���=��������=�½���
//    }

//    // ��Unity�༭����Scene��ͼ�л��Ƶ���������
//    void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.green;

//        Vector2 checkPosition = Application.isPlaying ?
//            (Vector2)transform.position + groundCheckOffset :
//            (Vector2)transform.position + groundCheckOffset;

//        // ����һ���߿�Բ��ʾ��ⷶΧ
//        Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
//    }

//}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    [Header("�ƶ�����")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float airControlMultiplier = 0.5f;

    [Header("��Ծ����")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private int maxAirJumps = 1;

    [Header("����Ч��")]
    [SerializeField] private float maxFallSpeed = -25f;
    [SerializeField] private float fastFallMultiplier = 1.5f;

    [Header("������")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector2 groundCheckOffset;

    [Header("��������")]
    [SerializeField] private float walkAnimationThreshold = 0.1f;

    //���
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    //����״̬
    private float horizontalInput;
    private bool isJumpPressed;
    private bool isJumpHeld;
    private bool isJumpReleased;
    private bool isFastFalling;

    //��Ծ��ؼ�ʱ��
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private int airJumpCounter;

    //�������
    private bool isJumping = false;
    private bool wasGrounded = true;

    // ��������
    public bool IsGround { get; set; }
    public bool IsFacingRight { get; set; }
    public float CurrentMoveSpeed => Mathf.Abs(rb.velocity.x);

    //�л�����ʱ�����ý�ɫ�ɲ���
    private bool inputDisable;


    [Header("�ƶ�����")]
    [SerializeField] private bool canMoveLeft = true;    // ���������ƶ�
    [SerializeField] private bool canMoveRight = true;  // ��ֹ�����ƶ�
    [SerializeField] private bool canJump = true;       // ��ֹ��Ծ


    [Header("全局暂停")]
    [SerializeField] private bool isMovementPaused = false;  // 全局暂停标记

    /// <summary>
    /// 暂停所有移动（UI弹出时调用）
    /// </summary>
    public void PauseMovement()
    {
        isMovementPaused = true;
        // 立即停止当前移动 - 完全停止所有速度
        horizontalInput = 0;
        rb.velocity = Vector2.zero;
        // 清除所有跳跃输入状态
        isJumpPressed = false;
        isJumpHeld = false;
        isJumpReleased = false;
        jumpBufferCounter = 0;
        Debug.Log("[Player] 移动已暂停");
    }

    /// <summary>
    /// 恢复所有移动（所有UI关闭时调用）
    /// </summary>
    public void ResumeMovement()
    {
        isMovementPaused = false;
    }

    /// <summary>
    /// 检查是否处于暂停状态
    /// </summary>
    public bool IsMovementPaused()
    {
        return isMovementPaused;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 3f;
        rb.freezeRotation = true;

        // 初始化朝向：确保IsFacingRight与scale.x一致
        IsFacingRight = true;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;


    }

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.Movement += SetMovementRestrictions;

        
        EventHandler.OnAllUIClosed += OnAllUIClosed;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.Movement -= SetMovementRestrictions;

        
        EventHandler.OnAllUIClosed -= OnAllUIClosed;
    }

 
    private void OnAllUIClosed()
    {
        ResumeMovement();
    }
    // =========================================================

    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        IsFacingRight = true; // 修复：统一设为朝右
                              // 确保scale.x为正值（朝右）
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x); // 强制朝右
        transform.localScale = scale;
    }

    private void OnAfterSceneLoadEvent()
    {
        inputDisable = false;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        inputDisable = true;
    }

    private void Update()
    {
        if (inputDisable == false)
        {
            GetInput();
        }
        HandleCoyoteTime();
        HandleJumpBuffer();
        HandleJumpCut();
        HandleFastFall();
        UpdateAnimations();
        FlipSprite();
    }

    void FixedUpdate()
    {
        CheckGround();
        // UI打开时完全禁止移动和跳跃
        if (!inputDisable && !isMovementPaused)
        {
            HandleMovement();
            HandleJump();
        }
        else if (isMovementPaused)
        {
            // 暂停期间持续清零速度，防止外力影响
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        ClampFallSpeed();
    }

    #region ���봦��
    private void GetInput()
    {
        if (isMovementPaused)
        {
            horizontalInput = 0;
            isJumpPressed = false;
            isJumpHeld = false;
            isJumpReleased = false;
            return;
        }
        // ��ȡԭʼ����
        float rawInput = Input.GetAxisRaw("Horizontal");

        // Ӧ���ƶ�����
        if (!canMoveRight && rawInput > 0)  // ��ֹ����
        {
            horizontalInput = 0;
        }
        else if (!canMoveLeft && rawInput < 0)  // ��ֹ����
        {
            horizontalInput = 0;
        }
        else
        {
            horizontalInput = rawInput;
        }



        // ��Ծ����
        if (canJump)
        {
            isJumpPressed = Input.GetButtonDown("Jump");
            isJumpHeld = Input.GetButton("Jump");
            isJumpReleased = Input.GetButtonUp("Jump");
        }
        else
        {
            isJumpPressed = false;
            isJumpHeld = false;
            isJumpReleased = false;
        }

        if (isJumpPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        isFastFalling = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    }
    #endregion

    #region ������
    void CheckGround()
    {
        Vector2 checkPosition = (Vector2)transform.position + groundCheckOffset;

        bool wasGround = IsGround;
        IsGround = Physics2D.OverlapCircle(checkPosition, groundCheckRadius, groundLayer);

        if (IsGround && rb.velocity.y <= 0)
        {
            airJumpCounter = 0;
            coyoteTimeCounter = coyoteTime;

            if (!wasGround)
            {
                OnLand();
            }
        }

        wasGrounded = IsGround;
    }

    void OnLand()
    {
        isJumping = false;
        // ���ʱ�������¶���״̬
        if (animator != null)
        {
            animator.SetBool("IsJumping", false);
        }
    }
    #endregion

    #region �ƶ�����
    void HandleMovement()
    {
        float targetSpeed = horizontalInput * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        accelRate = IsGround ? accelRate : accelRate * airControlMultiplier;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.5f) * Mathf.Sign(speedDiff);

        rb.AddForce(movement * Vector2.right);
    }
    #endregion

    #region ��Ծ����
    void HandleJump()
    {
        bool canNormalJump = IsGround && jumpBufferCounter > 0;
        bool canCoyoteJump = coyoteTimeCounter > 0 && jumpBufferCounter > 0;
        bool canAirJump = !IsGround && airJumpCounter < maxAirJumps && jumpBufferCounter > 0;

        if (canNormalJump || canCoyoteJump || canAirJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
            isJumping = true;

            // �ؼ����������� Jump ��������
            if (animator != null)
            {
                animator.SetBool("IsJumping", true);
            }

            if (!IsGround)
            {
                airJumpCounter++;
            }
        }
    }

    void HandleCoyoteTime()
    {
        if (IsGround)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void HandleJumpBuffer()
    {
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void HandleJumpCut()
    {
        if (isJumpReleased && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }
    }
    #endregion

    #region ����Ч��
    void HandleFastFall()
    {
        if (isFastFalling && !IsGround && rb.velocity.y < 0)
        {
            rb.gravityScale = 3f * fastFallMultiplier;
        }
        else
        {
            rb.gravityScale = 3f;
        }
    }

    void ClampFallSpeed()
    {
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }
    #endregion

    #region ����ת
    void FlipSprite()
    {
        if (horizontalInput > 0 && !IsFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && IsFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (IsFacingRight ? 1 : -1);
        transform.localScale = scale;
    }
    #endregion

    #region �������ƣ�֧��ֱ��ת����
    void UpdateAnimations()
    {
        if (animator == null) return;

        // ���õ���״̬����
        animator.SetBool("IsGrounded", IsGround);

        // ����״̬���ö���
        if (IsGround)
        {
            // �ڵ��棺�ж��Ƿ�Ӧ�ò���Walk����
            bool hasHorizontalInput = Mathf.Abs(horizontalInput) > walkAnimationThreshold;

            if (hasHorizontalInput)
            {
                // �����룺����Walk����
                animator.SetBool("IsWalking", true);
                // ע�⣺���ﲻ���� IsJumping = false����Ϊ�����������ᴦ��
            }
            else
            {
                // �����룺����Idle����
                animator.SetBool("IsWalking", false);
            }

            // ����ʱȷ�� IsJumping = false������������Ծ��
            if (!isJumping)
            {
                animator.SetBool("IsJumping", false);
            }
        }
        else
        {
            // �ڿ��У�ȷ������Jump����
            // Walk �������Զ�ֹͣ����Ϊ IsJumping = true
            animator.SetBool("IsJumping", true);
        }
    }
    #endregion

    //���ؿ��İ�����ֹ
    #region �ƶ����Ʒ���
    /// <summary>
    /// ��������ƶ�����Ծ������
    /// </summary>
    /// <param name="allowLeft">���������ƶ�</param>
    /// <param name="allowRight">���������ƶ�</param>
    /// <param name="allowJump">������Ծ</param>
    public void SetMovementRestrictions(bool allowLeft = true, bool allowRight = false, bool allowJump = false)
    {
        canMoveLeft = allowLeft;
        canMoveRight = allowRight;
        canJump = allowJump;

        // �����ǰ���뷽�򱻽�ֹ����������
        if (!allowRight && horizontalInput > 0)
        {
            horizontalInput = 0;
        }
        if (!allowLeft && horizontalInput < 0)
        {
            horizontalInput = 0;
        }
    }
    #endregion
}