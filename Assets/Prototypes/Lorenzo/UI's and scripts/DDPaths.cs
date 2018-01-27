using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DDPaths {
    
    public string textPathDeath;
    public string textPathKiller;
    public string textPathScore;
    public string textPathWeaponReport;

    public List<string> paths = new List<string>();

    public DDPaths ()
    {
        string appPath = Application.dataPath;

        if (Application.platform == RuntimePlatform.OSXPlayer) appPath += "/../../";
        else if (Application.platform == RuntimePlatform.WindowsPlayer) appPath += "/../";
        else if (Application.platform == RuntimePlatform.WindowsEditor) appPath += "/../../";

        textPathDeath = appPath + "Death.txt";
        textPathKiller = appPath + "Killer.txt";
        textPathScore = appPath + "Score.txt";
        textPathWeaponReport = appPath + "WeaponReport.txt";

        paths.Add(textPathDeath);
        paths.Add(textPathKiller);
        paths.Add(textPathScore);
        paths.Add(textPathWeaponReport);
    }

    public void CreateTextFiles ()
    {
        foreach (string path in paths)
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {

                }
            }
        }
    }
}
