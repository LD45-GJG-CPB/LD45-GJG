using System;
using Themes;
using UnityEngine;

public static class GameState
{
    public static string PlayerName;
    public static Highscore PlayerLastHighscore;
    public static Difficulty CurrentDifficulty;
    public static bool isBonusMaps = false;
    public static int Score = 0;
    public static TextFont Font = TextFont.FiraCode;
    public static Theme CurrentTheme => ThemeManager.Instance.CurrentTheme;
}