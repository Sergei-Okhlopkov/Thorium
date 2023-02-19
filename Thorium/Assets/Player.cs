using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private GameObject throwingStone;

    #region Variables from Unity

    private string horizontal = "Horizontal";
    private string vertical = "Vertical";
    private string speed = "Speed";

    #endregion

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        throwingStone = transform.Find("Stone").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)) ThrowStone();

        movement.x = Input.GetAxisRaw(horizontal);
        movement.y = Input.GetAxisRaw(vertical);

        animator.SetFloat(horizontal, movement.x);
        animator.SetFloat(vertical, movement.y);
        animator.SetFloat(speed, movement.magnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        //rb.AddForce(movement * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
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
}
