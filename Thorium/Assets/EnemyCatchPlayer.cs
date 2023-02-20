using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCatchPlayer : MonoBehaviour
{
    private EnemyVision enemyVision;
    private float moveSpeed = 4f;
    private Rigidbody2D rb;
    private Transform rotation;
    private Animator animator;

    private string horizontal = "Horizontal";
    private string vertical = "Vertical";
    private string speed = "Speed";
    private string isMoving = "IsMoving";

    void Awake()
    {
        animator = transform.GetComponent<Animator>();
        enemyVision = transform.GetComponent<EnemyVision>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        rotation = transform.Find("Rotation").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyVision.CanSeePlayer)
        {
            animator.SetFloat(horizontal, enemyVision.DirectionVector.x);
            animator.SetFloat(vertical, enemyVision.DirectionVector.y);
            animator.SetFloat(speed, enemyVision.DirectionVector.magnitude);
            animator.SetBool(isMoving, enemyVision.CanSeePlayer);
        }
        else
        {
            animator.SetFloat(speed, 0);
            animator.SetBool(isMoving, enemyVision.CanSeePlayer);
        }

    }

    void FixedUpdate()
    {
        if (enemyVision.CanSeePlayer)
        {

            rb.MovePosition(rb.position + enemyVision.DirectionVector * moveSpeed * Time.fixedDeltaTime);
           
        }
        else
        {
            StopMove();
        }
    }

    private void StopMove()
    {
        rb.velocity = Vector2.zero;
    }

   
}
