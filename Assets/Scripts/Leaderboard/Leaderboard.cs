using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    private string publicLeaderboardKey =
        "cba9a543c0e80244cf18d24bb52aa9c7e84f9d561224e733f9c44062e2fd750f";

    private void Start()
    {
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLenght = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLenght; ++i)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderBoardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((_) =>
        {
            GetLeaderboard();
        }));
        LeaderboardCreator.ResetPlayer();
    }
}
