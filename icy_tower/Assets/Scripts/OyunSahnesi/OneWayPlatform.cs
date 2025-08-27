using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider col;
    private Transform karakter;

    void Start()
    {
        col = GetComponent<Collider>();
        karakter = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (karakter == null || col == null) return;

        if (karakter.position.y > transform.position.y + 0.1f)
        {
            col.enabled = true; // üstünde durabilir
        }
        else
        {
            col.enabled = false; // altýndan geçebilir
        }
    }
}
