using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaketaCardScript : MonoBehaviour
{
    public PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    public void OnCardSelected()
    {
        if (playerController != null)
        {
            playerController.OnRaketaCardChosen();
            Debug.Log("ракета выбрана");
        }
    }
}
