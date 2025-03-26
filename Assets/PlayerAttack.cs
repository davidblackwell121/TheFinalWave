using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform firePoint; // Represents the fire point (point of origin for projectile)
    public GameObject projectile; // Projectile prefab to be instantiated

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
            projectileScript.SetDirection(direction);
        }
    }
}
