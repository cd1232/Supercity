using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveLoadHighScore {

    public static string FilePath = Application.persistentDataPath + "/savedHighScore.gd";

    public static void Save(float highScore)
    {
        if (File.Exists(FilePath))
        {
            string readContents = File.ReadAllText(FilePath);
            float oldHighScore;
            if (float.TryParse(readContents, out oldHighScore))
            {
                if (highScore > oldHighScore)
                {               
                    File.WriteAllText(FilePath, highScore.ToString());
                }
            }
        }
        else
        {
            StreamWriter file;
            file = File.CreateText(FilePath);
            file.WriteLine(highScore.ToString());
            file.Close();
        }

    }

    public static float Load()
    {
        float oldHighScore = -1;
        if (File.Exists(FilePath))
        {
            string readContents = File.ReadAllText(FilePath);

            if (float.TryParse(readContents, out oldHighScore))
            {}
        }
        return oldHighScore;
    }
	
}
