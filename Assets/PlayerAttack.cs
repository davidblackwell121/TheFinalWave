using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public Transform firePoint; // Represents the fire point (point of origin for projectile)
    public GameObject projectile; // Projectile prefab to be instantiated
    private float currentDamage = 1f; // Default damage with no buffs
    private float boostedDamage = 2f; // Boosted damage value with buffs

    // Update is called once per frame
    void Update()
    {
        // Prevents projectile firing if the shop is open
        if (ShopInteraction.isShopOpen)
        {
            return; // Don't fire projectiles if the shop is open
        }

        // Checks if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            FireAtCursor(); // Calls this method to fire the projectile at the cursor
        }
    }

    // Method to fire a projectile towards the cursors position upon clicking
    void FireAtCursor()
    {
        // Gets the mouse position in the world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculates the direction from the fire point to the mouse position
        Vector2 direction = (mousePosition - (Vector2)firePoint.position).normalized;

        // Instantiates a new projectile at the fire point (our character)
        GameObject newProjectile = Instantiate(projectile, firePoint.position, Quaternion.LookRotation(Vector3.forward, direction));

        // Gets the projectile script attached to the new projectile and sets
        // The direction of movement
        Projectile projectileScript = newProjectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction, currentDamage);
        }
    }
    
    // A function that boosts damage for the duration of the buff
    // using a coroutine, allowing a value to be updated each frame within Unity
    public void BoostDamage(float duration)
    {
        StartCoroutine(DamageBoostCoroutine(duration));
    }

    private IEnumerator DamageBoostCoroutine(float duration)
    {
        // Log when the damage boost starts
        Debug.Log("Damage Boost Activated!");

        currentDamage = boostedDamage; // Damage is boosted while buffed

        float timeRemaining = duration;

        while (timeRemaining > 0)
        {
            // Wait for 1 second before decreasing the buff timer instead of running continuously
            yield return new WaitForSeconds(1f);

            // If the shop UI is open, pause the timer
            while (ShopInteraction.isShopOpen)
            {
                yield return null; // Wait until the shop UI is closed
            }

            // Decrease the remaining time of the buff by 1 second
            timeRemaining -= 1f;
        }

        currentDamage = 1f; // Reset damage after the buff duration expires

        // Log when the damage boost ends
        Debug.Log("Damage Boost Ended.");
    }
}
