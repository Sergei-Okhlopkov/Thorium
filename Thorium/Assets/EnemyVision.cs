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

    
    public bool CanSeePlayer { get; set; }

    public Vector2 DirectionVector{ get; set; }

    private CircleCollider2D visionCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        visionCollider = gameObject.GetComponent<CircleCollider2D>();
        radius = visionCollider.radius;
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
        

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            DirectionVector = directionToTarget;

            if (Vector2.Angle(transform.right, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstaclesMask))
                    CanSeePlayer = true;
                else
                    CanSeePlayer = false;
            }
            else
                CanSeePlayer = false;
        }
        else if (CanSeePlayer)
            CanSeePlayer = false;

    }
}
