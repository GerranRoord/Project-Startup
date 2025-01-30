using UnityEngine;

public class BuyCosmetic : MonoBehaviour
{
    [SerializeField] GameObject cosPrefab;
    [SerializeField] int price;
   
   public void Buy()
    {
        if (GameManager.instance.coins >= price)
        {
            GameManager.instance.cosmeticsInventory.Add(cosPrefab);
            GameManager.instance.coins -= price;
        }
    }
}
