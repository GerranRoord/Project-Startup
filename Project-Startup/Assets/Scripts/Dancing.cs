using UnityEngine;

public class Dancing : MonoBehaviour
{
    [SerializeField] float swayAmount = 0.5f;
    [SerializeField] float swaySpeed = 1f;
    [SerializeField] Vector3 swayAxis = Vector3.right;

    Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float swayOffset = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        transform.position = initialPosition + swayAxis * swayOffset;
    }
    private void OnDestroy()
    {
        transform.position = initialPosition;
    }
}