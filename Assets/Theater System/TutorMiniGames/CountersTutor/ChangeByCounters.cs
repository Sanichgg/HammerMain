using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeByCounters : MonoBehaviour
{
    public SchetchikMain[] schetchikMain;
    ChangeScenee change;
    private void Start()
    {
        change = FindObjectOfType<ChangeScenee>();
    }
    public bool AllSchetshic()
    {
        foreach (SchetchikMain schetchikMain in schetchikMain)
        {
            if (!schetchikMain.isDamaged)
            {
                return false;
            }
        }
        return true;
    }
    void Update()
    {
        if(AllSchetshic())
        {
            change.ChangeScnene();
        }
    }
}
