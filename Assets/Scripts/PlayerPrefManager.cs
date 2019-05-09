using UnityEngine;
using System.Collections;

public static class PlayerPrefManager{
    public static void SetHighscore(int highscore)
    {
        PlayerPrefs.SetInt("HighScore", highscore);
    }

    public static int GetHighscore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
            return PlayerPrefs.GetInt("HighScore");
        else
            return 0;
    }
}
