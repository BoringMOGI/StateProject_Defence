using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameInfoUI : MonoBehaviour
{
    // 숫자를 3자리 단위 쉼표(,)로 구분하기 위한 포맷.
    private const string NUMBER_FORMAT = "#,##0";

    public Text goldText;
    public Text hpText;
    public Text waveText;

    public void UpdateInfo(int gold, int hp, int wave)
    {
        goldText.text = gold.ToString(NUMBER_FORMAT);
        hpText.text = hp.ToString(NUMBER_FORMAT);
        waveText.text = wave.ToString(NUMBER_FORMAT);
    }
}
