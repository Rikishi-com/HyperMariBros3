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
            goalMessageText.text = "ゴールおめでとう！";

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
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}