using UnityEngine;

public class selfDestruct : MonoBehaviour
{
    public float delay = 0.15f;


    void Awake() => Destroy(gameObject, delay);

}
