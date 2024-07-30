using DataInfo;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]string dataPath;
    public void Initialized()
    {
        dataPath = Application.persistentDataPath + "/GameDatafile.dat";
    }

    public void Save(GameData gamedata)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(dataPath);

        GameData data = new GameData();
        data.KillCount = gamedata.KillCount;
        data.hp = gamedata.hp;
        data.speed = gamedata.speed;
        data.equipItem = gamedata.equipItem;
        bf.Serialize(file, data);
        file.Close();
    }

    public GameData Load()
    {
        if(File.Exists(dataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);

            file.Close();
            return data;
        }
        else
        {
            GameData data = new GameData();
            return data;
        }
    }
}
