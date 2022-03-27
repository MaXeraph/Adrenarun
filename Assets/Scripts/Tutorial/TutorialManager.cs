using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Vector3 respawn;
    private GameObject player;
    private Stats playerStats;
    private GameObject enemy;
    private Stats enemyStats;
    ArtilleryAttackBehaviour temp = new ArtilleryAttackBehaviour(EntityType.ENEMY, durability:100000);

    [SerializeField]
    private GameObject pool;
    [SerializeField]
    private GameObject endPortal;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y <= -15 || playerStats.currentHealth <= 30) {
            Respawn(player);
        }
        
            
    }

    public void DisplayControls(GameObject hideControls, GameObject showControls) {
        showControls.SetActive(true);
        hideControls.SetActive(false);
    }

    public void HandleEvents(int stage){
        if (stage == 2) {
            ShootStage();
        }
        if (stage == 3) {
            SlowmoStage();
        }
        if (stage == 4) {
            HealingStage();
        }
        if (stage == 5) {
            PowerupStage();
        }
        if (stage == 6) {
            FinalStage();
        }
        if (stage == 7) {
            ClearPowerups();
        }
    }

    private void ShootStage() {
        CreateDummyEnemy(new Vector3(0, 1, 0));
        CreateDummyEnemy(new Vector3(10, 1, 0));
        CreateDummyEnemy(new Vector3(-10, 1, 0));
    }

    private void CreateDummyEnemy(Vector3 enemyPosition) {
        enemy = EnemyFactory.Instance.CreateEnemy(enemyPosition, EnemyType.TURRET);
        enemy.GetComponent<EnemyBehaviour>().enabled = false;
        enemy.transform.rotation = Quaternion.Euler(0, 180, 0);

    }

    private void SlowmoStage() {
        pool.GetComponent<ThermitePoolMono>().Initialize(temp);
        pool.SetActive(true);
    }

    private void HealingStage() {
        pool.SetActive(false);
        SpawnHealingPills();
    }

    private void SpawnHealingPills() {
        for (int i=-15; i<=15; i += 5) {
            for (int j=185; j<=195; j += 5) {
                Vector3 position = new Vector3(i, 1, j);
                HealingPill.SpawnPill(position);
            }
        }
    }

    private void PowerupStage() {
        player.GetComponent<PowerUpManager>().presentPowerUps();
        SpawnEnemies();
    }

    private void SpawnEnemies() {
        EnemyFactory.Instance.CreateEnemy(new Vector3(13, 1, 295), EnemyType.TURRET);
        EnemyFactory.Instance.CreateEnemy(new Vector3(-13, 1, 295), EnemyType.TURRET);
        EnemyFactory.Instance.CreateEnemy(new Vector3(0, 1, 287), EnemyType.RANGED);
    }

    private void FinalStage() {
        endPortal.SetActive(true); 
    }

    private void ClearPowerups() {
        Globals.TransitionPowerUpDictionary = new Dictionary<PowerUpType, int>()
        {
            {PowerUpType.DAMAGE, 0},
            {PowerUpType.FIRERATE, 0 },
            {PowerUpType.RELOADSPD, 0 },
            {PowerUpType.CLIPSIZE, 0 },
            {PowerUpType.ADRENALIN, 0 },
            {PowerUpType.SHOTGUN, 0 },
            {PowerUpType.REPEATER, 0 }
        };
    }

    private void Respawn(GameObject player) {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = respawn;
        player.GetComponent<CharacterController>().enabled = true;
        playerStats.currentHealth = playerStats.maxHealth;
    }
}
