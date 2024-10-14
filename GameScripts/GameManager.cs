using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent playPlayerTurn, playEnemyTurn, playPlayerRestTurn, playPlayerAttackTurn, onPlayerLoss, onPlayerGameWin, onBossStart;
    public UnityEvent<string> onPlayerVictory, onEnemyChange;
    public UnityEvent<int> onCountdown, onNextWave;
    [SerializeField] private GameObject[] enemies; // Corresponds to waveNum
    [SerializeField] private GameObject boss;

    private Slime enemy;
    private Slime player;

    private Team enemyTeam;
    private Team playerTeam;

    private int maxWave;
    private int waveNum;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Slime>();
        enemy = player.enemySlime;

        maxWave = 3;
        waveNum = -1;
        StartNextWave();
    }

    public void PlayEnemyTurn()
    {
        if (enemy.isDead)
        {
            Debug.Log("Victory!");

            if (waveNum >= maxWave)
            {
                Debug.Log("Game's done!");
                onPlayerGameWin?.Invoke();
                PlayerPrefs.SetInt("Win", 1); // 0 - did not win, 1 - won
                return;
            }
            foreach (Transform transform in enemies[waveNum].transform)
            {
                if (transform.gameObject.tag != "Recruitable")
                {
                    transform.gameObject.SetActive(false);
                }
            }
            Slime newMember = GameObject.FindGameObjectWithTag("Recruitable").GetComponent<Slime>();

            onPlayerVictory?.Invoke(newMember.slimeName);
            return;
        }

        playEnemyTurn?.Invoke();
    }

    public void PlayPlayerTurn()
    {
        if (player.isDead)
        {
            Debug.Log("LLLL");
            onPlayerLoss?.Invoke();
            return;
        }

        playPlayerTurn?.Invoke();
    }

    public void PlayPlayerAttackTurn()
    {
        playPlayerAttackTurn?.Invoke();
    }

    public void PlayPlayerRestTurn()
    {
        playPlayerRestTurn?.Invoke();
    }

    // Execute after eating/recruiting a team member and waiting a bit
    public void StartNextWave()
    {
        // Get main enemy + enemy team from a template
        // Re-set enemySlime value
        // Set up main enemy and team
        // Make the enemies walk in
        // Rebind event for enemy turn to the new enemy battler
        // playEnemyTurn.AddListener();
        if (waveNum >= 0) {
            
            Destroy(enemies[waveNum]);
        }

        waveNum += 1;

        if (waveNum >= maxWave)
        {
            Debug.Log("BOSS TIME");
            boss.SetActive(true);
            onBossStart?.Invoke();
        }
        else
        {
            enemies[waveNum].SetActive(true);
        }

        playEnemyTurn.RemoveAllListeners();

        playerTeam = player.gameObject.GetComponent<Team>();
        playerTeam.PlaceTeamMembers();

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        player.ResetVitals();
        yield return new WaitForSeconds(1f);
        onNextWave?.Invoke(waveNum + 1);
        onCountdown?.Invoke(3);
        yield return new WaitForSeconds(1f);
        onCountdown?.Invoke(2);
        yield return new WaitForSeconds(1f);
        onCountdown?.Invoke(1);
        yield return new WaitForSeconds(1f);
        onCountdown?.Invoke(0);
        yield return new WaitForSeconds(1f);
        onCountdown?.Invoke(-1);
        player.ResetVitals();

        enemy = player.enemySlime;
        enemy.ResetVitals();

        enemyTeam = enemy.gameObject.GetComponent<Team>();

        enemyTeam.PlaceTeamMembers();
        if (waveNum != 0)
        {
            playEnemyTurn.AddListener(enemy.GetComponent<Battler>().PlayTurn);
        }

        onEnemyChange?.Invoke(enemy.slimeName);
        PlayPlayerTurn();
    }

    public void StartNextScene()
    {
        waveNum += 1;
    }
}
