using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class selfDestruct : MonoBehaviour
{
    public float delay = 0.15f;
    public bool fade = false;
    public Image fadeImage;


    void Awake() => init();

    void init()
    {
    
        if (fade && fadeImage != null)
        {
            fadeImage.transform.localScale = Vector3.zero;
            fadeImage.transform.DOScale(new Vector3(1, 1, 1), delay);
            fadeImage.DOFade(0, delay).OnComplete(() => die(0));
        }
        else die(delay);
    }

    void die(float d = 0)
    {
        Destroy(gameObject, d);
    }

}
