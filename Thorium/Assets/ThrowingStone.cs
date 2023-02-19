using UnityEngine;

public class ThrowingStone : MonoBehaviour
{
    public bool isLanded = false;
    private float landingTime = 1f;
    private float speed = 5f;
    private Rigidbody2D rb;
    private CircleCollider2D stoneCollider;

    private int counter = 0;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        stoneCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        isLanded = false;
        rb.AddForce(transform.up * speed, ForceMode2D.Force);
        Invoke("Landing", landingTime); // Langing() will be called after landingTime (seconds)
    }

    private void Landing()
    {
        isLanded = true;
        counter++;
        transform.SetParent(null);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        stoneCollider.isTrigger = true;
    }

    public void prepareToThrow()
    {
        rb.isKinematic = false;
        stoneCollider.isTrigger = false;
    }
}
