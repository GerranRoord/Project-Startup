using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncommanFellaPuzzel : MonoBehaviour
{
    [SerializeField]
    public int timeBetweenAnim;
    [SerializeField]
    public int animTime;
    [SerializeField]
    public int animTimeWhenClicked;

    [SerializeField]
    private AudioSource source;

    private Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(timeBetweenAnim);
        transform.position += Vector3.up * .2f;
        yield return new WaitForSeconds(animTime);
        transform.position += Vector3.down * .2f;

        StartCoroutine(Timer());
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            source.Play();
            transform.position = startPos;
            StartCoroutine(ClickedAnim());
        }
    }

    public IEnumerator ClickedAnim()
    {
        GetComponentInChildren<FellaCapturable>().canCapture = true;
        transform.position += Vector3.up * .5f;
        yield return new WaitForSeconds(animTimeWhenClicked);
        transform.position += Vector3.down * .5f;

        StartCoroutine(Timer());
    }
}
