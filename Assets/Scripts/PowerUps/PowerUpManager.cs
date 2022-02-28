using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = System.Random;

public class PowerUpManager : MonoBehaviour
{
    private int numPowerUpOptions = 3;
    private int[] generatorList;
    private PowerUpType[] powerUpSelectionList;
    private bool includeNone = false;
    public List<AbstractFiringPowerUp> firingPowerUps;
    public List<AbstractBulletPowerUp> bulletPowerUps;

    private Weapon _weapon; 

    // Start is called before the first frame update
    void Start()
    {
        generatorList = Enumerable.Range(0, System.Enum.GetNames(typeof(PowerUpType)).Length).ToArray();
        powerUpSelectionList = new PowerUpType[numPowerUpOptions];
        firingPowerUps = new List<AbstractFiringPowerUp>();
    }

    // we generate powerups randomly by shuffling a premade list
    private void generatePowerUps()
    {
        shuffleGeneratorList();
        for (int i = 0; i < 3; i++)
        {
            powerUpSelectionList[i] = (PowerUpType)generatorList[i];

            // if we don't want none, we take the next index instead
            if(!includeNone && generatorList[i] == 0)
            {
                powerUpSelectionList[i] = (PowerUpType)generatorList[3];
            }
        }

        // uncomment to see what powerups are generated
        Debug.Log(powerUpSelectionList[0]);
        Debug.Log(powerUpSelectionList[1]);
        Debug.Log(powerUpSelectionList[2]);

    }

    // the knuth shuffle
    private void shuffleGeneratorList()
    {
        Random rand = new Random();
        int n = generatorList.Length;
        while (n > 1)
        {
            int k = rand.Next(n--);
            int temp = generatorList[n];
            generatorList[n] = generatorList[k];
            generatorList[k] = temp;
        }
    }

    // call this method to present the power ups
    public void presentPowerUps()
    {

        generatePowerUps();
        StartCoroutine(waitForSelection());

        // TODO: UI - display power up screen to player
        //  the three powerups generated are in powerUpSelectionList
    }

    // apply stat powerups directly, or add non stat ones to a powerup list
    private void applyPowerUp(PowerUpType type)
    {
        // AbstractPowerUp powerUp = Globals.PowerUpDictionary[type];
        _weapon = GameObject.FindWithTag("Player").GetComponent<Weapon>();

        switch(Globals.PowerUpClassDictionary[type])
        {
            case PowerUpClass.STAT:
                AbstractStatPowerUp statPowerUp = Globals.StatPowerUpDictionary[type];
                statPowerUp.applyPowerUp(_weapon);
                break;
            case PowerUpClass.FIRING:
                _weapon.firingBehavior[type] = true;
                AbstractFiringPowerUp firingPowerUp = Globals.FiringPowerUpDictionary[type];
                firingPowerUps.Add(firingPowerUp);
                firingPowerUps = firingPowerUps.OrderBy(powerUp => powerUp.sortOrder).ToList();
                Debug.Log("Sorted powerups");
                break;
            case PowerUpClass.BULLET:
                // TODO: implement bullet powerups
                AbstractBulletPowerUp bulletPowerUp = Globals.BulletPowerUpDictionary[type];
                bulletPowerUps.Add(bulletPowerUp);
                break;
        }
    }

    IEnumerator waitForSelection()
    {
        PowerUpType selection = PowerUpType.NONE;
        Time.timeScale = 0;
        while(selection == PowerUpType.NONE)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)){
                selection = powerUpSelectionList[0];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)){
                selection = powerUpSelectionList[1];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)){
                selection = powerUpSelectionList[2];
            }
            yield return null;
        }
        applyPowerUp(selection);
        Time.timeScale = 1;
    }
}
