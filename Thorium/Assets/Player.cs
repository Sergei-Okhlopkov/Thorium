using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private bool moving;
    private GameObject throwingStone;

    #region Variables from Unity

    private string horizontal = "Horizontal";
    private string vertical = "Vertical";
    private string speed = "Speed";
    private string isMoving = "IsMoving";

    #endregion

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        throwingStone = transform.Find("ThrowingStone").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)) ThrowStone();

        movement.x = Input.GetAxisRaw(horizontal);
        movement.y = Input.GetAxisRaw(vertical);

       
        if (movement != Vector2.zero)
        {
            animator.SetFloat(horizontal, movement.x);
            animator.SetFloat(vertical, movement.y);
            animator.SetFloat(speed, movement.magnitude);
            if (!moving)
            {
                moving = true;
                animator.SetBool(isMoving, moving);
            }

        }
        else
        {
            if (moving)
            {
                moving = false;
                animator.SetFloat(speed, movement.magnitude);
                animator.SetBool(isMoving, moving);
            }
           
        }

       
        
       
    }

    void FixedUpdate()
    {
        if (moving)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            StopMove();
        }
    }

    private void ThrowStone()
    {
        if (throwingStone == null) return;
        throwingStone.SetActive(true);
        throwingStone = null;
    }

    private void GetStone(Transform stone)
    {
        if (stone == null || stone.gameObject.GetComponent<ThrowingStone>().isLanded == false) return;
        stone.SetParent(transform);
        stone.position = transform.position;// + new Vector3(movement.x, movement.y, 0f);
        throwingStone = stone.gameObject;
        throwingStone.gameObject.GetComponent<ThrowingStone>().prepareToThrow();
        throwingStone.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Throwing") && !throwingStone) GetStone(trigger.transform);
    }

    private void StopMove()
    {
        rb.velocity = Vector2.zero;
    }
}
