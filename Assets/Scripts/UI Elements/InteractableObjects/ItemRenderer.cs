using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ItemRenderer : MonoBehaviour
{
    public GameObject itemIconPrefab;
    public void UpdateDisplays(List<Item> itemList)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        if (itemList.Count == 0)
        {
            return;
        }
        float spacing = 0.42f;
        float xPos = (spacing / 2) * -(itemList.Count - 1);

        foreach (Item item in itemList)
        {
            GameObject newItemDisplay = Instantiate(itemIconPrefab, transform);
            newItemDisplay.transform.localPosition = new UnityEngine.Vector3(xPos, 0, 0);
            newItemDisplay.transform.localScale = new UnityEngine.Vector3(0.33f, 0.33f, 0.33f);
            SpriteRenderer spriteRendererComponent = newItemDisplay.GetComponent<SpriteRenderer>();
            string itemName = item.getImageString();
            string file_path = $"ItemIcons/{itemName}";
            spriteRendererComponent.sprite = Resources.Load<Sprite>(file_path);
            xPos += spacing;
        }
    }
}