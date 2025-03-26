using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float moveSpeed; // Variable for character movement speed
    float speedX, speedY;
    Rigidbody2D rb; // RigidBody variable

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        // Prevent player movement if the shop is open
        if (ShopInteraction.isShopOpen)
        {
            return; // Don't update movement if the shop is open
        }
        // Calculations that adjust the players X and Y position based on movement
        // Inputs and current move speed
        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.velocity = new Vector2(speedX, speedY);
    }
}
