using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strelkaManager : MonoBehaviour
{
    public GameObject strelka; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("hammer"))
        {
            if (strelka != null)
            {
                strelka.SetActive(false); 
            }
        }
    }
}
