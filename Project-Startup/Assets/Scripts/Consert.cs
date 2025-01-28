using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Concert : MonoBehaviour
{
    List<GameObject> fellas = new List<GameObject>(); // singers
    List<GameObject> dancingFellas = new List<GameObject>(); // bystanders

    private bool concertGoing = false;
    bool concertEnded = false;
    private float timer;
    [SerializeField]
    private float concertTime;

    private int score;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private GameObject scoreScreen;
    [SerializeField]
    private NumberGoUp scoreScreenScoreText;

    [SerializeField] Transform concertCamTransform;
    private Vector3 originalCamPos;
    private Quaternion originalCamRot;
    [SerializeField] private float camLerpSpeed = 2f;

    [SerializeField]
    private AudioSource BG;

    [SerializeField]
    private GameObject[] muteMeButtons = new GameObject[4];
    [SerializeField]
    private GameObject[] muteAllButMeButtons = new GameObject[4];
    [SerializeField]
    private GameObject[] muteButtons = new GameObject[4];

    private bool isMuted = false;

    [SerializeField]
    private AudioSource consertComplete;
    [SerializeField]
    private GameObject consertUpgradePos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(concertEnded)
        {
            scoreText.text = "Score: " + score;
            concertEnded = false;
        }
    }

    public void playPreview()
    {
        foreach (GameObject fella in fellas)
        {
            fella.GetComponent<AudioSource>().Stop();
        }

        for (int i = 0; i < fellas.Count; i++)
        {
            muteButtons[i].SetActive(false);
        }

        fellas.Clear();
        int curActive = 0;
        foreach (Transform child in transform.transform)
        {
            if (child.tag == "FellaPos" && child.childCount >= 2)
            {
                fellas.Add(child.GetComponentInChildren<Fella>().gameObject);
                muteButtons[curActive].SetActive(true);
                curActive++;
            }

        }


        foreach (GameObject fella in fellas)
        {
            fella.GetComponent<AudioSource>().Play();
        }
    }

    public void stopPreview()
    {
        for (int i = 0; i < fellas.Count; i++)
        {
            fellas[i].GetComponent<AudioSource>().Stop();
            muteButtons[i].SetActive(false);
        }
    }

    public void StartConsert()
    {
        for (int i = 0; i < fellas.Count; i++)
        {
            fellas[i].GetComponent<AudioSource>().Stop();
            muteButtons[i].SetActive(false);
        }

        fellas.Clear();

        originalCamPos = Camera.main.transform.position;
        originalCamRot = Camera.main.transform.rotation;

        int curActive = 0;

        foreach (Transform child in transform.transform)
        {
            if (child.tag == "FellaPos" && child.childCount >= 2)
            {
                fellas.Add(child.GetComponentInChildren<Fella>().gameObject);
                muteButtons[curActive].SetActive(true);
                curActive++;
            }
            if(child.tag == "Fella")
            {
                dancingFellas.Add(child.gameObject);
            }
        }

        BG.Stop();

        foreach(GameObject fella in fellas)
        {
            fella.GetComponent<AudioSource>().Play();
        }
        foreach(GameObject dancingFella in dancingFellas)
        {
            dancingFella.AddComponent<Dancing>();

        }

        timer = 0;
        score = 0;

        concertGoing = true;
        GameManager.instance.userFrozen = true; 
        StartCoroutine(lerpCamera(concertCamTransform.position, concertCamTransform.rotation));

        StartCoroutine(Timer());

    }

    public void StopConsert()
    {
        for (int i = 0; i < fellas.Count; i++)
        {
            fellas[i].GetComponent<AudioSource>().Stop();
            muteButtons[i].SetActive(false);
        }
        foreach (GameObject dancingFella in dancingFellas)
        {
            Destroy(dancingFella.GetComponent<Dancing>());
        }
        consertComplete.Play();
        BG.Play();
        concertGoing = false;
        concertEnded = true;
        GameManager.instance.userFrozen = false;
        GameManager.instance.addEXP(score, consertUpgradePos);
        scoreScreen.SetActive(true);
        scoreScreenScoreText.SetTarget(score);
        StartCoroutine(lerpCamera(originalCamPos, originalCamRot));

    }

    public IEnumerator Timer()
    {
        while (concertGoing)
        {
            yield return new WaitForSeconds(1);
            foreach (GameObject fella in fellas)
            {
                score += fella.GetComponent<Fella>().getScore();
            }
            scoreText.text = "score: " + score;

            timer++;
            if (timer >= concertTime)
            {
                StopConsert();
                StopCoroutine(Timer());
                StartCoroutine(lerpCamera(originalCamPos, originalCamRot));

                print("wth");
            }
        }
    }
    IEnumerator lerpCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        Camera mainCamera = Camera.main;
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.01f || Quaternion.Angle(mainCamera.transform.rotation, targetRotation) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * camLerpSpeed);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetRotation, Time.deltaTime * camLerpSpeed);

            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
    }

    public void MuteButtonPressed(int buttonPressed)
    {
        if(isMuted)
        {
             UnMuteAll(buttonPressed);
        }
        else
        {
            MuteAllBut(buttonPressed);
        }
    }

    public void MuteAllBut(int notMute)
    {
        for(int i = 0; i < fellas.Count; i++)
        {
            if(i != notMute)
            {
                fellas[i].GetComponent<AudioSource>().mute = true;
            }
            else
            {
                muteAllButMeButtons[i].GetComponent<Image>().color = Color.gray;
            }
        }
        isMuted = true;

    }
    public void UnMuteAll(int notMute)
    {
        foreach (GameObject fella in fellas)
        {
            if (fella != fellas[notMute])
            {
                fella.GetComponent<AudioSource>().mute = false;
            }
        }
        foreach(GameObject button in muteAllButMeButtons)
        {
            button.GetComponent<Image>().color = Color.white;
        }
        isMuted = false;
    }

    public void muteMe(int buttonNumber)
    {
        AudioSource source = fellas[buttonNumber].GetComponent<AudioSource>();
        Image image = muteMeButtons[buttonNumber].GetComponent<Image>();
        if (source.mute)
        {
            source.mute = false;
            image.color = Color.white;
        }
        else
        {
            source.mute = true;
            image.color = Color.gray;
        }
    }

}
