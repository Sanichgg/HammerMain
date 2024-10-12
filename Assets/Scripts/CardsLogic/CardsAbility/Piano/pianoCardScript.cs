using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pianoCardScript : MonoBehaviour
{
    public PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    public void Spawn()
    {
        if (playerController != null)
        {
            playerController.OnPianoCardChosen();
        }
    }
}
