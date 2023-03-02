using System.Collections;
using UnityEngine;

public class EnemyVision : Unit
{
    [SerializeField]
    private float radius = 5f;

    [SerializeField]
    [Range(0, 360)]
    private float angle;

    [HideInInspector]
    public GameObject playerRefObj;
    [HideInInspector]
    public Player playerRef;

    [SerializeField]
    private LayerMask targetMask;

    [SerializeField]
    private LayerMask obstaclesMask;
    
    [SerializeField]
    public Vector2 RotationSide { get; set; }
    
    public bool CanSeePlayer { get; set; }

    public Vector2 DirectionVector{ get; set; }

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
        playerRefObj = GameObject.Find("Player");
        playerRef = playerRefObj.GetComponent<Player>();

        switch (viewDirection)
        {
            case ViewDirection.Up: RotationSide = Vector2.up; break;
            case ViewDirection.Down: RotationSide = Vector2.down; break;
            case ViewDirection.Left: RotationSide = Vector2.left; break;
            case ViewDirection.Right: RotationSide = Vector2.right; break;
        }
    }
    private void Start()
    {
        StartCoroutine(FOVRoutine());
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
                RotationSide = (playerRefObj.transform.position - transform.position).normalized;
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

            if (!Physics2D.Raycast(transform.position, DirectionVector, radius, obstaclesMask) && playerRef.isHiding == false)
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
