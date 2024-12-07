using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System;
using TMPro;
using Spine.Unity;


[RequireComponent(typeof(WaypontsSystem))]
public class TheaterLeft : MonoBehaviour
{
    [SerializeField] bool facingRight;
    [SerializeField] float scaleX;
    SkeletonAnimation skeleton;
    [Header("Waypoints")]
    WaypontsSystem waypontsSystem;
    [SerializeField] int waypointindex;
    [SerializeField] bool moving = true;
    public GameObject currentTextBox;
    public TMP_Text text;
    [Space]
    public string walkAnimationName;
    public bool walkAnimationLoopSetting;
    public List<PointsSettings> mainIndividaulParameters = new List<PointsSettings>();


    private void OnEnable()
    {
        waypontsSystem = GetComponent<WaypontsSystem>();

        //text = GetComponentInChildren<TMP_Text>();
        skeleton = GetComponent<SkeletonAnimation>();

        Debug.Log(GetComponentInChildren<TMP_Text>().name);
        Debug.Log(mainIndividaulParameters.Count);


        if (waypontsSystem.waypoints.Length < mainIndividaulParameters.Count || waypontsSystem.waypoints.Length > mainIndividaulParameters.Count)
        {
            throw new Exception("Ты Дэбил? Где ещё точки?!");
        }
        else
        {
            for (int i = 0; i < waypontsSystem.waypoints.Length; i++)
            {
                mainIndividaulParameters[i].waypoint = waypontsSystem.waypoints[i];
            }
        }
    }
    private void Start()
    {
        scaleX = -transform.localScale.x;

    }
    public void Update()
    {
        /*
        if (currentTextBox.activeInHierarchy)
        {
            Debug.Log("Yes It's Active");
        }
        else Debug.Log("Nope");*/

        Movement();



    }
    protected void Movement()
    {

        if (moving)
        {

            transform.position = Vector2.MoveTowards(transform.position,
                waypontsSystem.waypoints[waypointindex].position,
                mainIndividaulParameters[waypointindex].speed * Time.deltaTime);

            skeleton.loop = walkAnimationLoopSetting;
            skeleton.AnimationName = walkAnimationName;

            if (mainIndividaulParameters[waypointindex].showTextWhileWalking)
            {
                mainIndividaulParameters[waypointindex].textBox.SetActive(true);
                text.text = mainIndividaulParameters[waypointindex].text;

            }
            else mainIndividaulParameters[waypointindex].textBox.SetActive(false);

        }
        if (Vector2.Distance(transform.position, waypontsSystem.waypoints[waypointindex].position) < 0.01f)
        {
            //Flip(mainIndividaulParameters[waypointindex]);
            if (moving)
            {

                //DirCheck();
                StartCoroutine(Waiting(mainIndividaulParameters[waypointindex].stop));


            }

        }
    }
    IEnumerator Waiting(bool wait = true)
    {
        mainIndividaulParameters[waypointindex].WhatToDo.Invoke();
        moving = false;
        yield return new WaitForSeconds(wait ? mainIndividaulParameters[waypointindex].stopTimer : 0);
        if (waypointindex < waypontsSystem.waypoints.Length - 1) { waypointindex++; }

        Flip();
        moving = true;
    }


    public void Flip()
    {
        //if (mainIndividaulParameters[waypointindex].lookToTheLeftOnNextPoint == false)
        //{


        Vector3 localScale = transform.localScale;
        //Mathf.Sign заставляет персонажа смотреть в ту же сторону, к которой он шёл(отличие от основного в минусе)
        localScale.x = scaleX * Mathf.Sign(mainIndividaulParameters[waypointindex].waypoint.transform.position.x - transform.position.x);
        transform.localScale = localScale;


        Vector3 localScaleText = currentTextBox.transform.localScale;
        //Mathf.Sign заставляет диалоговое окно смотреть в ту же сторону, к которой он шёл персонаж (отличие от основного в минусе)
        localScaleText.x = -localScaleText.x * Mathf.Sign(mainIndividaulParameters[waypointindex].waypoint.transform.position.x - transform.position.x);
        currentTextBox.transform.localScale = localScaleText;
        Debug.LogWarning("Flip");
        /*
        Vector3 localScaleText = currentTextBox.transform.localScale;
        localScaleText.x *= -1f;
        currentTextBox.transform.localScale = localScaleText;
        Debug.LogWarning("Flip");*/

        // } else return;


    }

    ////
    //Методы во время остановки
    ////
    public void SayingWhileStanding(string textLocal)
    {
        if (mainIndividaulParameters[waypointindex].stop)
        {
            mainIndividaulParameters[waypointindex].textBox.SetActive(true);
            text.text = textLocal;
        }
        else mainIndividaulParameters[waypointindex].textBox.SetActive(false);
    }

    public void LoopAnimationOnStayOrNot(bool loop) => skeleton.loop = loop;
    public void AnimationNameToPlay(string animName)
    {
        skeleton.AnimationName = animName;
        if (!skeleton.AnimationName.Contains(animName))
        {
            throw new Exception("Имя анимации неправильно записана, либо её не существует у модели!");
        }

        /*
        Debug.LogWarning(skeleton.AnimationName);
        skeleton.AnimationName.Contains(animName);
        skeleton.AnimationName = animName;
        Debug.LogWarning(skeleton.AnimationName);*/
    }
    public void AddSaund()
    {
        //сыграть звук
        AudioClip.Create("far", 23,1,1,true);
        
    }
}
//Сам массив данных
[System.Serializable]
public class PointsSettings2
{

    public string namePoint;
    [Tooltip("Название точки")]
    [Header("TextBubble")]
    public bool showTextWhileWalking;
    public GameObject textBox;
    public string text;
    [Header("Path")]
    public Transform waypoint;
    [Header("Speed Settings")]
    public float speed;
    public bool lookToTheLeftOnNextPoint;

    [Header("Stay on Point")]
    public bool stop;
    public float stopTimer;
    public UnityEvent WhatToDo;
}
