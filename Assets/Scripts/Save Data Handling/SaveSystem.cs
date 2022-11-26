using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SavePlayer(PlayerMovement player)
    {
        // saves data of the player (including lvl, health, xp, etc)
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.sus";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        // making the same variable again like a boss
        string path = Application.persistentDataPath + "/playerData.sus";
        if (File.Exists(path))
        {
            // load data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveDungeon(DungeonGeneration dungeon)
    {
        // saves dungeon level and floor because gosh darn it i need those
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/dungeonData.sus";
        FileStream stream = new FileStream(path, FileMode.Create);

        DungeonData data = new DungeonData(dungeon);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static DungeonData LoadDungeon()
    {
        // making the same variable again like a boss
        string path = Application.persistentDataPath + "/dungeonData.sus";
        if (File.Exists(path))
        {
            // load data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            DungeonData data = formatter.Deserialize(stream) as DungeonData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    // check if a save file for the player exists and if it doesn't, return false 
    public static bool DoesPlayerFileExist(bool exists)
    {
        string path = Application.persistentDataPath + "/playerData.sus";
        if (File.Exists(path))
        {
            Debug.Log("exists");
            return true;
        }
        else
        {
            Debug.Log("Doesn't exist for some reason");
            return false;
        }
    }

    // the same as above but with the dungeonData file instead 
    public static bool DoesDungeonFileExist(bool exists)
    {
        string path = Application.persistentDataPath + "/dungeonData.sus";
        if (File.Exists(path))
        {
            exists = true;
            return exists;
        }
        else
        {
            exists = false;
            return exists;
        }
    }

    // check for trigger file
    public static bool DoesTriggerFileExist(bool exists)
    {
        string path = Application.persistentDataPath + "/triggerData.sus";
        if (File.Exists(path))
        {
            exists = true;
            return exists;
        }
        else
        {
            exists = false;
            return exists;
        }
    }

    // initially create a player file with data and do nothing if there is one
    public static void CreatePlayerFile(PlayerMovement player)
    {
        string path = Application.persistentDataPath + "/playerData.sus";
        if (!File.Exists(path))
        {
            Debug.Log("PlayerData doesn't exist, creating!");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            Debug.Log("playerData exists, doing nothing!");
            return;
        }
    }

    // create dungeon file if there isn't one
    public static void CreateDungeonFile(DungeonGeneration dungeon)
    {
        string path = Application.persistentDataPath + "/dungeonData.sus";
        if (!File.Exists(path))
        {
            Debug.Log("dungeonData doesn't exist, creating!");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            DungeonData data = new DungeonData(dungeon);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            Debug.Log("dungeonData exists, doing nothing!");
            return;
        }
    }

    public static void CreateTriggerFile(TriggerHandling trigger)
    {
        string path = Application.persistentDataPath + "/triggerData.sus";
        if (!File.Exists(path))
        {
            Debug.Log("triggerData doesn't exist, creating!");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            TriggerData data = new TriggerData(trigger);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            Debug.Log("triggerData exists, doing nothing!");
            return;
        }
    }

    public static void SaveBattleData(Unit player)
    {
        // overwrite original player data updated data from the battle
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.sus";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // remove data file for player upon gameover
    public static void DeletePlayerData()
    {
        string path = Application.persistentDataPath + "/playerData.sus";
        File.Delete(path);
    }

    // deletio the dungeonio
    public static void DeleteDungeonData()
    {
        string path = Application.persistentDataPath + "/dungeonData.sus";
        File.Delete(path);
    }

    // delete trigger data
    public static void DeleteTriggerData()
    {
        string path = Application.persistentDataPath + "/triggerData.sus";
        File.Delete(path);
    }

    public static void SaveTriggerData(TriggerHandling trigger)
    {
        // saves data of trigger database (an array of bools) - should probably be called when a trigger is... well, triggered
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/triggerData.sus";
        FileStream stream = new FileStream(path, FileMode.Create);

        TriggerData data = new TriggerData(trigger);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static TriggerData LoadTriggerData()
    {
        // you'd think at this point i wouldn't continually redefine path but no
        string path = Application.persistentDataPath + "/triggerData.sus";
        if (File.Exists(path))
        {
            // load
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            TriggerData data = formatter.Deserialize(stream) as TriggerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
