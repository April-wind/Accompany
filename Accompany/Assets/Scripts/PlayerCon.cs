﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    #region private
    /*人物移动速度*/
    [SerializeField]
    private Vector2 moveSpeed;
    
    /*上一帧的速度*/
    private float lastSpeedX;
    private float lastSpeedY;
    
    /*键盘响应*/
    private float horizontal;
    private float vertical;
    
    /*按键状态*/
    enum MoveDir
    {
        none,
        Up,
        Down,
        Left,
        Right
    }
    /*默认*/
    private MoveDir _moveDir = MoveDir.none;

    /*速度因子 X和Y方向*/
    [SerializeField]
    private float speedFactorX;
    [SerializeField]
    private float speedFactorY;

    /*刚体*/
    private Rigidbody2D rb;
    /*动画*/
    private Animator _animator;
    #endregion

    #region const
    private const KeyCode Up = KeyCode.UpArrow;
    private const KeyCode W = KeyCode.W;
    private const KeyCode Down = KeyCode.DownArrow;
    private const KeyCode S = KeyCode.S;
    private const KeyCode Left = KeyCode.LeftArrow;
    private const KeyCode A = KeyCode.A;
    private const KeyCode Right = KeyCode.RightArrow;
    private const KeyCode D = KeyCode.D;
    #endregion
    
    #region public
    /*人物移动速度*/
    public Vector2 MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
   
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        _animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //逐一检测响应
        CheckInputKey();
        
        //水平方向键盘响应
        if(_moveDir == MoveDir.Left || _moveDir == MoveDir.Right)
            horizontal = Input.GetAxisRaw("Horizontal");
        //竖直方向键盘响应
        if(_moveDir == MoveDir.Up || _moveDir == MoveDir.Down)
            vertical = Input.GetAxisRaw("Vertical");
        
        
        //速度 x和y方向的矢量和 
        moveSpeed = Vector2.right * (horizontal) * speedFactorX + Vector2.up * (vertical) * speedFactorY;

        //若这一帧有速度
        if (Mathf.Abs(moveSpeed.x) > Mathf.Epsilon || Mathf.Abs(moveSpeed.y) > Mathf.Epsilon)
        {
            lastSpeedX = moveSpeed.x;
            lastSpeedY = moveSpeed.y;
        }
        
        //设置动画参数
        _animator.SetFloat("Horizontal", horizontal);    
        _animator.SetFloat("Vertical",vertical);
        _animator.SetFloat("Speed",moveSpeed.sqrMagnitude);
        _animator.SetFloat("LastSpeedX",lastSpeedX);
        _animator.SetFloat("LastSpeedY",lastSpeedY);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime);
    }
    
    /// <summary>
    /// 检测键盘输入
    /// </summary>
    void CheckInputKey()
    {
        //检测单一输入
        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                ChangeKeyPressState(kcode,true);
            }

            if (Input.GetKeyUp(kcode))
            {
                ChangeKeyPressState(kcode, false);
            }
        }
    }

    /// <summary>
    /// 检测按键状态
    /// </summary>
    /// <param name="kcode"></param>
    /// <param name="isPress"></param>
    void ChangeKeyPressState(KeyCode kcode,bool message)
    {
        //横向移动
        if (kcode == A || kcode == Left || kcode == D || kcode == Right)
        {
            if (message)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = 0;
            }
            else
            {
                horizontal = 0;
            }
        }
        //纵向移动
        if (kcode == W || kcode == Up || kcode == S || kcode == Down)
        {
            if (message)
            {
                vertical = Input.GetAxisRaw("Vertical");
                horizontal = 0;
            }
            else
            {
                vertical = 0;
            }
        }
    }
}
