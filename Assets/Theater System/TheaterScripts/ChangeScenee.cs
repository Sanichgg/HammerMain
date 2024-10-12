using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenee : MonoBehaviour
{
    [SerializeField] Animator anim;
    public int sceneNumber;
    public float timeToChangeScene;
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void TimerChangeSceneMethod()
    {
        anim.SetTrigger("Start");
        Invoke("ChangeSceneewew", timeToChangeScene);
    }
    public void ChangeScene() => SceneManager.LoadScene(sceneNumber);
}
