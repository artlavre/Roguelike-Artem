using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamage;

    private Animator anim;
    private Transform target;
    private bool skipMove;

    public AudioClip enemyAtack1;
    public AudioClip enemyAtack2;
    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if(Mathf.Abs(target.position.x - transform.position.x)< float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        anim.SetTrigger("enemyAtack");
        SoundManager.instance.RandomizeSfx(enemyAtack1,enemyAtack2);
        hitPlayer.LoseFood(playerDamage);
    }
}
