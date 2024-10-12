using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR;

public class UIHammer : MonoBehaviour
{
    public HammerUse hammerUse;

    [Header("Статистика")]
    public TextMeshProUGUI hammerMagazine;
    public TextMeshProUGUI hammerAllDrop;
    public TextMeshProUGUI timerHammer;
    public TextMeshProUGUI hammerDamage;

    [Header("Настройки для молотков")]
    public TextMeshProUGUI trowForce;
    [SerializeField] Slider trowForceSlider;
    [Space]
    public TextMeshProUGUI bounce;
    [SerializeField] Slider bounceSlider;
    [Space]
    public TextMeshProUGUI bounceForce;
    [SerializeField] Slider bounceForceSlider;
    [Space]
    public TextMeshProUGUI spin;
    [SerializeField] Slider spinSlider;
    [Space]
    public TextMeshProUGUI reload;
    [SerializeField] Slider reloadSlider;


    [Header("Настройка параметров")]
    [TextArea(5,5)]
    [SerializeField] string info;
    [Tooltip("Не забудьте добавить максимум в каждом слайдере, на который он ссылается! Здесь, ставится у слайдера bounceSlider, от 0 до 20")]
    [Range(0, 20)]
    public int maxBounce;
    [Tooltip("Не забудьте добавить максимум в каждом слайдере, на который он ссылается! Здесь, ставится у слайдера bounceForceSlider, от 0 до 2")]
    [Range(0, 2)]
    public float bounceForceInstrument;
    [Tooltip("Не забудьте добавить максимум в каждом слайдере, на который он ссылается! Здесь, ставится у слайдера trowForceSlider, от 0 до 20")]
    [Range(0, 5)]
    public float massforce;
    [Tooltip("Не забудьте добавить максимум в каждом слайдере, на который он ссылается! Здесь, ставится у слайдера spinSlider, от 0 до 40")]
    [Range(0, 40)]
    public float angularSpin;
    [Tooltip("Не забудьте добавить максимум в каждом слайдере, на который он ссылается! Здесь, ставится у слайдера reloadSlider, от 0 до 10")]
    [Range(0, 10)]
    public float reloadTime;



    private void Awake()
    {
        //это деффолтные значения молотка, если нужно то можно от них избавится в этом методе и ставить в инспекторе значение
        maxBounce = 5;
        bounceForceInstrument = 0.4f;
        massforce = 1;
        angularSpin = 1.5f;
        reloadTime = 5f;

        trowForceSlider.value = massforce;
        bounceSlider.value = maxBounce;
        bounceForceSlider.value = bounceForceInstrument;
        spinSlider.value = angularSpin;
        reloadSlider.value = reloadTime;
       
    }

    // Update is called once per frame
    void Update()
    {
        
        bounce.text = "Максимум отскоков " + maxBounce.ToString();
        trowForce.text = "Вес молотка " + massforce.ToString();
        bounceForce.text = "Сила отскока " + bounceForceInstrument.ToString();
        spin.text = "Сила АНТИкручения " + angularSpin.ToString();
        reload.text = "Время перезарядки" + hammerUse.reloadTime.ToString();
        timerHammer.text = hammerUse.reloadTimer.ToString();

        massforce = trowForceSlider.value;
        maxBounce = (int)bounceSlider.value;
        bounceForceInstrument = bounceForceSlider.value;
        angularSpin = spinSlider.value;
        hammerUse.reloadTime = reloadSlider.value;

        hammerMagazine.text = hammerUse.hammerCount.ToString();
        hammerAllDrop.text = hammerUse.allHammers.ToString();
        timerHammer.text = hammerUse.reloadTimer.ToString();
        hammerDamage.text = hammerUse.finalDamage.ToString();




    }
}
