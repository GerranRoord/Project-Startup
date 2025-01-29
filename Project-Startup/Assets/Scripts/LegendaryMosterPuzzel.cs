using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class LegendaryMosterPuzzel : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Fins = new GameObject[4];

    [SerializeField]
    public int timeBetweenFin;
    [SerializeField]
    public int FinTime;
    [SerializeField]
    public GameObject fella;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(timeBetweenFin);
        GameObject Fin = Fins[Random.Range(0, Fins.Length)];
        Fin.SetActive(true);
        yield return new WaitForSeconds(FinTime);
        Fin.SetActive(false);

        StartCoroutine(Timer());
    }

    public void showFella()
    {
        StopAllCoroutines();
        fella.SetActive(true);
    }
}
