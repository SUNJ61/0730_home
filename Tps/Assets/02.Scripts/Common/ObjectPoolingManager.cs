using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager poolingManager;
    private GameObject Bullet;
    private int MaxPool = 10; //������Ʈ Ǯ������ ������ ����
    public List<GameObject> bulletpoolList; //using System.Collections.Generic; Ȱ��ȭ ��

    private GameObject E_Bullet;
    private int E_MaxPool = 20;
    public List<GameObject> E_bulletpoolList;

    private GameObject EnemyPrefab;
    public List<GameObject> EnemyList;
    private int E_Spawn_MaxPool = 3;
    public List<Transform> SpawnPointList;

    private GameObject S_Bullet;
    private int S_MaxPool = 20;
    public List<GameObject> S_bulletpoolList;

    private GameObject swatPrefab;
    public List<GameObject> swatList;
    private int S_Spawn_MaxPool = 3;

    private string bullet = "Bullet";
    private string E_bullet = "E_Bullet";
    private string enemy = "Enemy";
    private string S_bullet = "S_Bullet";
    private string swat = "swat";
    void Awake()
    {
        if (poolingManager == null)
            poolingManager = this;
        else if (poolingManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Bullet = Resources.Load(bullet) as GameObject;
        E_Bullet = Resources.Load(E_bullet) as GameObject;
        EnemyPrefab = Resources.Load<GameObject>(enemy);
        S_Bullet = Resources.Load<GameObject>(E_bullet);
        swatPrefab = Resources.Load<GameObject>(swat);

        CreatBulletPool(); //������Ʈ Ǯ�� ���� �Լ�
        CreatE_BulletPool();
        CreatEnemyPool();
        CreatS_BulletPool();
        CreatswatPool();
    }
    private void Start() //������Ʈ Ǯ������ ���� ����Ʈ�� ������� start����
    {
        var spawnPoint = GameObject.Find("SpawnPoints"); //var�� ��������Ʈ ������Ʈ �Ҵ�
        if (spawnPoint != null) //var ��������Ʈ�� �����ٸ�
            spawnPoint.GetComponentsInChildren<Transform>(SpawnPointList); //������Ʈ�� �θ��ڽ��� ��� ����Ʈ�� ��´�.

        SpawnPointList.RemoveAt(0); //���� ���� ���� �θ� ������Ʈ ����
        if (SpawnPointList.Count > 0)
        {
            StartCoroutine(CreatEnemy());
            StartCoroutine(Creatswat());
        }
    }

    private void CreatEnemyPool()
    {
        GameObject EnemyGroup = new GameObject("EnemyGroup");
        for (int i = 0; i < E_Spawn_MaxPool; i++)
        {
            var enemyObj = Instantiate(EnemyPrefab, EnemyGroup.transform);
            enemyObj.name = $"{(i + 1).ToString()}����";
            enemyObj.SetActive(false);
            EnemyList.Add(enemyObj);
        }
    }

    IEnumerator CreatEnemy()
    {
        while(!GameManager.G_instance.isGameOver)
        {
            yield return new WaitForSeconds(3.0f);
            if (GameManager.G_instance.isGameOver) yield break; //���ӿ����� �ȴٸ�, startcoroutine�� ������.
            foreach(GameObject _enemy in EnemyList)
            {
                if(_enemy.activeSelf == false)
                {
                    int idx = Random.Range(0,SpawnPointList.Count);
                    _enemy.transform.position = SpawnPointList[idx].position;
                    _enemy.transform.rotation = SpawnPointList[idx].rotation;
                    _enemy.gameObject.SetActive(true);
                    break; //�Ѹ��� �¾�� foreach����
                }
            }
        }
    }

    private void CreatswatPool()
    {
        GameObject swatGroup = new GameObject("swatGroup");
        for (int i = 0; i < S_Spawn_MaxPool; i++)
        {
            var swatObj = Instantiate(swatPrefab, swatGroup.transform);
            swatObj.name = $"{(i + 1).ToString()}����";
            swatObj.SetActive(false);
            swatList.Add(swatObj);
        }
    }

    IEnumerator Creatswat()
    {
        while (!GameManager.G_instance.isGameOver)
        {
            yield return new WaitForSeconds(3.0f);
            if (GameManager.G_instance.isGameOver) yield break;
            foreach (GameObject _swat in swatList)
            {
                if (_swat.activeSelf == false)
                {
                    int idx = Random.Range(0, SpawnPointList.Count);
                    _swat.transform.position = SpawnPointList[idx].position;
                    _swat.transform.rotation = SpawnPointList[idx].rotation;
                    _swat.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    void CreatBulletPool()
    {
        GameObject PlayerBulletGroup = new GameObject("PlayerBulletGroup "); //���� ������Ʈ �Ѱ� ����
        for (int i = 0; i < MaxPool; i++)
        {
            var _bullet = Instantiate(Bullet, PlayerBulletGroup.transform); //�Ѿ� ������Ʈ�� 10���� PlayerBulletGroup �ȿ� ����
            _bullet.name = $"{(i+1).ToString()}��"; //������Ʈ �̸��� 1�� ���� 10�� ����
            _bullet.SetActive(false); //���� �Ȱ� ��Ȱ��ȭ
            bulletpoolList.Add(_bullet); // ������ �Ѿ� ����Ʈ�� �־���.
        }
    }
    public GameObject GetBulletPool()
    {
        for(int i = 0; i < bulletpoolList.Count; i++)
        {
            if(bulletpoolList[i].activeSelf == false) //�ش� ��°�� �Ѿ��� ��Ȱ��ȭ �Ǿ��ִٸ�
            {
                return bulletpoolList[i]; //��Ȱ��ȭ �Ǿ��ִٸ� �Ѿ� ��ȯ
            }
        }
        return null; //Ȱ��ȭ �Ǿ������� null�� ��ȯ
    }

    void CreatE_BulletPool()
    {
        GameObject EnemyBulletGroup = new GameObject("EnemyBulletGroup "); //���� ������Ʈ �Ѱ� ����
        for (int i = 0; i < E_MaxPool; i++)
        {
            var _bullet = Instantiate(E_Bullet, EnemyBulletGroup.transform); //�Ѿ� ������Ʈ�� 10���� PlayerBulletGroup �ȿ� ����
            _bullet.name = $"{(i + 1).ToString()}��"; //������Ʈ �̸��� 1�� ���� 10�� ����
            _bullet.SetActive(false); //���� �Ȱ� ��Ȱ��ȭ
            E_bulletpoolList.Add(_bullet); // ������ �Ѿ� ����Ʈ�� �־���.
        }
    }
    public GameObject GetE_BulletPool()
    {
        for (int i = 0; i < E_bulletpoolList.Count; i++)
        {
            if (E_bulletpoolList[i].activeSelf == false) //�ش� ��°�� �Ѿ��� ��Ȱ��ȭ �Ǿ��ִٸ�
            {
                return E_bulletpoolList[i]; //��Ȱ��ȭ �Ǿ��ִٸ� �Ѿ� ��ȯ
            }
        }
        return null; //Ȱ��ȭ �Ǿ������� null�� ��ȯ
    }

    void CreatS_BulletPool()
    {
        GameObject swatBulletGroup = new GameObject("swatBulletGroup "); //���� ������Ʈ �Ѱ� ����
        for (int i = 0; i < S_MaxPool; i++)
        {
            var s_bullet = Instantiate(S_Bullet, swatBulletGroup.transform); //�Ѿ� ������Ʈ�� 10���� PlayerBulletGroup �ȿ� ����
            s_bullet.name = $"{(i + 1).ToString()}��"; //������Ʈ �̸��� 1�� ���� 10�� ����
            s_bullet.SetActive(false); //���� �Ȱ� ��Ȱ��ȭ
            S_bulletpoolList.Add(s_bullet); // ������ �Ѿ� ����Ʈ�� �־���.
        }
    }
    public GameObject GetS_BulletPool()
    {
        for (int i = 0; i < S_bulletpoolList.Count; i++)
        {
            if (S_bulletpoolList[i].activeSelf == false) //�ش� ��°�� �Ѿ��� ��Ȱ��ȭ �Ǿ��ִٸ�
            {
                return S_bulletpoolList[i]; //��Ȱ��ȭ �Ǿ��ִٸ� �Ѿ� ��ȯ
            }
        }
        return null; //Ȱ��ȭ �Ǿ������� null�� ��ȯ
    }
}
