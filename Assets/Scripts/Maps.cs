﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Maps
{
    public static Dictionary<string, string> maps;

    public static int mapSizeX = 40;
    public static int mapSizeY = 20;

    private static string path = "Assets/Resources/Maps/";

    static Maps()
    {
        maps = new Dictionary<string, string>();
        maps.Add("map_0", ReadMap("map_0"));
        maps.Add("map_1", ReadMap("map_1"));
    }

    private static string ReadMap(string mapName)
    {
        StreamReader reader = new StreamReader(path + mapName + ".txt");
        string map = reader.ReadToEnd().Replace(Environment.NewLine, "");
        reader.Close();
        return map;
    }

    public static string[,] StringTo2DArray(string input)
    { 
        var n = mapSizeX;
        var split = input
            .Select((c, i) => new { letter = c, group = i / n })
            .GroupBy(l => l.group, l => l.letter)
            .Select(g => string.Join("", g))
            .ToArray();

        var result = new string[mapSizeX,mapSizeY];
        for (var yIndex = 0; yIndex < mapSizeY; yIndex++)
        {
            for (var xIndex = 0; xIndex < mapSizeX; xIndex++)
            {
                result[xIndex, yIndex] = split[yIndex][xIndex].ToString();
            }
        }
        
        return result;
    }
}