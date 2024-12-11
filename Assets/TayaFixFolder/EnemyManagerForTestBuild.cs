using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerForTestBuild : MonoBehaviour
{
    public float timeToSpawn;
    [SerializeField] private Transform[] spawnPonts;
    [SerializeField] private GameObject enemy;

    [SerializeField] private int finalTime = 3;

    public bool canSpawn = true;

    public float speed;

    [SerializeField] UIController uIController;

    void Start()
    {
        Spawn();
        StartCoroutine(Timer());
    }

    private void Update()
    {

    }

    private IEnumerator Timer()
    {
        if (canSpawn)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeToSpawn);
                Spawn();
                //Debug.Log("Spawn");
            }
        }
    }

    private void Spawn()
    {
        if (canSpawn)
        {
            int indexToSpawn = Random.Range(0, spawnPonts.Length);

            GameObject spawnedObject = Instantiate(enemy);
            spawnedObject.transform.position = spawnPonts[indexToSpawn].position;
            //spawnedObject.GetComponent<SkeletonAnimation>().gameObject.layer = spawnPonts[indexToSpawn].GetComponent<SpawnPoint>().LayerNumber;
            spawnedObject.GetComponent<MeshRenderer>().sortingOrder = spawnPonts[indexToSpawn].GetComponent<SpawnPoint>().LayerNumber;
        }
        else Debug.Log("NO");
    }

    public void Final(bool facingRight)
    {
        Debug.Log("wfwerwerqerewrqwerqwerewdwedqwdsw");
        PlayerAnimationController playerAnimationController = FindObjectOfType<PlayerAnimationController>();
        playerAnimationController.LoseHammerAnimation();
        if (facingRight) playerAnimationController.transform.localScale = -playerAnimationController.transform.localScale;

        canSpawn = false;
        StopCoroutine(Timer());
        
        foreach (EnemyBase enemy in FindObjectsOfType<EnemyBase>())
        {
            enemy.TheVictoryMarch(facingRight);
            
        }
        
        StartCoroutine(FinalTimer());
    }

    private IEnumerator FinalTimer()
    {
        yield return new WaitForSeconds(finalTime);
        uIController.ActivateGameOverPanel();
    }
}
