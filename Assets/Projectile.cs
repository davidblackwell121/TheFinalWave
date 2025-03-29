using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectilespeed; // Variable for the projectile speed
    public float lifetime; // Variable to set projectile lifetime
    public float damage = 1f; // Default damage value without buffs

    private new Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction, float damageValue)
    {
        // Applying velocity to the rigidbody2D of the projectile
        rigidbody.linearVelocity = direction * projectilespeed;
        damage = damageValue; // Apply the projectile's damage value

        // Destroy the projectile after a specified time
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks for Terrain tag when projectile collides with terrain
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject); // Destroys (removes) projectile if it hits terrain
        }
    }
}
