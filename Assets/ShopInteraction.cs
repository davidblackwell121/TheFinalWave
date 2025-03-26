using UnityEngine;

public class ShopInteraction : MonoBehaviour
{
    public GameObject ShopUI; // Reference to the shop UI
    public GameObject ShopPanel; // Reference to the shop panel
    public GameObject InteractionText; // Reference the shop interaction text
    private bool isNearShop = false; // Check if player is near the shop

    // Reference to PlayerUI
    public GameObject PlayerUI;

    // Tracks the shop state globally
    public static bool isShopOpen = false;

    void Start()
    {
        // Find InteractionText inside PlayerUI if not assigned
        if (InteractionText == null && PlayerUI != null)
        {
            InteractionText = PlayerUI.transform.Find("InteractionText").gameObject; // Find InteractionText child inside PlayerUI
        }
    }

    void Update()
    {
        // Show or hide the interaction text
        if (isNearShop)
        {
            if (Input.GetKeyDown(KeyCode.E)) // If the player presses E, open the shop
            {
                ToggleShopUI();
                TogglePlayerUI(ShopPanel.activeSelf);
            }
            InteractionText.SetActive(true); // Show the interaction text
        }
        else
        {
            InteractionText.SetActive(false); // Hide the interaction text when not near the shop
        }
    }

    // Detect when the player enters the shop trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Determines if the player is near the shop and sets the flag to true
        if (other.CompareTag("Player"))
        {
            isNearShop = true;
        }
    }

    // Detect when the player exits the shop trigger area
    private void OnTriggerExit2D(Collider2D other)
    {
        // Determines if the player is not near the shop and sets the flag to false
        if (other.CompareTag("Player"))
        {
            isNearShop = false;
        }
    }

    // Toggle the visibility of the shop UI
    private void ToggleShopUI()
    {
        ShopPanel.SetActive(!ShopPanel.activeSelf); // Toggle visibility of the Shop Panel
        ShopUI.SetActive(true); // Ensure the Shop UI canvas is active
        isShopOpen = ShopPanel.activeSelf; // Update the isShopOpen global flag
    }

    // Toggles the visibility of the player UI
    private void TogglePlayerUI(bool isShopActive)
    {
        PlayerUI.SetActive(!isShopActive); // UI is set to visible when the shop UI is not active
    }
}
