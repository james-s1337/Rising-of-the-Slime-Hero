using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlimeUIStats : MonoBehaviour
{
    [SerializeField] private Image playerHealthBar;
    private float barX = 245;
    [SerializeField] private Image playerEnergyBar;

    [SerializeField] private Image enemyHealthBar;
    [SerializeField] private Image enemyEnergyBar;

    [SerializeField] private TextMeshProUGUI enemyName1;
    [SerializeField] private TextMeshProUGUI enemyName2;

    public void UpdatePlayerHealthBar(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0, 1);
        playerHealthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barX * percentage);
    }

    public void UpdatePlayerEnergyBar(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0, 1);
        playerEnergyBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barX * percentage);
    }

    public void UpdateEnemyHealthBar(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0, 1);
        enemyHealthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barX * percentage);
    }

    public void UpdateEnemyEnergyBar(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0, 1);
        enemyEnergyBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barX * percentage);
    }

    public void UpdateEnemyName(string newName)
    {
        enemyName1.text = newName;
        enemyName2.text = newName;
    }

    public void ResetEnemyStatBars()
    {
        enemyHealthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barX);
        enemyEnergyBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barX);

        enemyName1.text = "---";
        enemyName2.text = "---";
    }
}
