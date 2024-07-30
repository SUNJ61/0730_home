using DataInfo;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
//enemy�� �¾�� ������ ���Ҿ� ������ü�� �ƿ츣�� ����� ����, �� ���� ��ü�� ���� Ŭ����
//1. �� ������, 2. �¾ ��ġ 3. �ð� ����(���ʸ��� �¾�°�) 4. ��� ���� �¾�°�
//���ӸŴ����� ���� ��ü�� ��Ʈ�� �ؾ��ϹǷ� ������ �������, static������ ������ �� ������ ��ǥ�ؼ� ���ӸŴ����� �����ϵ��� �Ѵ�.
//�̰��� ���к��� ��ü ������ ���� �ϳ��� �����ϰ� �ϴ� ����̴�. -> �̱��� ���
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Text Killtxt;
    public static int Killcount = 0;

    public GameObject Zom_fb; //1��
    public GameObject Monster_fb;
    public GameObject Skel_fb;
    public Transform[] point; //2��
    public GameObject[] Mob;
    private CanvasGroup Inventory;

    private DataManager dataManager;
    [SerializeField]public GameData gameData;

    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;
    [SerializeField] private GameObject slotList;
    public GameObject[] itemObj;

    private float timePrev_Z; //3�� - ���Ž� �����
    private float timePrev_M; //�迭�� ������ ����ҵ�
    private float timePrev_S;
    private float timePrev;

    private float SpawnTime_Z = 3.0f; //3�� - 3�ʰ���
    private float SpawnTime_M = 8.0f;
    private float SpawnTime_S = 5.0f;
    private float SpawnTime = 3.0f;

    private int MaxCount_Z = 10; //4��
    private int MaxCount_M = 3;
    private int MaxCount_S = 5;
    private int MaxCount_MOB = 10;

    private string Zom = "ZOMBIE";
    private string Mon = "MONSTER";
    private string Skel = "SKELETON";
    private string mob = "MOB";
    private string SP = "SpawnPoints";
    public string player = "Player";
    private string invenStr = "Inventory";

    private bool IsOpen = false;
    void Awake()
    {
        if(Instance == null)
            Instance = this; // ��ü�� 1���� ����, Instance�� ���� �ش� Ŭ�����ȿ� public���� ������ ������ �żҵ忡 ���� �� �� �ִ�.
        else if(Instance != this)
            Destroy(Instance);
        DontDestroyOnLoad(Instance);

        //���̶�Ű���� SpawnPoints��� ������Ʈ�� ã�� �� ������Ʈ�� ���� ��ġ ������Ʈ���� �����Ѵ�.(##�ڱ� �ڽ���ġ ���� ����##)
        point = GameObject.Find(SP).GetComponentsInChildren<Transform>(); //���� �Ҵ�
        timePrev_Z = Time.time; // ������Ʈ�� �������� ���Žð��� ��.
        timePrev_M = Time.time;
        timePrev_S = Time.time;
        timePrev = Time.time;

        Inventory = GameObject.Find(invenStr).GetComponent<CanvasGroup>();

        dataManager = GetComponent<DataManager>();
        dataManager.Initialized();

        Inventory.blocksRaycasts = IsOpen;
        Inventory.alpha = 0f;
    }


    void Update()
    {
        //Spawn_Zom();
        //Spawn_Mon();
        //Spawn_Skel();
        //Spawn_Mob();//Ǯ���Ŵ����� �� �������� ��ü
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPaused();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            OnInventory(IsOpen);
        }
    }
    private void LoadGameData()
    {
        GameData data = dataManager.Load();
        gameData.hp = data.hp;
        gameData.damage = data.damage;
        gameData.speed = data.speed;
        gameData.KillCount = data.KillCount;
        gameData.equipItem = data.equipItem;

        if(gameData.equipItem.Count > 0)
            InventorySetUp();

        Killtxt.text = $"KILL : <color=#FF0000>{gameData.KillCount.ToString()}</color>";
    }
    void InventorySetUp()
    {
        var slots = slotList.GetComponentsInChildren<Transform>();
        for (int i = 0; i < gameData.equipItem.Count; i++)
        {
            for (int j = 1; j < slots.Length; j++)
            {
                if (slots[j].childCount > 0) continue;
                int itemIdex = (int)gameData.equipItem[i].itemType;
                itemObj[itemIdex].GetComponent<Transform>().SetParent(slots[j].transform);
                itemObj[itemIdex].GetComponent<ItemInfo>().itmeData = gameData.equipItem[i];
                break;
            }
        }
    }
    void SaveGameData()
    {
        dataManager.Save(gameData);
    }
    public void AddItem(Item item)
    {
        if (gameData.equipItem.Contains(item)) return;
        gameData.equipItem.Add(item);

        switch (item.itemType)
        {
            case (Item.ItemType.HP):
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp += item.value;
                else
                    gameData.hp += gameData.hp * item.value;
                break;

            case (Item.ItemType.DAMAGE):
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage += item.value;
                else
                    gameData.damage += gameData.damage * item.value;
                break;

            case (Item.ItemType.SPEED):
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed += item.value;
                else
                    gameData.speed += gameData.speed * item.value;
                break;

            case (Item.ItemType.GRANADE):
                break;
        }
        OnItemChange();
    }
    public void RemoveItem(Item item)
    {
        gameData.equipItem.Remove(item);

        switch (item.itemType)
        {
            case (Item.ItemType.HP):
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp -= item.value;
                else
                    gameData.hp = gameData.hp / (1f + item.value);
                break;

            case (Item.ItemType.DAMAGE):
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage -= item.value;
                else
                    gameData.damage = gameData.damage / (1f + item.value);
                break;

            case (Item.ItemType.SPEED):
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed -= item.value;
                else
                    gameData.speed = gameData.speed / (1f + item.value);
                break;

            case (Item.ItemType.GRANADE):
                break;
        }
        OnItemChange();
    }
    public void KillScore()
    {
        ++gameData.KillCount;
        Killtxt.text = $"KILL : <color=#FF0000>{gameData.KillCount.ToString()}</color>";
    }

    #region ������ ���� ��������
    private void Spawn_Mob()
    {
        timePrev += Time.deltaTime;
        int mobcounter = GameObject.FindGameObjectsWithTag(mob).Length;
        if (timePrev > SpawnTime)
        {
            if (mobcounter < MaxCount_MOB)
            {
                int pos = Random.Range(1, point.Length);
                int i = Random.Range(0, Mob.Length);
                Instantiate(Mob[i].gameObject, point[pos].position, point[pos].rotation);
                timePrev = 0f;
            }
        }
    }

    private void Spawn_Skel()
    {
        if (Time.time - timePrev_S > SpawnTime_S)
        {
            int Skelcounter = GameObject.FindGameObjectsWithTag(Skel).Length;
            if (Skelcounter < MaxCount_S)
            {
                int randPos_S = Random.Range(1, point.Length);
                Instantiate(Skel_fb, point[randPos_S].position, point[randPos_S].rotation);
                timePrev_S = Time.time;
            }
        }
    }

    private void Spawn_Mon()
    {
        if (Time.time - timePrev_M > SpawnTime_M)
        {
            int Moncounter = GameObject.FindGameObjectsWithTag(Mon).Length;
            if (Moncounter < MaxCount_M)
            {
                int randPos_M = Random.Range(1, point.Length);
                Instantiate(Monster_fb, point[randPos_M].position, point[randPos_M].rotation);
                timePrev_M = Time.time;
            }
        }
    }

    private void Spawn_Zom()
    {
        if (Time.time - timePrev_Z >= SpawnTime_Z)
        {
            //���̶�Ű���� ���� �±׸� ���� ������Ʈ�� ������ ī��Ʈ�ؼ� �ѱ�
            int Zomcounter = GameObject.FindGameObjectsWithTag(Zom).Length;
            if (Zomcounter < MaxCount_Z)
            {
                int randPos_Z = Random.Range(1, point.Length);
                Instantiate(Zom_fb, point[randPos_Z].position, point[randPos_Z].rotation);
                timePrev_Z = Time.time; //���Žð� ������Ʈ
            }
        }
    }
    #endregion
    private bool isPaused = false;
    public void OnPaused()
    {
        isPaused = !isPaused;
        Time.timeScale = (isPaused) ? 0f : 1f;
        var player = GameObject.FindGameObjectWithTag("Player");
        var scripts = player.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = !isPaused;
        }
    }
    public void OnInventory(bool isOpen)
    {
        IsOpen = !IsOpen;
        isOpen = IsOpen;

        var playerObj = GameObject.FindGameObjectWithTag(player);
        var scripts = playerObj.GetComponents<MonoBehaviour>();

        foreach(var script in scripts)
            { script.enabled = !isOpen; }

        Cursor.visible = isOpen;
        Cursor.lockState = CursorLockMode.None;

        
        Time.timeScale = (isOpen) ? 0f : 1f;
        Inventory.blocksRaycasts = isOpen;
        Inventory.alpha = isOpen ? 1f : 0f;
    }
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
