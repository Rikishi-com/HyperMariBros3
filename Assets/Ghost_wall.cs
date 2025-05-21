using UnityEngine;

public class Ghost_wall : MonoBehaviour
{
    private Renderer wallRenderer;
    private Collider wallCollider;

    private float timer = 0f;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
        wallCollider = GetComponent<Collider>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 10秒周期の1秒間だけ消す（10~11, 20~21, ...）
        if (timer % 10f >= 0f && timer % 10f < 1f)
        {
            wallRenderer.enabled = false;
            wallCollider.enabled = false;
        }
        else
        {
            wallRenderer.enabled = true;
            wallCollider.enabled = true;
        }
    }
}