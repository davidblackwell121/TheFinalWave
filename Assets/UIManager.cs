using UnityEngine;
using UnityEngine.UI; // Required for UI related variables
using TMPro; // Required for TextMeshPro variables

public class UIManager : MonoBehaviour
{
    public Image healthBarFill; // Reference to the health bar
    public TextMeshProUGUI waveText; // Reference to wave counter
    public TextMeshProUGUI coinAmountText; // Reference to the players coin amount

    private int waveNumber = 1; // Sets the initial wave count to 1

    public GameObject coin;
    private int coinAmount = 9999; // Player's initial coin amount (placebo value)

    public void UpdateWaveCounter(int wave)
    {
        // Updates the waveNumber variable and updates the wave text on UI to match
        waveNumber = wave;
        waveText.text = "Wave: " + waveNumber;
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        healthBarFill.fillAmount = healthPercentage;
    }

    // Updates the coin amount text on the player's UI
    public void UpdateCoinAmountText()
    {
        coinAmountText.text = coinAmount.ToString();
    }

    // Deduct coins when a purchase is made from the shop
    public bool DeductCoins(int amount)
    {
        if (coinAmount >= amount)
        {
            coinAmount -= amount;
            UpdateCoinAmountText(); // Update the UI text with the new coin amount
            return true; // Successfully deducted coins
        }
        else
        {
            Debug.Log("Not enough coins!"); // Display error if not enough coins
            return false; // Not enough coins to purchase
        }
    }
}
