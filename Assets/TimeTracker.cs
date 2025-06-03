using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // ← シーン操作に必要

public class TimeTracker : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text bestTimeText;
    public TMP_Text goalMessageText;

    private float elapsedTime = 0f;
    private bool isTiming = true;
    private float bestTime;
    private bool goalReached = false;

    void Start()
    {
        bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        if (bestTime < float.MaxValue)
        {
            bestTimeText.text = "Best: " + bestTime.ToString("F2") + "s";
        }
        else
        {
            bestTimeText.text = "Best: --.--s";
        }

        goalMessageText.text = "";
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
    if (!goalReached && other.CompareTag("Goal"))
    {
        goalReached = true;
        isTiming = false;
        timeText.text = "Goal! Time: " + elapsedTime.ToString("F2") + "s";
        goalMessageText.text = "GOAL!!!";

        if (elapsedTime < bestTime)
        {
            bestTime = elapsedTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save();

            bestTimeText.text = "Best: " + bestTime.ToString("F2") + "s";
        }

        // ✅ 3秒後に再スタート
        Invoke("RestartScene", 3f);
    }

    // ✅ 敵と接触したら時間リセット
    if (other.CompareTag("Enemy"))
    {
        elapsedTime = 0f;
        timeText.text = "Time: 0.00s";

        // 任意：時間再スタートしたことを通知する表示なども可
        Debug.Log("敵と接触！時間をリセットしました。");
    }
}

}