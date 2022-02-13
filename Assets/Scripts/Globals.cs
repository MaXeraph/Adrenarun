using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    // Start is called before the first frame update
    public static class Guns
    {
        public static Dictionary<int, Bullet> bulletTypes = new Dictionary<int, Bullet>(){
            {0, new BulletPeashooter()}
        };
    }

}
