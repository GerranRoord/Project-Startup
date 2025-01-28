using UnityEngine;

public class ConsertPosBG : MonoBehaviour
{
    [SerializeField]
    private GameObject bg;

    public int consertPos;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isHoldingFella)
        {
            bg.SetActive(true);
        }
        else
        {
            bg.SetActive(false);
        }
    }
}
