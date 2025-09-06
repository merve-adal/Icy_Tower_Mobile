using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider col;
    public Transform karakter;

    void Awake()
    {
        col = GetComponent<Collider>();
        if (karakter == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) karakter = player.transform;
        }
    }

    void Update()
    {
        if (col == null || karakter == null) return;

        // Karakter platformun üstündeyse collider açýk kalsýn
        if (karakter.position.y >= transform.position.y - 0.1f)
        {
            col.enabled = true;
        }
        else
        {
            // Karakter alttan geçmek isterse collider kapansýn
            col.enabled = false;
        }
    }
}
