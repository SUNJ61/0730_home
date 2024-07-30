using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager P_instance;
    private FPS_Damage Player;
    
    public GameObject Bullet;
    public List<GameObject> BulletList;
    private int bulletMax = 10;
    private string BulletStr = "_Bullet";

    private GameObject[] EnemyPrefabs;
    public List<GameObject> EnemyList;
    private int MaxEnemy = 10;
    private string EnemyFile = "Enemy";

    public List<Transform> SpawnList;
    private float SpawnTime = 3.0f;
    private string SP = "SpawnPoints";
    void Awake()
    {
        if(P_instance == null)
            P_instance = this;
        else if(P_instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Bullet =  Resources.Load(BulletStr) as GameObject;
        EnemyPrefabs = Resources.LoadAll<GameObject>(EnemyFile);
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPS_Damage>();
        //SpawnBullet();
        //SpawnEnemy();
    }
    private void Start()
    {
        var spawnPoint = GameObject.Find(SP);
        if (spawnPoint != null)
            spawnPoint.GetComponentsInChildren<Transform>(SpawnList);

        //SpawnBullet();
        SpawnEnemy();

        SpawnList.RemoveAt(0);
        if (SpawnList.Count > 0)
        {
            StartCoroutine(CreatMob());
        }
    }
    IEnumerator CreatMob()
    {
        while(!Player.isPlayerDie)
        {
            yield return new WaitForSeconds(SpawnTime);
            if(Player.isPlayerDie) yield break;
            foreach (GameObject Mob in EnemyList)
            {
                if(Mob.activeSelf == false)
                {
                    int idx = Random.Range(0, SpawnList.Count);
                    Mob.transform.position = SpawnList[idx].position;
                    Mob.transform.rotation = SpawnList[idx].rotation;
                    Mob.gameObject.SetActive(true);


                    break;
                }
            }
        }
    }
    void SpawnBullet()
    {
        GameObject PlayerBullet = new GameObject("PlayerBullet");
        for(int i = 0; i < bulletMax; i++)
        {
            var _bullet = Instantiate(Bullet, PlayerBullet.transform);
            _bullet.name = $"{(i + 1).ToString()}¹ß";
            _bullet.SetActive(false);
            BulletList.Add(_bullet);
        }
    }

    public GameObject GetBullet()
    {
        for(int i = 0; i < BulletList.Count; i++)
        {
            if(BulletList[i].activeSelf == false)
                return BulletList[i];
        }
        return null;
    }

    void SpawnEnemy()
    {
        GameObject enemy = new GameObject("Enemy");
        for(int i = 0; i < MaxEnemy; i++)
        {
            var _enemy = Instantiate(EnemyPrefabs[Random.Range(0,EnemyPrefabs.Length)], enemy.transform);
            _enemy.name = $"{(i + 1).ToString()}¸¶¸®";
            _enemy.SetActive(false);
            EnemyList.Add(_enemy);
        }
    }

    public GameObject GetEnemy()
    {
        for(int i = 0; i <  EnemyList.Count; i++)
        {
            if (EnemyList[i].activeSelf == false)
                return EnemyList[i];
        }
        return null;
    }
}
