using UnityEngine;
using UnityEngine.UI;

public class deathUI : MonoBehaviour
{

    private Image[] toFade;
    Button retry;
    Button leave;
    public static GameObject instance;

   void Awake()
    {
        instance = this.gameObject;
        toFade = GetComponentsInChildren<Image>();
        retry = transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
        leave = transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
        retry.onClick.AddListener(rewind);
        leave.onClick.AddListener(done);
        instance.SetActive(false);
    }

    public static void reveal(GameObject inst)
    {
        Menu.open();
        Menu.pause();
        UIManager.dead = true;
        instance.SetActive(true);
    }

    public void done()
    {
        Menu.quit();
    }

   public void rewind()
    {
        Menu.restart();
        gameObject.SetActive(false);
    }
}
