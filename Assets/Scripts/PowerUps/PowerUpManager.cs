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
    public List<AbstractBulletPowerUp> bulletPowerUps;

	private List<PowerUpType> highTierPowerUps;
	private List<PowerUpType> midTierPowerUps;
	private List<PowerUpType> lowTierPowerUps;

    private Weapon _weapon;

    // Start is called before the first frame update
    void Start()
    {
        generatorList = Enumerable.Range(0, System.Enum.GetNames(typeof(PowerUpType)).Length).ToArray();
        powerUpSelectionList = new PowerUpType[numPowerUpOptions];
		initializePowerUpLists();
    }

	private void initializePowerUpLists()
	{
		// Current internal power up tier list:

		// High (0): Shotgun, Repeater
		// Mid (1): Exploding, Piercing, Damage, Fire Rate
		// Low (2): ClipSize, ReloadSpeed, Homing, Adrenalin

		highTierPowerUps  = new List<PowerUpType>{PowerUpType.SHOTGUN, 
												PowerUpType.REPEATER,
												PowerUpType.EXPLODING};

		midTierPowerUps = new List<PowerUpType>{PowerUpType.DAMAGE, 
												PowerUpType.PIERCING,
												PowerUpType.FIRERATE};

		lowTierPowerUps = new List<PowerUpType>{PowerUpType.CLIPSIZE, 
												PowerUpType.RELOADSPD, 
												PowerUpType.DASHCD,
												PowerUpType.ADRENALIN};
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

    private void generateTieredPowerUps(float waveTime, int waveNumber)
    {
		// TODO: find thresholds
		// either through a calculation based on the enemies and which wave it is,
		// or some static number that works
        float highThreshold = 20f;
		float midThreshold = 40f;
		
		// check which tier distribution for powerups we will use
		// 0 = high, 1 = mid, 2 = low
		int powerUpPool = waveTime < highThreshold ? 0 : waveTime < midThreshold ? 1 : 2;
		
		for (int i = 0; i < 3; i++)
		{
			// randomly generate the powerup tier.
			// then, randomly select a powerup within that tier.
			PowerUpTier powerUpTier = findPowerUpTierFromPool(powerUpPool);
			PowerUpType powerUp = generatePowerUpFromTier(powerUpTier);
			powerUpSelectionList[i] = powerUp;

			// remove the option we just chose so we don't generate duplicates
			if (powerUpTier == PowerUpTier.HIGH)
			{
				for (int j = 0; j < highTierPowerUps.Count; j++)
				{
					if (highTierPowerUps[j] == powerUp)
					{
						highTierPowerUps.Remove(highTierPowerUps[j]);
						break;
					}
				}
			}
			else if (powerUpTier == PowerUpTier.MID)
			{	
				for (int j = 0; j < midTierPowerUps.Count; j++)
				{
					if (midTierPowerUps[j] == powerUp)
					{
						midTierPowerUps.Remove(midTierPowerUps[j]);
						break;
					}
				}
			}
			else 
			{	
				for (int j = 0; j < lowTierPowerUps.Count; j++)
				{
					if (lowTierPowerUps[j] == powerUp)
					{
						lowTierPowerUps.Remove(lowTierPowerUps[j]);
						break;
					}
				}
			}
		}
		// re-initialize the power up lists
		initializePowerUpLists();
    }

	private PowerUpTier findPowerUpTierFromPool(int pool)
	{
		// unityengine is inclusive both bounds
		int randNum = UnityEngine.Random.Range(0, 99);
		PowerUpTier powerUpTier;
		// distribution: h 15 - m 65 - l 20
		if (pool == 0)
		{
			powerUpTier = randNum < 15 ? PowerUpTier.HIGH : randNum < 80 ? PowerUpTier.MID : PowerUpTier.LOW;
		}
		// distribution: h 10 - m 55 - l 35
		else if (pool == 1)
		{
			powerUpTier = randNum < 10 ? PowerUpTier.HIGH : randNum < 65 ? PowerUpTier.MID : PowerUpTier.LOW;
		}
		// distribution: h 5 - m 50 - l 45
		else 
		{
			powerUpTier = randNum < 5 ? PowerUpTier.HIGH : randNum < 55 ? PowerUpTier.MID : PowerUpTier.LOW;
		}
		return powerUpTier;
	}

	private PowerUpType generatePowerUpFromTier(PowerUpTier tier)
	{
		Random randNum = new Random();
		if (tier == PowerUpTier.HIGH && highTierPowerUps.Count > 0)
		{
			return highTierPowerUps[randNum.Next(highTierPowerUps.Count)];
		}
		else if ((tier == PowerUpTier.MID || tier == PowerUpTier.HIGH) && midTierPowerUps.Count > 0)
		{
			return midTierPowerUps[randNum.Next(midTierPowerUps.Count)];
		}
		else
		{
			return lowTierPowerUps[randNum.Next(lowTierPowerUps.Count)];
		}
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
    public void presentPowerUps(float waveTime = 0, int waveNumber = 0)
    {
        generateTieredPowerUps(waveTime, waveNumber);
        UIManager.showPowerups(powerUpSelectionList);
        StartCoroutine(waitForSelection());
    }

    // apply stat powerups directly, or add non stat ones to a powerup list
    public void applyPowerUp(PowerUpType type)
    {
        _weapon = GameObject.FindWithTag("Player").GetComponent<Weapon>();
        UIManager.powerSelection = -1;
        switch(Globals.PowerUpClassDictionary[type])
        {
            case PowerUpClass.STAT:
                AbstractStatPowerUp statPowerUp = Globals.StatPowerUpDictionary[type];
                statPowerUp.applyPowerUp(_weapon);
                break;
            case PowerUpClass.FIRING:
                AbstractFiringPowerUp firingPowerUp = Globals.FiringPowerUpDictionary[type];
                _weapon.firingMods.Add(firingPowerUp);
                _weapon.firingMods = _weapon.firingMods.OrderBy(powerUp => powerUp.sortOrder).ToList();
                break;
            case PowerUpClass.BULLET:
                // TODO: implement bullet powerups
                AbstractBulletPowerUp bulletPowerUp = Globals.BulletPowerUpDictionary[type];
				bulletPowerUp.applyPowerUp(_weapon._attackBehaviour);
                break;
        }
    }

    IEnumerator waitForSelection()
    {
        PowerUpType selection = PowerUpType.NONE;
        Time.timeScale = 0;
        while(selection == PowerUpType.NONE)
        {
            if (UIManager.powerSelection != -1)
            {
                selection = powerUpSelectionList[UIManager.powerSelection];
            }
            yield return null;
        }
		Globals.TransitionPowerUpDictionary[selection] += 1;
        applyPowerUp(selection);
        Time.timeScale = 1;
    }
}
