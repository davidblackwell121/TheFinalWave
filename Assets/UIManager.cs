using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro variables

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI waveText; // Reference to wave counter
    public Image healthBarFill; // Reference to the health bar

    private int waveNumber = 1;

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
}
