using UnityEngine;

public class FellaCapturable : MonoBehaviour
{
    public bool canCapture = true;

    [SerializeField]
    private AudioSource m_AudioSource;

    [SerializeField]
    private GameObject fella;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.instance.userFrozen && canCapture)
        {
            GameManager.instance.capturedFella(fella);
            m_AudioSource.Play();

            if(GetComponentInParent<UncommanFellaPuzzel>() != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
