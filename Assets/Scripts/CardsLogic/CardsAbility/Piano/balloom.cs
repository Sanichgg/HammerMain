using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("hammer"))
        {
            pianoLogicCard pianoLogic = FindObjectOfType<pianoLogicCard>();
            if (pianoLogic != null)
            {
                pianoLogic.StopPiano();
                Destroy(this.gameObject);
            }
        }
    }
}
