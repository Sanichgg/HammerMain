using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorAgain : MonoBehaviour
{
    public SchetchikMain[] schetchikMain;
    public NasosMain[] nasosMains;
    ChangeScenee change;
    public float timer; 
    private void Start()
    {
        change = FindObjectOfType<ChangeScenee>();
        StartCoroutine(Timer());
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
        if ((AllSchetshic() && AllNasos()) || timer == 0)
        {
            change.ChangeScnene();
        }
        
    }
    public IEnumerator Timer()
    {
        Debug.Log("fweweqwwef");
        while (timer != 0)
        {
            yield return new WaitForSeconds(1);
            --timer;
        }
    }
}
