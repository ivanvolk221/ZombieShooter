using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerZombies : MonoBehaviour
{
    [Header("Settings Spawner")]
    [SerializeField] private int _countSpawningZombie;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private GameObject _parent;

    [Header("Settings Easy Zombie")]
    [SerializeField] private GameObject _easyZombiePrefab;
    [SerializeField] private float _easyPercentSpawn;


    private GameObject spawningZombies;
    private float Chanse;
    private Transform position;
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < _countSpawningZombie; i++)
        {
            Chanse = Random.Range(0f,100f);
            if(Chanse < _easyPercentSpawn)
            {
                spawningZombies = _easyZombiePrefab;
            }   
            Instantiate(spawningZombies, new Vector3(Random.Range(0f - _spawnRadius, 0f + _spawnRadius) + transform.position.x, Random.Range(0f - _spawnRadius, 0f + _spawnRadius) + transform.position.y), Quaternion.Euler(new Vector3()), _parent.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
