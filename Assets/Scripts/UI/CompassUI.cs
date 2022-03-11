using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassUI : MonoBehaviour
{
    public static CompassUI Instance;
    private static Transform CameraTransform;
    private static RectTransform CompassBar;

    private static Transform mask;

    private static RectTransform[] directionTransforms = new RectTransform[8];

    private static string[] directionNames = new string[] { "North", "South", "East", "West", "NW", "NE", "SW", "SE" };

    private static Vector3[] directionVectors = new Vector3[] {Vector3.forward* 1000, Vector3.back* 1000, Vector3.right* 1000, Vector3.left* 1000,
                                                       (Vector3.right + Vector3.forward)* 1000, (Vector3.left + Vector3.forward)* 1000, (Vector3.right + Vector3.back)* 1000, (Vector3.left + Vector3.back)* 1000 };

    private static List<Transform> enemies = new List<Transform>();
    private static List<GameObject> enemyMarkers = new List<GameObject>();

    public static CompassUI instance;

    void Awake()
    {
        instance = this;
        mask = transform.GetChild(1);
        CameraTransform = Camera.main.transform;
        CompassBar = transform.GetChild(1).GetComponent<RectTransform>();
        for (int i = 0;i < directionNames.Length; i++)
        {
            string _name = directionNames[i];
            directionTransforms[i] = CompassBar.Find(_name).GetComponent<RectTransform>();
        }
    }

    public static void addEnemy(Transform entity)
    {
        enemies.Add(entity);
        GameObject marker = GameObject.Instantiate(Resources.Load("CompassWarning")) as GameObject;
        marker.transform.SetParent(mask);
        enemyMarkers.Add(marker);
    }

    public static void enemyDied(int enemy)
    {
        enemies.Remove(enemies[enemy]);
        GameObject mark = enemyMarkers[enemy];
        Destroy(mark);
        enemyMarkers.Remove(mark);
    }

    static void updateEnemyPosition(int enemy)
    {
        if (enemies[enemy] == null) enemyDied(enemy);
        else SetMarkerPosition(enemyMarkers[enemy].GetComponent<RectTransform>(), enemies[enemy].position); 
    }

    public static void updateCompass()
    {
        for (int i = 0; i < directionNames.Length; i++)
        {
            SetMarkerPosition(directionTransforms[i], directionVectors[i]);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            updateEnemyPosition(i);
        }
    }

    private static void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        Vector3 directionToTarget = worldPosition - CameraTransform.position;
        float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), new Vector2(CameraTransform.transform.forward.x, CameraTransform.transform.forward.z));
        float compassPosX = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -1, 1);
        markerTransform.anchoredPosition = new Vector3(CompassBar.rect.width / 1.9f * compassPosX, 0, 0);
    }

}
