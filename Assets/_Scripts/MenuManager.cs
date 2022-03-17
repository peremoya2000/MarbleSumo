using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _leaderboardSection;
    [SerializeField]
    private PlayfabManager _playfab;

    void Start()
    {
        Time.timeScale = 0;
    }

    public void StartGame() {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameplayMap1");
    }

    public void OpenLeaderboards() {
        _leaderboardSection.SetActive(true);
        _playfab.GetLeaderboard();
    }

    public void CloseLeaderboards() {
        _leaderboardSection.SetActive(false);
    }

    public void CloseGame() {
        Application.Quit();
    }

}
