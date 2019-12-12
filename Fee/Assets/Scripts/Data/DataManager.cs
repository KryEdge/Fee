using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public string dataToSave;
    private string savePath;

    private string fileSaved;
    private static DataManager instance =  null;
    public static DataManager Instance
    {
        get
        {
            return instance;
        }

    }
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Che maestro, esto no funca");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public bool saveData(string filename,string data)
    {
        bool fileExisted = false;

        if (File.Exists(filename))
        {
            fileExisted = true;
        }

        savePath = Application.persistentDataPath;
        Debug.Log(savePath);

        fileSaved = Path.Combine(savePath,filename);

        FileStream fsw = File.Open(fileSaved,FileMode.Create);
        StreamWriter sw = new StreamWriter(fsw);

        dataToSave = data;

        sw.Write(dataToSave);

        sw.Flush();
        sw.Close();
        return fileExisted;
    }

    public bool loadData(string filename, out string data)
    {
        savePath = Application.persistentDataPath;
        fileSaved = Path.Combine(savePath, filename);

        if (File.Exists(fileSaved))
        {
            FileStream fsr = File.Open(fileSaved,FileMode.Open);
            StreamReader sr = new StreamReader(fsr);

            data = sr.ReadToEnd();

            sr.Close();
            return true;
        }
        else
        {
            data = null;
            return false;
        }    
    }
}