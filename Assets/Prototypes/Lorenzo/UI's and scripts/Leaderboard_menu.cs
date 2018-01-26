using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class Leaderboard_menu : MonoBehaviour
{
    public Text highscore_List;


    void Main()
    {
        const string f = "Assets/Prototypes/Sidi/Analytics/Score.txt";
        List<string> lines = new List<string>();

        using (StreamReader r = new StreamReader(f))
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }
        string newline;
        List<string> newlines = new List<string>();
        foreach (string value in lines)
        {
            newline = value.Replace(',',' ');
            newlines.Add(newline);

        }
        newlines.Sort((a, b) => -1 * a.CompareTo(b));

        highscore_List.text = "score: " + newlines[0] + "\nscore: " + newlines[1] + "\nscore: " + newlines[2];
    }
}
