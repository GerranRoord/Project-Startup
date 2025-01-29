using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject curSceneWorld;

    public bool userFrozen = false;
    public bool interactingObj = false; // set this to true with object interactions so the camera doesn't move
    public bool interactingUI; // no need to update this one
    public bool canMove = true;
    public bool isHoldingFella = false;

    public GameObject fellaInspectionScreen;

    [SerializeField]
    public List<GameObject> cosmeticsInventory;
    
    [SerializeField]
    public List<GameObject> BaitInv;

    public List<GameObject> newFellas;

    [SerializeField]
    private List<GameObject> ownedFellas;

    [SerializeField]
    private GameObject[] lvlRewards;

    [SerializeField]
    public int exp;

    public int expToNextLevel = 100;

    [SerializeField]
    public int lvl;

    [SerializeField]
    private GameObject invItemPrefab;
    [SerializeField]
    public int coins;

    [SerializeField]
    private AudioSource gmAudioSource;
    [SerializeField]
    private AudioClip lvlUpClip;

    void Awake()
    {
        if (GameManager.instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void spawnFellas()
    {
        foreach (GameObject fella in newFellas)
        {
            ownedFellas.Add(fella);
        }
        newFellas.Clear();

        for (int i = 0; i < ownedFellas.Count; i++)
        {
            GameObject fella = Instantiate(ownedFellas[i], curSceneWorld.transform);
            fella.transform.position = new Vector3(i * 2,1,0);
        }
    }

    public void capturedFella(GameObject fella)
    {
        newFellas.Add(fella);
    }
    
    void Update()
    {
        if (!interactingUI)
        {
               interactingUI = EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0);
        }
        else
        {
            interactingUI = !Input.GetMouseButtonUp(0);
        }
    }
    public void SetCanMove(bool b)
    {
        canMove = b;
        userFrozen = !b;
        print(!b);
    }
    public void addEXP(int xp, GameObject consertUpgradePos, out string gainedItems )
    {
        gainedItems = "";
        exp += xp;

        while(exp >= expToNextLevel)
        {
            lvl++;
            gmAudioSource.PlayOneShot(lvlUpClip);

            if (lvl < lvlRewards.Length)
            {
                if (lvlRewards[lvl].GetComponent<Cosmetic>() != null)
                {
                    cosmeticsInventory.Add(lvlRewards[lvl]);
                    gainedItems += lvlRewards[lvl].name + "\n";
                }
                else if(lvlRewards[lvl].tag == "Bait")
                {
                    BaitInv.Add(lvlRewards[lvl]);
                    gainedItems += "Steak \n";

                }
                else if (lvlRewards[lvl].tag == "Consert")
                {
                    Instantiate(lvlRewards[lvl], consertUpgradePos.transform);
                    gainedItems += " Stage upgrade \n";

                }
            }
            exp -= expToNextLevel;
            expToNextLevel += 100 * lvl;
        }
    }

    public void openInv(GameObject content)
    {
        foreach(GameObject item in cosmeticsInventory)
        {
            GameObject invItem = Instantiate(invItemPrefab, content.transform);
            Item itemScript = item.gameObject.GetComponent<Item>();
            invItem.GetComponentInChildren<Image>().sprite = itemScript.itemImage;
            invItem.GetComponentInChildren<TextMeshProUGUI>().text = itemScript.itemName;
        }
        foreach (GameObject item in BaitInv)
        {
            GameObject invItem = Instantiate(invItemPrefab, content.transform);
            Item itemScript = item.GetComponent<Item>();
            invItem.GetComponentInChildren<Image>().sprite = itemScript.itemImage;
            invItem.GetComponentInChildren<TextMeshProUGUI>().text = itemScript.itemName;
        }
    }
}
