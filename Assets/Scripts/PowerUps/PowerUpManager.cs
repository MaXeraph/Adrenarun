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
    public List<AbstractPowerUp> appliedPowerUps;

    private Weapon _weapon; 

    // Start is called before the first frame update
    void Start()
    {
        generatorList = Enumerable.Range(0, System.Enum.GetNames(typeof(PowerUpType)).Length).ToArray();
        powerUpSelectionList = new PowerUpType[numPowerUpOptions];
        appliedPowerUps = new List<AbstractPowerUp>();
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
        // Debug.Log(powerUpSelectionList[0]);
        // Debug.Log(powerUpSelectionList[1]);
        // Debug.Log(powerUpSelectionList[2]);

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
        AbstractPowerUp powerUp = Globals.StatPowerUpDictionary[type];
        switch (type)
        {
            case PowerUpType.NONE:
                return;
            case PowerUpType.DAMAGE:
                _weapon = GameObject.FindWithTag("Player").GetComponent<Weapon>();
                _weapon._attackBehaviour._damage += powerUp.modifier;
                Debug.Log("Damage PowerUp Applied: " + _weapon._attackBehaviour._damage);
                break;
            case PowerUpType.FIRERATE:
                _weapon = GameObject.FindWithTag("Player").GetComponent<Weapon>();
                _weapon._fireRate += powerUp.modifier;
                Debug.Log("Fire Rate PowerUp Applied: " + _weapon._fireRate);
                break;
            case PowerUpType.RELOADSPD:
                _weapon = GameObject.FindWithTag("Player").GetComponent<Weapon>();
                _weapon._reloadSpeed += powerUp.modifier;
                UIManager.reloadSpeed = _weapon._reloadSpeed;
                Debug.Log("Reload Speed PowerUp Applied: " + _weapon._reloadSpeed);
                break;
            case PowerUpType.CLIPSIZE:
                _weapon = GameObject.FindWithTag("Player").GetComponent<Weapon>();
                _weapon._magazineSize += (int)powerUp.modifier;
                UIManager.AmmoCapacity = _weapon._magazineSize;
                Debug.Log("Clip Size PowerUp Applied: " + _weapon._magazineSize);
                break;                
            case PowerUpType.ADRENALIN:
                SpeedManager.adrenalinModifier += powerUp.modifier;
                Debug.Log("Adrenalin PowerUp Applied: " + SpeedManager.adrenalinModifier);
                break;                
            default:
                appliedPowerUps.Add(powerUp);
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
