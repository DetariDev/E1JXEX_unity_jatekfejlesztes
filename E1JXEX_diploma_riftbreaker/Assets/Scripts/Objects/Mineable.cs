using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mineable : MonoBehaviour
{
    [SerializeField] public ResourceType resourceType;
    [SerializeField] private int resourceAmount;
    [SerializeField] private int maxResourceAmount = 100;
    public TMP_Text typeText;
    public Image countBar;

    void Start()
    {
        typeText.text = resourceType.ToString();
        resourceAmount = maxResourceAmount;
    }
    

    public void Mine(int amount)
    {
        if (resourceAmount <= 0) return;

        resourceAmount -= amount;
        ResourceManager.Instance.AddResource(resourceType, amount);
        countBar.fillAmount = (float)resourceAmount / maxResourceAmount;
        if (resourceAmount <= 0)
        {
            Destroy(gameObject);
        }
    }
}