using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    public string myName { get; private set; }
    [SerializeField]
    private TextMeshProUGUI _positionText;
    [SerializeField]
    private TextMeshProUGUI _rankingText,_scoresText;
    [SerializeField]
    private GameObject _namePanel;
    [SerializeField]
    private InputField _nameInputText;
    private string _myId; 


    void Start(){
        Login(); 
    }

    //Log into user with UID or create
    void Login() {
        var request = new LoginWithCustomIDRequest {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }
    void OnLoginSuccess(LoginResult result) {
        _myId = result.PlayFabId;
        myName = null;
        if(result.InfoResultPayload.PlayerProfile!=null)
            myName = result.InfoResultPayload.PlayerProfile.DisplayName;

        if (myName == null)
            _namePanel.SetActive(true);

        Debug.Log("Loged in!");
    }

    public void SubmitPlayerName() {
        if (string.IsNullOrEmpty(_nameInputText.text)) return;
        var request = new UpdateUserTitleDisplayNameRequest {
            DisplayName = _nameInputText.text
        };
        myName = _nameInputText.text;
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result) {
        Debug.Log("Name sent!");
        _namePanel.SetActive(false);
    }

    //Called by all playfab request functions on error
    void OnError(PlayFabError error) {
        Debug.Log("Error");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score) {
        var request = new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate{
                    StatisticName = "HighScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result) {
        Debug.Log("Score Sent!");
    }

    //Get current player leaderboard position
    public TextMeshProUGUI GetLeaderboardPosition() {
        var request = new GetLeaderboardAroundPlayerRequest { 
            StatisticName = "HighScore",
            MaxResultsCount = 3
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, CalculateLeaderboardPosition, OnError);
        return _positionText;
    }
    void CalculateLeaderboardPosition(GetLeaderboardAroundPlayerResult result){
        foreach (var item in result.Leaderboard) {
            if (item.Profile.PlayerId == _myId) {
                _positionText.gameObject.SetActive(true);
                _positionText.SetText("Ranking position: "+(item.Position+1));
                break;
            }
        }
    }

    //Get and display the leaderboard around player
    public void GetLeaderboard(){
        if (string.IsNullOrEmpty(myName)) return;
        var request = new GetLeaderboardAroundPlayerRequest{
            StatisticName = "HighScore",
            MaxResultsCount = 7
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, ShowLeaderboard, OnError);
    }
    void ShowLeaderboard(GetLeaderboardAroundPlayerResult result) {
        StringBuilder sb0 = new StringBuilder();
        StringBuilder sb1 = new StringBuilder();
        sb0.Append("  Players");
        sb1.Append("Score");
        foreach (var item in result.Leaderboard){
            sb0.Append("\n  ").Append((item.Position + 1).ToString()).Append("\t").Append(item.DisplayName);
            sb1.Append("\n").Append(item.StatValue);
        }
        _rankingText.SetText(sb0.ToString());
        _scoresText.SetText(sb1.ToString());
    }

}
