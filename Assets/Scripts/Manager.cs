using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Concurrent;

public enum gameStatus
{
    next,play,gameover,win
}

public class Manager : Loader<Manager>
{
    [SerializeField]
    int totalWaves = 10;
    [SerializeField]
    TMP_Text totalMoneyLabel;
    [SerializeField]
    TMP_Text currentWave;
    [SerializeField]
    TMP_Text playBtnLabel;
    [SerializeField]
    Button playBtn;
    [SerializeField]
    TMP_Text totalEscapedLabel;

    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    GameObject[] enemies;
    [SerializeField]
    int totalEnemies;
    [SerializeField]
    int enemiesPerSpawn;

    int waveNumber = 1;
    int totalMoney = 10;
    int totalEscaped = 0;
    int roundEscaped = 0;
    int totalKilled = 0;
    int wichEnemiesToSpawn = 0;
    gameStatus currentState = gameStatus.play;

    public List<Enemy> EnemyList = new List<Enemy>();

    const float spawnDelay = 1f;

    public int TotalEscaped
    {
        get { return totalEscaped; }
        set { totalEscaped = value; }
    }
    public int RoundEscaped
    {
        get { return roundEscaped; }
        set { roundEscaped = value; }
    }
    public int TotalKilled
    {
        get { return totalKilled; }
        set { totalKilled = value; }
    }
    public int TotalMoney
    {
        get { return totalMoney; }
        set { totalMoney = value; totalMoneyLabel.text = TotalMoney.ToString(); }
    }

    void Start()
    {
        playBtn.gameObject.SetActive(false);
        ShowMenu();
    }
    private void Update()
    {
        HandleEscape();
    }
    IEnumerator Spawn()
    {
        if(enemiesPerSpawn  > 0 && EnemyList.Count < totalEnemies)
        {
            for(int i = 0; i < enemiesPerSpawn; i++)
            {
                if(EnemyList.Count < totalEnemies)
                {
                    GameObject newEnemy = Instantiate(enemies[0] as GameObject);
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }
    public void UnRegisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    public void DestroyEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }

        EnemyList.Clear();
    }
    public void addMoney(int amount)
    {
        TotalMoney += amount;
    }
    public void subtractMoney(int amount)
    {
        TotalMoney -= amount;
    }
    public void IsWaweOver()
    {
        totalEscapedLabel.text = "Escaped" + totalEscaped + "/ 10";

        if((RoundEscaped + TotalKilled) == totalEnemies)
        {
            SetCurrentGameState();
            ShowMenu();
        }
    }

    public void SetCurrentGameState()
    {
        if(totalEscaped >= 10)
        {
            currentState = gameStatus.gameover;
        }
        else if (waveNumber == 0 && (RoundEscaped + TotalKilled) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= totalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void PlayButtonPressed()
    {
        switch(currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            default:
                totalEnemies = 5;
                TotalEscaped = 0;
                TotalMoney = 10;
                ToweManger.Instance.DestroyAllTowers();
                ToweManger.Instance.RenameTagBuildSite();
                totalMoneyLabel.text = TotalMoney.ToString();
                totalEscapedLabel.text = "Escaped " + totalEscaped + " / 10";
                break;
        }
        DestroyEnemies();
        totalKilled = 0;
        RoundEscaped = 0;
        currentWave.text = "Wave " + (waveNumber + 1);
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
    }
    public void ShowMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLabel.text = "Play Again";
                break;
            case gameStatus.next:
                playBtnLabel.text = "Next wave";
                break;
            case gameStatus.play:
                playBtnLabel.text = "Play game";
                break;
            case gameStatus.win:
                playBtnLabel.text = "Play game";
                break;
        }
        playBtn.gameObject.SetActive(true);
    }
    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToweManger.Instance.DisableDrag();
            ToweManger.Instance.towerBtnPressed = null;
        }
    }
}
