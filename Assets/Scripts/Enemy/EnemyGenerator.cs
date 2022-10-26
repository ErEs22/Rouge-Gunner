using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;

    [SerializeField] float generatorInterval = 5f;

    Room mainRoom;

    WaitForSeconds waitForGenerateEnemy;

    private void Awake()
    {
        waitForGenerateEnemy = new WaitForSeconds(generatorInterval);
        mainRoom = GetComponentInParent<Room>();
    }

    public void StartGenerateEnemys()
    {
        StartCoroutine(nameof(EnemyGenerateCoroutine));
    }

    IEnumerator EnemyGenerateCoroutine()
    {
        foreach (GameObject enemy in enemyPrefabs)
        {
            GameObject newEnemy = PoolManager.Release(enemy, transform.position, Quaternion.identity);
            mainRoom.enemys.Add(newEnemy);
            yield return waitForGenerateEnemy;
        }

        gameObject.SetActive(false);
        yield break;
    }
}
