using UnityEngine;
using TMPro;

public class TimeTracker : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text bestTimeText; // ← 追加：ハイスコア用

    private float elapsedTime = 0f;
    private bool isTiming = true;
    private float bestTime;

    void Start()
    {
        // 初期化：保存されたハイスコアを取得（なければ大きな値を設定）
        bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        if (bestTime < float.MaxValue)
        {
            bestTimeText.text = "Best: " + bestTime.ToString("F2") + "s";
        }
        else
        {
            bestTimeText.text = "Best: --.--s";
        }
    }

    void Update()
    {
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
            timeText.text = "Time: " + elapsedTime.ToString("F2") + "s";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Goal")
        {
            isTiming = false;
            timeText.text = "Goal! Time: " + elapsedTime.ToString("F2") + "s";

            // ハイスコア更新判定
            if (elapsedTime < bestTime)
            {
                bestTime = elapsedTime;
                PlayerPrefs.SetFloat("BestTime", bestTime);
                PlayerPrefs.Save();

                bestTimeText.text = "Best: " + bestTime.ToString("F2") + "s";
            }
        }
    }
}
