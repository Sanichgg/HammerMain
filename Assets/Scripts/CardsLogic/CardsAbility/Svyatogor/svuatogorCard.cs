using UnityEngine;

public class SvyatogorCard : MonoBehaviour
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
            playerController.OnSvyatogorCardChosen();
        }
    }
}
