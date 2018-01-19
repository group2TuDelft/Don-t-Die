using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class Leaderboard_menu : MonoBehaviour
{

    public TextAsset dataFile;
    public Text highscore_List;

    public void GetHighScoreList()
    {
        string[] dataLines = dataFile.text.Split('\n');
        for (int i = 0; i < dataLines.Length; i++)
        {
            dataLines[i] = dataLines[i].Replace(',',' ');
        }
        Array.Sort<string>(dataLines,
                    new Comparison<string>(
                            (i1, i2) => i2.CompareTo(i1)
                    ));

        highscore_List.text = "score: " + dataLines[0] + "\nscore: " + dataLines[1] + "\nscore: " + dataLines[2];
    }
}
