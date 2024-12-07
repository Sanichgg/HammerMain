using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorNasos : MonoBehaviour
{
    public NasosMain[] nasosMains;
    ChangeScenee change;
    private void Start()
    {
        change = FindObjectOfType<ChangeScenee>();
    }
    public bool AllNasos()
    {
        foreach (NasosMain nasosMainss in nasosMains)
        {
            if (!nasosMainss.isFixedInPlace)
            {
                return false;
            }
        }
        return true;
    }
    void Update()
    {
        if (AllNasos())
        {
            change.ChangeScnene();
        }
    }
}
