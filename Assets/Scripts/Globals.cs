using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    // Start is called before the first frame update
    public static class Weapon
    {
        public static Dictionary<int, IHitBehaviour> hitBehaviours = new Dictionary<int, IHitBehaviour>(){
            {0, new BulletBehaviour()}
        };
    }

}
