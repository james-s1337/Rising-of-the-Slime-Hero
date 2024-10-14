using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class SlimeUI : MonoBehaviour
{
    [SerializeField] private GameObject playerTurnMenu;
    [SerializeField] private GameObject recruitMenu;

    [SerializeField] private TextMeshProUGUI recruitMenuPrompt;
    [SerializeField] private TextMeshProUGUI recruitMenuPromptBack;

    [SerializeField] private TextMeshProUGUI waveCounter;
    [SerializeField] private TextMeshProUGUI waveCounterBack;

    [SerializeField] private TextMeshProUGUI cd1;
    [SerializeField] private TextMeshProUGUI cd2;

    [Header("Screens")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject victoryScreen;

    public UnityEvent startNextWave;

    public void ShowPlayerTurnMenu()
    {
        playerTurnMenu.active = true;
    }

    public void HidePlayerTurnMenu()
    {
        playerTurnMenu.active = false;
    }

    public void ShowRecruitMenu(string name)
    {
        recruitMenuPrompt.text = name + " wants to join your team...";
        recruitMenuPromptBack.text = recruitMenuPrompt.text;
        recruitMenu.active = true;
    }

    public void HideRecruitMenu()
    {
        recruitMenu.active = false;
        startNextWave?.Invoke();
    }

    public void UpdateWaveCounter(int num)
    {
        waveCounter.text = "Wave: " + num;
        waveCounterBack.text = waveCounter.text;
    }

    public void UpdateCountdown(int num)
    {
        if (num == -1)
        {
            cd1.gameObject.SetActive(false);
            cd2.gameObject.SetActive(false);
        }
        else
        {
            cd1.text = "" + num;
            cd2.text = "" + num;

            cd1.gameObject.SetActive(true);
            cd2.gameObject.SetActive(true);
        }
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        deathScreen.GetComponent<EndScreen>().Enable();
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
        victoryScreen.GetComponent<EndScreen>().Enable();
    }
}
