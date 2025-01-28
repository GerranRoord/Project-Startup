using UnityEngine;

public class BuyCosmetic : MonoBehaviour
{
    [SerializeField] Cosmetic cosPrefab;
   
   public void Buy()
    {
        if (GameManager.instance.coins >= 50)
        {
            GameManager.instance.cosmeticsInventory.Add(cosPrefab);
            GameManager.instance.coins -= 50;
        }
    }
}
