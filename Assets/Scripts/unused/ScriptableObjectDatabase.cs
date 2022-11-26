using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 
using System.Linq;

// Fetches item databases that inherit from this class so that items within them can be used mostly in the editor

public class ScriptableObjectDatabase<T> : ScriptableObject where T: class
{
    [SerializeField] protected List<T> item = new List<T>();

    public List<T> Item
    {
        get 
        { 
            return item; 
        }
    }

    // these two aren't entirely needed anymore but oh well
    // returns database count
    public int Count
    {
        get { return item.Count; }
    }

    // fetches database index
    public T Get(int index)
    {
        return item.ElementAt(index);
    }

#if UNITY_EDITOR
    // add item to database from editor
    public void Add(T i)
    {
        item.Add(i);
        EditorUtility.SetDirty(this);
    }

    // insert existing item to database editor
    public void Insert(int index, T i)
    {
        item.Insert(index, i);
        EditorUtility.SetDirty(this);
    }

    // remove item from detabase using the editor
    public void Remove(T i)
    {
        item.Remove(i);
        EditorUtility.SetDirty(this);
    }

    // remove item but this time the input is based on the index of the item
    public void Remove(int index)
    {
        item.RemoveAt(index);
        EditorUtility.SetDirty(this);
    }

    // replace object in database based on index of previous
    public void Replace(int index, T i)
    {
        item[index] = i;
        EditorUtility.SetDirty(this);
    }

    // returns reference to the database
    public static U GetDatabase<U>(string db_Path, string db_Name) where U : ScriptableObject
    {
        string dbFullPath = @"Assets/" + db_Path + "/" + db_Name;

        U db = AssetDatabase.LoadAssetAtPath(dbFullPath, typeof(U)) as U;
        if (db == null)
        {
            // check to see if folder exists
            if (!AssetDatabase.IsValidFolder(@"Assets/" + db_Path))
            {
                AssetDatabase.CreateFolder(@"Assets", db_Path);

                // create database and refresh assetDatabase
                db = ScriptableObject.CreateInstance<U>() as U;
                AssetDatabase.CreateAsset(db, dbFullPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        // NOTE, DOESN'T CARE IF THE FILE DOESN'T EXIST IT *WILL* RECREATE IT
       /* if (AssetDatabase.IsValidFolder(@"Assets/" + db_Path))
        {
            if (!AssetDatabase.IsValidFolder(@"Assets/" + db_Path + db_Name))
            {
                // create database and refresh assetDatabase
                db = ScriptableObject.CreateInstance<U>() as U;
                AssetDatabase.CreateAsset(db, dbFullPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        } */
        return db;
    }
#endif 
}
