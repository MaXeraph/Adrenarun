using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private int numPowerUps = 3;
    private PowerUpType[] powerUpList = new PowerUpType[3];
    private GameObject _player;
    private Weapon _weapon;

    void Start(){
        _player = GameObject.FindWithTag("Player");
    }

    private void generatePowerUps(){

        // generates 3 power up types
        for (int i = 0; i < numPowerUps; i++){
            PowerUpType powerUp = (PowerUpType)Random.Range(1, 5);

            // TODO: check for repeats
            powerUpList[i] = powerUp;
        }

        Debug.Log("Generated powerups");
        Debug.Log("power up 1: " + powerUpList[0]);
        Debug.Log("power up 2: " + powerUpList[1]);
        Debug.Log("power up 3: " + powerUpList[2]);

    }
    
    // Call to present the player with the choice of powerups
    public void presentPowerUps()
    {
        Time.timeScale = 0;
        generatePowerUps();
        StartCoroutine(waitForSelection());
        
        // TODO: Display the powerups to the player using UI

    }

    IEnumerator waitForSelection()
    {
        PowerUpType selection = PowerUpType.NONE;
        while (selection == PowerUpType.NONE){
            if (Input.GetKeyDown(KeyCode.Alpha1)){
                selection = powerUpList[0];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)){
                selection = powerUpList[1];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)){
                selection = powerUpList[2];
            }
            yield return null;
        }
        PowerUpProcessor.applyPowerUp(selection, _player);
        Time.timeScale = 1;
    }
}
