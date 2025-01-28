using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public enum FellaRarity
    {
        COMMON,
        RARE,
        LEGENDARY
    }
public class Fella : MonoBehaviour
{
    
    [SerializeField]
    private int score;
    //[UnityEngine.Range(1, 100)]
    //private int fatigue;
    public FellaRarity rarity;

    [SerializeField]
    private string fellaName;
    [SerializeField]
    private string descriptionText;
    [SerializeField] List<Cosmetic> cosmetics;
    [SerializeField] List<Cosmetic> cosmeticPrefabs;
    [SerializeField] Transform hatPos;
    [SerializeField] Transform bowPos;

    public AudioClip fellaAudio;
    [SerializeField]
    private AudioClip pickupClip;


    float lastClickTime = 0f;
    float doubleClickTime = 0.3f;
    bool dragging = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TestSound()
    {
        GetComponent<AudioSource>().PlayOneShot(fellaAudio);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ChangeScene")
        {
            collision.transform.GetComponent<SceneSwitcher1>().SwitchScene();
        }
    }

    public int getScore()
    {
        int bonus = 0;
        foreach (Cosmetic c in cosmetics)
        {
            bonus += c.ScoreBonus;
        }
        return score + bonus;
    }

    void equipCosmetic(Cosmetic instance, Cosmetic c)
    {
        if (instance.Name == "Hat")
            instance.transform.position = hatPos.position;
        else if (instance.Name == "Bowtie")
            instance.transform.position = bowPos.position;

        if (cosmetics.Contains(instance)) cosmetics.Add(instance);
        if (cosmeticPrefabs.Contains(c)) cosmeticPrefabs.Add(c);
        if (GameManager.instance.cosmeticsInventory.Contains(c)) GameManager.instance.cosmeticsInventory.Remove(c);
    }
    void FellaInspectionScreen()
    {
        GameManager.instance.canMove = false;
        Canvas canvas = FindAnyObjectByType<Canvas>();
        GameObject tempInspector = Instantiate(GameManager.instance.fellaInspectionScreen, canvas.transform, false);

        TMP_Text nameText = tempInspector.transform.Find("FellaName")?.GetComponent<TMP_Text>();
        TMP_Text rarityText = tempInspector.transform.Find("FellaRarity")?.GetComponent<TMP_Text>();
        TMP_Text fatigueText = tempInspector.transform.Find("FellaFatigue")?.GetComponent<TMP_Text>();
        TMP_Text descText = tempInspector.transform.Find("FellaDescription")?.GetComponent<TMP_Text>();
        TMP_Dropdown dropdown = tempInspector.transform.Find("CosmeticDropdown")?.GetComponent<TMP_Dropdown>();

        if (nameText != null) nameText.text = fellaName;

        string rareText = "";
        switch (rarity)
        {
            case FellaRarity.COMMON:
                rareText = "Common";
                break;
                case FellaRarity.RARE:
                rareText = "Rare";
                break;
            case FellaRarity.LEGENDARY:
                rareText = "Legendary";
                break;
        }
        if (rarityText != null) rarityText.text = rareText;

        if (fatigueText != null) fatigueText.text = "Energetic";
        if (descText != null) descText.text = descriptionText;

        List<string> cosmeticNames = new List<string>() { "Select cosmetic" };

        foreach (Cosmetic cosmetic in GameManager.instance.cosmeticsInventory)
        {
            cosmeticNames.Add(cosmetic.name);
        }
        cosmeticNames.Add("None");
        dropdown.ClearOptions();
        dropdown.AddOptions(cosmeticNames);

        dropdown.onValueChanged.AddListener(delegate
        {
            string selectedCosmeticName = dropdown.options[dropdown.value].text;
            print(selectedCosmeticName);
            if (selectedCosmeticName == "None")
            {
                List<Transform> toDestroy = new List<Transform>();
                foreach (Transform child in transform)
                {
                    Cosmetic childCosmetic = child.GetComponent<Cosmetic>();
                    if (childCosmetic != null)
                    {
                        GameManager.instance.cosmeticsInventory.Add(cosmeticPrefabs.Find(cosmetic => childCosmetic.Name == cosmetic.Name));
                        cosmetics.Remove(childCosmetic);
                        toDestroy.Add(child);
                    }
                }
                foreach (Transform transform in toDestroy)
                {
                    Destroy(transform.gameObject);
                }
                return;
            }
            else
            {
                Cosmetic cosmeticToAdd = GameManager.instance.cosmeticsInventory.Find(cosmetic => cosmetic.name == selectedCosmeticName);
                Cosmetic cosmeticInstance = Instantiate(cosmeticToAdd, transform);
                equipCosmetic(cosmeticInstance, cosmeticToAdd);
            }
        });

        foreach (Button button in tempInspector.GetComponentsInChildren<Button>())
        {
            if (button.gameObject.name == "Sound")
            {
                button.onClick.AddListener(() =>
                {
                    TestSound();
                });
            }
            else
            {
                button.onClick.AddListener(() =>
                {
                    Destroy(tempInspector);
                    GameManager.instance.canMove = true;
                });
            }
        }
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.instance.userFrozen)
        {
            dragging = true;


            float clickDelta = Time.time - lastClickTime;
            if (clickDelta <= doubleClickTime)
            {
                dragging = false;
                print("double click");
                FellaInspectionScreen();
                gameObject.GetComponent<FellaMover>().followMouse = false;
                GameManager.instance.isHoldingFella = false;
                GameManager.instance.canMove = true;
                transform.parent = GameManager.instance.curSceneWorld.transform;

                List<Transform> positions = new List<Transform>();

                foreach (Transform child in transform.parent.transform)
                {
                    if (child.tag == "FellaPos" && child.childCount == 1)
                    {
                        positions.Add(child);
                    }
                }

                if (positions.Count > 0)
                {
                    int closest = 0;
                    for (int i = 0; i < positions.Count; i++)
                    {
                        if ((transform.position - positions[i].position).magnitude < (transform.position - positions[closest].position).magnitude)
                        {
                            closest = i;
                        }
                    }
                    transform.position = positions[closest].position;
                    transform.parent = positions[closest];
                }
                GameManager.instance.userFrozen = true;
            }
            lastClickTime = Time.time;
        }
        if (Input.GetMouseButtonDown(0) && !GameManager.instance.userFrozen && dragging)
        {
            gameObject.GetComponent<FellaMover>().followMouse = true;
            GetComponent<AudioSource>().PlayOneShot(pickupClip);
            gameObject.GetComponent<FellaMover>().transform.parent = null;
            print("drag");
        }
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            gameObject.GetComponent<FellaMover>().followMouse = false;
            GameManager.instance.isHoldingFella = false;
            GameManager.instance.canMove = true;
            transform.parent = GameManager.instance.curSceneWorld.transform;

            List<Transform> positions = new List<Transform>();

            foreach (Transform child in transform.parent.transform)
            {
                if (child.tag == "FellaPos" && child.childCount == 1)
                {
                    positions.Add(child);
                }
            }

            if (positions.Count > 0)
            {
                int closest = 0;
                for (int i = 0; i < positions.Count; i++)
                {
                    if ((transform.position - positions[i].position).magnitude < (transform.position - positions[closest].position).magnitude)
                    {
                        closest = i;
                    }
                }
                transform.position = positions[closest].position;
                transform.parent = positions[closest];
            }
            print("drag release");

        }
    }
}