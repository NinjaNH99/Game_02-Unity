﻿using System;
using UnityEngine;

public class Ball : MonoSingleton<Ball>
{
    public bool firstBallLanded;
    public bool allBallLanded;

    private Vector2 lastColPosL;
    private Vector2 lastColPosR;
    private Vector2 landingPosition;

    private Rigidbody2D rigid;

    private float currentSpawnY;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0f;
        rigid.simulated = true;
    }

    private void Start()
    {
        firstBallLanded = false;
        lastColPosL = lastColPosR = Vector2.zero;
        transform.position = new Vector2(0, -1.48f);  
    }

    public void SendBallInDirection(Vector2 dir)
    {
        firstBallLanded = false;
        rigid.simulated = true;
        rigid.AddForce(dir * GameController.speed, ForceMode2D.Impulse);
    }

    private void TouchFloor()
    {
        firstBallLanded = true;
        GameController.Instance.IsAllBallLanded(true);
        rigid.velocity = Vector2.zero;
        rigid.simulated = false;
        // Reload position Y
        transform.position = new Vector2(transform.position.x, -1.48f);
    }

    private void IfIsBlocked()
    {
        if (Math.Round(lastColPosL.y , 1) == Math.Round(lastColPosR.y , 1))
        {
            rigid.gravityScale = 0.02f;
        }
        else
            rigid.gravityScale = 0;
    }

    public void CollectBall()
    {
        GameController.amountBalls++;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(Tags.Floor))
        {
            TouchFloor();
        }
        if (coll.gameObject.CompareTag(Tags.Square))
        {
            coll.transform.parent.GetComponent<Block>().ReceiveHit();
        }
        if (coll.gameObject.CompareTag(Tags.Wall))
            lastColPosL = gameObject.transform.position;
        if (coll.gameObject.CompareTag(Tags.WallR))
        {
            lastColPosR = gameObject.transform.position;
            IfIsBlocked();
        }
    }

}
