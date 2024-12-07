using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenee : MonoBehaviour
{
    public int sceneNumber;
    public Animator transitionRight;
    public Animator transitionLeft;
    public Animator transitionUp;
    public Animator transitionLeftShtori;
    public Animator transitionRightShtori;
    public float transitionTime;

    public void ChangeScnene()
    {
        StartCoroutine(LoadLevel(sceneNumber));
    }
    IEnumerator LoadLevel(int sceneNUm)
    {
        if(transitionLeftShtori == null && transitionRightShtori == null)
        {
            transitionRight.SetTrigger("Start");
            transitionLeft.SetTrigger("Start");
            transitionUp.SetTrigger("Start");
        }
        else
        {
            transitionRight.SetTrigger("Start");
            transitionLeft.SetTrigger("Start");
            transitionUp.SetTrigger("Start");
            transitionLeftShtori.SetTrigger("Start");
            transitionRightShtori.SetTrigger("Start");
        }
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneNUm);
    }
}
