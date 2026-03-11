using System;
using System.Collections;
using UnityEngine;

public class WindTurbine : MonoBehaviour
{
    public int generationAmount = 5;
    private Coroutine powerGenerationCoroutine;
    private ResourceManager resourceManager;

    private void Start()
    {
        resourceManager = ResourceManager.Instance;
        powerGenerationCoroutine = StartCoroutine(GeneratePower());

    }

    private IEnumerator GeneratePower()
    {
        while (true)
        {
            resourceManager.AddPower(generationAmount);
            yield return new WaitForSeconds(10f);
        }
    }
}
