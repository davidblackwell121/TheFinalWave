using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopInteraction : MonoBehaviour
{
    public GameObject PlayerUI; // Reference to the PLayer UI
    public GameObject ShopUI; // Reference to the shop UI
    public GameObject ShopPanel; // Reference to the shop panel
    public GameObject InteractionText; // Reference the shop interaction text
    public TextMeshProUGUI InvalidPurchaseText;

    // References for the buttons in the shop
    public Button ShopFireBlast;
    public Button ShopMoveSpeed;
    public Button ShopDamage;
    public Button CloseButton;

    // References for the Player scripts to be modified by buffs
    private PlayerCtrl playerCtrl;
    private PlayerAttack playerAttack;

    private Coroutine errorCoroutine; // Reference to track coroutine error message

    private bool isNearShop = false; // Check if player is near the shop
    public static bool isShopOpen = false; // Tracks the shop state globally

    [System.Obsolete]
    void Start()
    {
        playerCtrl = FindObjectOfType<PlayerCtrl>();
        playerAttack = FindObjectOfType<PlayerAttack>();

        // Add listeners for the shop button clicks
        ShopMoveSpeed.onClick.AddListener(() => playerCtrl.PurchaseMoveSpeed());
        ShopDamage.onClick.AddListener(() => playerCtrl.PurchaseDamage());
        ShopFireBlast.onClick.AddListener(() => playerCtrl.PurchaseFireBlast());

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

        // Initially hide the error message when the shop is opened
        InvalidPurchaseText.gameObject.SetActive(false);
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

                // Initially hide the error message when shop opens
                InvalidPurchaseText.gameObject.SetActive(false);
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
        TogglePlayerUI(false); // Ensure Player UI is visible again
    }

    public void ShowErrorMessage(string message)
    {
        // Stop any previous running coroutine
        if (errorCoroutine != null)
        {
            StopCoroutine(errorCoroutine);
        }

        // Show the error message and set the text
        InvalidPurchaseText.gameObject.SetActive(true);
        InvalidPurchaseText.text = message;

        // Start the coroutine to hide the error message after a delay (2 seconds)
        errorCoroutine = StartCoroutine(HideErrorMessage());
    }

    private IEnumerator HideErrorMessage()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Hide the error message after 2 seconds
        InvalidPurchaseText.gameObject.SetActive(false);
    }
}
