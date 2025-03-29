using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject fireBlastPrefab; // Reference to the fire blast prefab
    public float moveSpeed = 5f; // Variable for character movement speed (adjustable)
    public float originalSpeed; // Matches the original move speed of 5f
    float speedX, speedY;

    public bool hasFireBlast = false; // Flag to check if the player has fire blast powerup

    UIManager uiManager; // Reference to the UIManager script
    Rigidbody2D rb; // RigidBody variable

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = moveSpeed;

        // Attempt to find the UIManager in the scene
        uiManager = FindObjectOfType<UIManager>();
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
        rb.linearVelocity = new Vector2(speedX, speedY);
    }

    // Method to activate the FireBlast power-up when available
    private void ActivateFireBlast()
    {
        if (hasFireBlast)
        {
            Debug.Log("FireBlast Activated!"); // Log for testing

            // Used to fire multiple prefabs in random directions
            for (int i = 0; i < 10; i++)
            {
                // Instantiate the FireBlast prefab
                GameObject fireBlast = Instantiate(fireBlastPrefab, transform.position, Quaternion.identity);

                // Get a random direction in degrees (between 0 and 360)
                float randomAngle = Random.Range(0f, 360f);

                // Convert the angle to a direction vector and math stuff
                Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));

                // Get the Rigidbody2D component of the FireBlast and apply velocity
                Rigidbody2D rb = fireBlast.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = randomDirection * 10f;  // Apply velocity in a random direction
                }

                // Destroy the fire blast after some time (e.g., 1 second)
                Destroy(fireBlast, 1f);
            }

            // Disable the use of fire blast after use (must be repurchased)
            hasFireBlast = false;
        }
    }

    // Purchase MoveSpeed (called when the player clicks the MoveSpeed button in the shop)
    public void PurchaseMoveSpeed()
    {
        if (uiManager.DeductCoins(100)) // Deduct 100 coins
        {
            Debug.Log("Move speed boost purchased!");
        }
    }

    // Purchase Damage (called when the player clicks the Damage button in the shop)
    public void PurchaseDamage()
    {
        if (uiManager.DeductCoins(200)) // Deduct 200 coins
        {
            Debug.Log("Damage boost purchased!");
        }
    }

    // Purchase FireBlast (called when the player clicks the FireBlast button in the shop)
    [System.Obsolete]
    public void PurchaseFireBlast()
    {
        if (hasFireBlast)
        {
            // Show the error message in the Shop UI
            ShopInteraction shopInteraction = FindObjectOfType<ShopInteraction>();
            shopInteraction.ShowErrorMessage("Fire Blast Limit Reached!");
            return; // Prevent purchasing the powerup if already owned
        }

        if (uiManager.DeductCoins(400)) // Deduct 400 coins
        {
            hasFireBlast = true; // Grant the player the FireBlast power-up
            Debug.Log("Fire Blast power-up purchased!");
        }
    }

    // A function that boosts move speed for the duration of the buff
    // using a coroutine, allowing a value to be updated each frame within Unity
    public void BoostSpeed(float duration, float speedMultiplier)
    {
        StartCoroutine(SpeedBoostCoroutine(duration, speedMultiplier));
    }

    private IEnumerator SpeedBoostCoroutine(float duration, float speedMultiplier)
    {
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
