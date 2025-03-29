using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject fireBlastPrefab; // Reference to the fire blast prefab
    public float moveSpeed = 5f; // Variable for character movement speed (adjustable)
    public float originalSpeed; // Matches the original move speed of 5f
    float speedX, speedY;

    public bool hasFireBlast = false; // Flag to check if the player has fire blast powerup

    private Coroutine speedBoostCoroutine;

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
    [System.Obsolete]
    public void PurchaseMoveSpeed()
    {
        if (uiManager.DeductCoins(100)) // Deduct 100 coins
        {
            Debug.Log("Move speed boost purchased!");
        }
        else
        {
            FindObjectOfType<ShopInteraction>().ShowErrorMessage("Not Enough Coins!");
        }
    }

    // Purchase Damage (called when the player clicks the Damage button in the shop)
    [System.Obsolete]
    public void PurchaseDamage()
    {
        if (uiManager.DeductCoins(200)) // Deduct 200 coins
        {
            Debug.Log("Damage boost purchased!");
        }
        else
        {
            FindObjectOfType<ShopInteraction>().ShowErrorMessage("Not Enough Coins!");
        }
    }

    // Purchase FireBlast (called when the player clicks the FireBlast button in the shop)
    [System.Obsolete]
    public void PurchaseFireBlast()
    {
        if (hasFireBlast)
        { 
            FindObjectOfType<ShopInteraction>().ShowErrorMessage("Fire Blast Limit Reached!");
            return; // Prevent purchasing the powerup if already owned
        }

        bool purchaseSuccess = uiManager.DeductCoins(400);

        if (purchaseSuccess) // Deduct 400 coins when the purchase is successful
        {
            hasFireBlast = true; // Grant the player the FireBlast power-up
            Debug.Log("Fire Blast power-up purchased!");
        }
        else
        {
            // Error message is shown when the powerup can't be purchased
            FindObjectOfType<ShopInteraction>().ShowErrorMessage("Not Enough Coins!");
        }
    }

    // A function that boosts move speed for the duration of the buff
    // using a coroutine, allowing a value to be updated each frame within Unity
    public void BoostSpeed(float duration, float speedMultiplier)
    {
        // Prevents overlapping buffs when purchasing multiple from the shop
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine); // Stop the existing coroutine
            moveSpeed = originalSpeed; // Reset speed before applying the new boost
        }

        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(duration, speedMultiplier));
    }

    private IEnumerator SpeedBoostCoroutine(float duration, float speedMultiplier)
    {
        moveSpeed *= speedMultiplier; // Speed multiplied during buff duration

        while (duration > 0)
        {
            // Wait for 1 second before decreasing the buff timer instead of running continuously
            yield return new WaitForSeconds(1f);

            // If the shop UI is open, pauses the buff timer
            while (ShopInteraction.isShopOpen)
            {
                yield return null; // Wait until the shop UI is closed
            }

            // Decrease the remaining time by 1 second
            duration -= 1f;
        }

        moveSpeed = originalSpeed; // Changes the movespeed back to the original speed
        speedBoostCoroutine = null; // Clear the coroutine reference

        // Log when the speed boost ends
        Debug.Log("Move Speed Boost Ended.");
    }
}
