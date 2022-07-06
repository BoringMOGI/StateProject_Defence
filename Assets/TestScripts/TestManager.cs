using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

class Player
{
    public int score;        // 클래스 변수.
    public int number;       // 인스턴스 변수.

    public int Number => number;
}

public class TestManager : MonoBehaviour
{
    private void Start()
    {
        Player player = new Player() { score = 100, number = 200 };
        var enemy = new { name = "고블린", hp = 100, power = 20f };

        Player[] players = new Player[] {
            new Player{ score = 100, number = 50 },
            new Player{ score = 200, number = 60 },
            new Player{ score = 300, number = 70 },
        };

        int[] scores = players.Select(player => player.score).ToArray();
        foreach(int s in scores)
        {
            Debug.Log(s);
        }
    }

}
