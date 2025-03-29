using UnityEngine;
using UnityEngine.UI;

public class ShopInteraction : MonoBehaviour
{
    public GameObject ShopUI; // Reference to the shop UI
    public GameObject ShopPanel; // Reference to the shop panel
    public GameObject InteractionText; // Reference the shop interaction text
    private bool isNearShop = false; // Check if player is near the shop

    // References for the buttons in the shop
    public Button ShopFireBlast;
    public Button ShopMoveSpeed;
    public Button ShopDamage;

    // References for the Player scripts to be modified by buffs
    private PlayerCtrl playerCtrl;
    private PlayerAttack playerAttack;

    // Reference to PlayerUI and Shop UI close button
    public GameObject PlayerUI;
    public Button CloseButton; 

    // Tracks the shop state globally
    public static bool isShopOpen = false;

    [System.Obsolete]
    void Start()
    {
        playerCtrl = FindObjectOfType<PlayerCtrl>();
        playerAttack = FindObjectOfType<PlayerAttack>();

        // Find InteractionText inside PlayerUI if not assigned
        if (InteractionText == null && PlayerUI != null)
        {
            InteractionText = PlayerUI.transform.Find("InteractionText").gameObject; // Find InteractionText child inside PlayerUI
        }

        // Assign the close function to the button click
        if (CloseButton != null)
        {
            CloseButton.onClick.AddListener(CloseShop);
        }

        // Creates event listener for the Fire Blast powerup button
        if (ShopFireBlast != null)
        {
            ShopFireBlast.onClick.AddListener(() =>
            {
                playerCtrl.hasFireBlast = true;  // Give the player the FireBlast power-up
            });
        }

        // Creates event listeners for the shop buffs by calling the player scripts
        if (ShopMoveSpeed != null)
        {
            ShopMoveSpeed.onClick.AddListener(() => playerCtrl.BoostSpeed(15f, 1.5f)); // 15s, 1.5x speed
        }

        if (ShopDamage != null)
        {
            ShopDamage.onClick.AddListener(() => playerAttack.BoostDamage(20f)); // 20s, 2x damage boost
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

    // Function to close the shop when the close button is clicked
    public void CloseShop()
    {
        ShopPanel.SetActive(false);
        isShopOpen = false;
        TogglePlayerUI(false); // Ensure PlayerUI is visible again
    }
}
