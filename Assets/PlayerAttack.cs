using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectile;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireAtCursor();
        }
    }

    void FireAtCursor()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = (mousePosition - (Vector2)firePoint.position).normalized;

        GameObject newProjectile = Instantiate(projectile, firePoint.position, Quaternion.LookRotation(Vector3.forward, direction));

        Projectile projectileScript = newProjectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction);
        }
    }
}
