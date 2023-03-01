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

    private void Start()
    {
        SetAnimatorRotation(enemyVision.RotationSide);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyVision.CanSeePlayer)
        {
            SetAnimatorRotation(enemyVision.DirectionVector);
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

    private void SetAnimatorRotation(Vector2 rotationVector)
    {
        animator.SetFloat(horizontal, rotationVector.x);
        animator.SetFloat(vertical, rotationVector.y);
        animator.SetFloat(speed, rotationVector.magnitude);
        animator.SetBool(isMoving, enemyVision.CanSeePlayer);
    }

}
