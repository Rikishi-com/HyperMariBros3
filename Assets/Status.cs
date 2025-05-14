using UnityEngine;

public class Status : MonoBehaviour
{
    public float baseSpeed = 10f;
    private float currentSpeed;
    private float boostDuration = 5f;
    private bool isSpeedBoosted = false;

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ) * currentSpeed * Time.deltaTime;
        transform.Translate(move);
    }

    public void SpeedBoost()
    {
        if (!isSpeedBoosted)
        {
            isSpeedBoosted = true;
            currentSpeed = baseSpeed * 1.5f; // 50%アップ
            Invoke(nameof(ResetSpeed), boostDuration);
        }
    }

    private void ResetSpeed()
    {
        currentSpeed = baseSpeed;
        isSpeedBoosted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedBoost"))
        {
            SpeedBoost();
            Destroy(other.gameObject);
        }
    }
}