using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private bool triggered = false;
    private static int stage = 0;

    [SerializeField]
    private GameObject hideControls;
    [SerializeField]
    private GameObject showControls;
    [SerializeField]
    private TutorialManager tutManager;

    private void OnTriggerEnter(Collider other) {
        if (!triggered) {
            if (other.gameObject.tag == "Player") {
                tutManager.DisplayControls(hideControls, showControls);
                tutManager.HandleEvents(stage);
                triggered = true;
                stage++;
                tutManager.respawnLocation = gameObject.transform.position;
            }
        }
    }
}
