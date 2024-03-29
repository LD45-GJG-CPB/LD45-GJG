﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreNameInput : MonoBehaviour
{

    public TMP_InputField InputField;
    public TextMeshProUGUI nameInput;
    public String nextScene;
    
    // Start is called before the first frame update
    void Start()
    {
        InputField.Select();
        InputField.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SaveName();
        }
    }

    public void SaveName()
    {
        var diff = GameState.CurrentDifficulty.difficultyName;
        var score = GameState.Score;
        GameState.Score = 0;
        var gamertag = nameInput.text;

        if (string.IsNullOrWhiteSpace(gamertag) || gamertag.Length < 2)
        {
            gamertag = "gamer";
        }

        Debug.Log($"[HighscoreNameInput] received name: {gamertag}");
        Debug.Log($"[HighscoreNameInput] received score: {score}");

        StartCoroutine(HighScoreApi.Save(gamertag, score, diff, s =>
        {
            var response = JsonUtility.FromJson<Response>(s);
            GameState.PlayerLastHighscore = response.data;
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }));
    }
    
    [Serializable]
    public class Response
    {
        public Highscore data;
    }
}
