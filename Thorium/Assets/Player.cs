using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private bool moving;

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
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw(horizontal);
        movement.y = Input.GetAxisRaw(vertical);

       
        if (movement != Vector2.zero)
        //if (movement.x != 0 && movement.y != 0)
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

       
        
        //animator.SetBool(isMoving, moving);
    }

    void FixedUpdate()
    {
        if (moving)
        {
            //Движение. К текущем положению 
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            //rb.AddForce(movement * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }
}
