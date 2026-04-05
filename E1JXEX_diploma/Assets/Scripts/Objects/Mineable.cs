using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mineable : MonoBehaviour
{
    public ResourceType resourceType;
    public float maxDurability = 100f;
    private float currentDurability;
    public bool nest;
    public int yieldAmount = 3;
    public GameObject chunkPrefab;

    public TMP_Text typeText;
    public Image countBar;

    void Start()
    {
        currentDurability = maxDurability;
        if (nest)
        {
            if (typeText != null) typeText.text = "Enemy Nest!";
        }
        else
        {
            if (typeText != null) typeText.text = resourceType.ToString();
        }
        
    }

    public void Mine(float drillPower)
    {
        currentDurability -= drillPower;
        countBar.fillAmount = currentDurability / maxDurability;

        if (currentDurability <= 0)
        {
            BreakAndDrop();
        }
    }

    private void BreakAndDrop()
    {
        for (int i = 0; i < yieldAmount; i++)
        {
            Vector3 dropPos = transform.position + new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
            GameObject chunkObj = Instantiate(chunkPrefab, dropPos, Quaternion.identity);

            ResourceChunk chunk = chunkObj.GetComponent<ResourceChunk>();
            chunk.resourceType = this.resourceType;
            chunk.amount = 1;
        }
        Destroy(gameObject);
    }
}