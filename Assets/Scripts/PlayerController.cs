using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float marshrutkaCardsPoint; // маршрутка
    public float pianoCardPoint;
    //public List<svyatogorScript> svyatogors = new List<svyatogorScript>(); 

    public bool isSvyatogorCardChosen = false;
    public bool isPianoCardChosen = false;

    public void OnSvyatogorCardChosen()
    {
        isSvyatogorCardChosen = true;
    }

    public void OnPianoCardChosen()
    {
        isPianoCardChosen = true;
    }
}