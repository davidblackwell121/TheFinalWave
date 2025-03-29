using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour
{
    public float moveSpeed = 5f; // Variable for character movement speed (adjustable)
    public float originalSpeed; // Matches the original move speed of 5f
    float speedX, speedY;

    public bool hasFireBlast = false; // Flag to check if the player has fire blast powerup

    Rigidbody2D rb; // RigidBody variable

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = moveSpeed;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        // Method is called when the shop is closed and the player presses space
        if (!ShopInteraction.isShopOpen && Input.GetKeyDown(KeyCode.Space) && hasFireBlast)
        {
            ActivateFireBlast(); // Activate FireBlast if the player has it
        }

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

    // Method to activate the FireBlast power-up when available
    private void ActivateFireBlast()
    {
        Debug.Log("FireBlast Activated!"); // Log for testing

        // Disable the use of fire blast after use (must be repurchased)
        hasFireBlast = false;
    }

    // A function that boosts move speed for the duration of the buff
    // using a coroutine, allowing a value to be updated each frame within Unity
    public void BoostSpeed(float duration, float speedMultiplier)
    {
        StartCoroutine(SpeedBoostCoroutine(duration, speedMultiplier));
    }

    private IEnumerator SpeedBoostCoroutine(float duration, float speedMultiplier)
    {
        // Log when the speed boost starts
        Debug.Log("Move Speed Boost Activated!");

        moveSpeed *= speedMultiplier; // Speed multiplied during buff duration

        float timeRemaining = duration;

        while (timeRemaining > 0)
        {
            // Wait for 1 second before decreasing the buff timer instead of running continuously
            yield return new WaitForSeconds(1f);

            // If the shop UI is open, pauses the buff timer
            while (ShopInteraction.isShopOpen)
            {
                yield return null; // Wait until the shop UI is closed
            }

            // Decrease the remaining time by 1 second
            timeRemaining -= 1f;
        }

        moveSpeed = originalSpeed; // Changes the movespeed back to the original speed

        // Log when the speed boost ends
        Debug.Log("Move Speed Boost Ended.");
    }
}
