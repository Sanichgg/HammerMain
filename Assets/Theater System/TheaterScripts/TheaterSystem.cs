using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;
using Spine.Unity;


/// <summary>
/// Общая инструкция: 
/// 
/// !Как работает логика. Есть точки, он их ищет, он идёт к ним. 
/// 
/// Есть возможность говорить пока он идёт к цели.
/// 
/// * Если вам эта функция не нужна будет, то только добавляйте, в Text Box, диалоговое окно перса.
/// 
/// Если в "mainIndividaulParameters" стоит галочка стоп, то он останавливается, на определённое время.
/// 
/// Чтобы проигрывать анимацию и говорить что-то, используйте методы написанные в самом низу.
/// 
/// Если же нужна пауза(остаться на точке к которой он пришёл) у персонажа(ждать своей очереди),
/// то ставите галочку в Stop и на сколько времени (Stop Timer)
/// 
/// Внизу под методами находится класс "PointsSettings" 
/// в нём прописаны все нужные переменные.
/// ** ЕСЛИ вам нужно будет добавить новую переменную то добавляется таким образом
/// mainIndividaulParameters[waypointindex].название переменной в классе "PointsSettings"
/// 
/// Читайте ошибки прописанные в этом скрипте, если они возникнут. Они являются подсказками.
/// 
/// Если добавляете анимацию, то анимация должна соответствовать названию
/// в компоненте SkeletonAnimation => Animation name!
/// </summary>

[RequireComponent(typeof(WaypontsSystem))]
public class TheaterSystem : MonoBehaviour
{
    [SerializeField] bool moving = true;
    [SerializeField] float scaleX;
    SkeletonAnimation skeleton;
    [Header("Waypoints")]
    WaypontsSystem waypontsSystem;
    [SerializeField] int waypointindex;
    
    [Header("ChatBox")]
    public GameObject currentTextBox;
    public TMP_Text text;
    [Header("Audio")]
    [SerializeField] AudioSource audiToPlay;
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

        //Поиск точек
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
        scaleX = transform.localScale.x;
        audiToPlay = GetComponent<AudioSource>();
    }
    public void Update()
    {
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
        //Если достигает цели то останавливается
        if (Vector2.Distance(transform.position, waypontsSystem.waypoints[waypointindex].position) < 0.01f)
        {
            if (moving)
            {
                StartCoroutine(Waiting(mainIndividaulParameters[waypointindex].stop));
            }

        }
    }
    //Что происходит при остановки
    IEnumerator Waiting(bool wait = true)
    {
        mainIndividaulParameters[waypointindex].WhatToDo.Invoke();
        moving = false;
        yield return new WaitForSeconds(wait ? mainIndividaulParameters[waypointindex].stopTimer : 0);
        if (waypointindex < waypontsSystem.waypoints.Length - 1) { waypointindex++; }

        Flip();
        moving = true;
    }
    
    //Поворот персонажа если персонаж изначально смотрит ВПРАВО
    //*Альтернативный скрипт называется TheaterLeft
    public void Flip()
    {
            Vector3 localScale = transform.localScale;
            //Mathf.Sign заставляет персонажа смотреть в ту же сторону, к которой он шёл
            localScale.x = scaleX * Mathf.Sign(mainIndividaulParameters[waypointindex].waypoint.transform.position.x - transform.position.x);
            transform.localScale = localScale;

            
            Vector3 localScaleText = currentTextBox.transform.localScale;
            localScaleText.x = localScaleText.x * Mathf.Sign(mainIndividaulParameters[waypointindex].waypoint.transform.position.x - transform.position.x);
            currentTextBox.transform.localScale = localScaleText;
            Debug.LogWarning("Flip"); 
            /*
            if (mainIndividaulParameters[waypointindex].lookToTheLeftOnNextPoint == false)
            {
            Vector3 localScaleText = currentTextBox.transform.localScale;
            localScaleText.x *= -1f;
            currentTextBox.transform.localScale = localScaleText;
            Debug.LogWarning("Flip");
            } else return;
            */
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
            AudioClip audio = mainIndividaulParameters[waypointindex].audioClip;
            audiToPlay.clip = audio;
            audiToPlay.Play();
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
}

//Сам массив данных
[System.Serializable]
public class PointsSettings
{
    public string namePoint;
    [Tooltip("Название точки")]
    [Header("TextBubble")]
    public bool showTextWhileWalking;
    public GameObject textBox;
    public string text;
    [Header("Audio To play")]
    
    public AudioClip audioClip;
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