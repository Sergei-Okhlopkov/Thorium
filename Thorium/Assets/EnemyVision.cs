using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    private float radius;

    [SerializeField]
    [Range(0, 360)]
    private float angle;

    private GameObject playerRef;

    [SerializeField]
    private LayerMask targetMask;

    [SerializeField]
    private LayerMask obstaclesMask;

    [SerializeField]
    private float moveSpeed;

    
    [SerializeField]
    public Vector2 RotationSide { get; set; }
    
    public bool CanSeePlayer { get; set; }

    public Vector2 DirectionVector{ get; set; }

    private CircleCollider2D visionCollider;
    private SpriteRenderer spriteRenderer;

    public enum ViewDirection
    {
        Up,
        Down,
        Right,
        Left
    };

    [SerializeField]
    private ViewDirection viewDirection;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        visionCollider = gameObject.GetComponent<CircleCollider2D>();
        radius = visionCollider.radius;
        playerRef = GameObject.Find("Player");
    }
    private void Start()
    {
        StartCoroutine(FOVRoutine());

        switch (viewDirection)
        {
            case ViewDirection.Up: RotationSide = Vector2.up; break;
            case ViewDirection.Down: RotationSide = Vector2.down; break;
            case ViewDirection.Left: RotationSide = Vector2.left; break;
            case ViewDirection.Right: RotationSide = Vector2.right; break;
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            CheckFieldView();
        }
    }

    private void CheckFieldView()
    {
        Vector2 point = transform.position;
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(point, radius, targetMask);

        //if guard has alredy seen you, he will follow you
        if (CanSeePlayer)
        {
            if (rangeChecks.Length == 0) // player has been escaped
            {
                RotationSide = (playerRef.transform.position - transform.position).normalized;
                CanSeePlayer = false;
            }
            else
            {
                Pursuit(rangeChecks);
            }
            return;
        }

        if (rangeChecks.Length != 0)
        {
            Pursuit(rangeChecks);
        }
        else if (CanSeePlayer)
            CanSeePlayer = false;

    }

    private void Pursuit(Collider2D[] rangeChecks)
    {
        Transform target = rangeChecks[0].transform;
        Vector2 directionToTarget = (target.position - transform.position).normalized;
        DirectionVector = directionToTarget;

        if (CanSeePlayer) return;

        if (Vector2.Angle(RotationSide, DirectionVector) < angle / 2)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (!Physics2D.Raycast(transform.position, DirectionVector, radius, obstaclesMask))
            {
                CanSeePlayer = true;
            } 
            else
            { 
                CanSeePlayer = false;
            }
        }
        else CanSeePlayer = false;
    }
}
