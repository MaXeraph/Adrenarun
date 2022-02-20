using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private int numPowerUps = 3;
    private PowerUpContainer[] powerUpList = new PowerUpContainer[3];
    private GameObject _player;

    private void generatePowerUps(){

        // if the containers don't exist, instantiate them
        if(powerUpList[0] == null)
        {
            for (int i = 0; i < numPowerUps; i++){
                PowerUpContainer powerup = new PowerUpContainer();
                powerUpList[i] = powerup;
            }
        }
        // refresh the effect the container holds if they do exist
        else{
            for (int i = 0; i < numPowerUps; i++){
                powerUpList[i].setEffect();
            }
        }
        Debug.Log("Generated powerups");
        Debug.Log("power up 1: " + powerUpList[0].effect);
        Debug.Log("power up 2: " + powerUpList[1].effect);
        Debug.Log("power up 3: " + powerUpList[2].effect);

    }
    
    // Call to present the player with the choice of powerups
    public void presentPowerUps()
    {
        Time.timeScale = 0;
        generatePowerUps();
        StartCoroutine(waitForSelection());
        
        // TODO: Display the powerups to the player using UI

    }

    // TODO: implement the powerup effects
    public void applyPowerUp(int choice)
    {
        switch (powerUpList[choice].effect)
        {
            case "damage":
                Debug.Log("Damage Powerup To Be Implemented");
                break;
            case "firerate":
                Debug.Log("Fire Rate Powerup To Be Implemented");
                break;
            case "reload":
                Debug.Log("Reload Speed Powerup To Be Implemented");
                break;
            case "clip":
                Debug.Log("Clip Size Powerup To Be Implemented");
                break;
            case "adrenalin":
                Debug.Log("Adrenalin Powerup Applied");
                break;
        }
    }

    IEnumerator waitForSelection()
    {
        int selection = -1;
        while (selection == -1){
            if (Input.GetKeyDown(KeyCode.Alpha1)){
                selection = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)){
                selection = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)){
                selection = 2;
            }
            yield return null;    
        }
        applyPowerUp(selection);
        Time.timeScale = 1;
    }
}
