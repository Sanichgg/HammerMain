using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesScript : MonoBehaviour
{
    public GameObject minePrefab;
    public GameObject[] spawnLines;           
    public float spawnHeight = 10.0f;
    private int minMines = 3;                   
    private int maxMines = 6;                   

    private void Start()
    {
        // Инициализируем массив с заданными точками на сцене
        spawnLines = new GameObject[6];
        spawnLines[0] = GameObject.Find("Line1Left");
        spawnLines[1] = GameObject.Find("Line2Left");
        spawnLines[2] = GameObject.Find("Line3Left");
        spawnLines[3] = GameObject.Find("Line1Right");
        spawnLines[4] = GameObject.Find("Line2Right");
        spawnLines[5] = GameObject.Find("Line3Right");
    }

    public void SpawnMines()
    {
        int mineCount = Random.Range(minMines, maxMines + 1);

        for (int i = 0; i < mineCount; i++)
        {
            GameObject lineObject = spawnLines[Random.Range(0, spawnLines.Length)];
            SpriteRenderer lineRenderer = lineObject.GetComponent<SpriteRenderer>();

            if (lineRenderer != null)
            {
                float lineMinX = lineRenderer.bounds.min.x;
                float lineMaxX = lineRenderer.bounds.max.x;
                float randomX = Random.Range(lineMinX, lineMaxX);

                Vector3 spawnPosition = new Vector3(randomX, lineObject.transform.position.y + spawnHeight, 0);
                GameObject mineInstance = Instantiate(minePrefab, spawnPosition, Quaternion.identity);

                MineWork mineWork = mineInstance.GetComponent<MineWork>();
                if (mineWork != null)
                {
                    mineWork.SetTargetLine(lineObject);
                }
            }
        }
    }

}

