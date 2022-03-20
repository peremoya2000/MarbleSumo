using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _leaderboardSection;
    [SerializeField]
    private PlayfabManager _playfab;
    [SerializeField]
    private GameObject _fadePanel;
    private float _fade = 1;
    private Image _fadeImage;
    private bool _fadeOut;

    void Start()
    {
        _fadeOut = false;
        _fadeImage = _fadePanel.GetComponent<Image>();
        Time.timeScale = 0;
    }

    public void StartGame() {
        if (string.IsNullOrEmpty(_playfab.myName)) return;
        Time.timeScale = 1;
        _fadeOut = true;
        _fade = 0;
        _fadePanel.SetActive(true);
    }

    void Update(){
        if (!_fadeOut){
            if (_fade > 0){
                //Fade in
                _fade -= Time.unscaledDeltaTime*2.5f;
                _fadeImage.color = new Color(0, 0, 0, _fade);
                if (_fade < .05f) _fadePanel.SetActive(false);
            }
        }else{
            //Fade out
            _fade += Time.unscaledDeltaTime*2;
            _fadeImage.color = new Color(0, 0, 0, _fade);
            if (_fade > 1) SceneManager.LoadScene("GameplayMap1");
        }
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
