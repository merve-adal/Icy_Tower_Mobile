using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OneWayPlatform : MonoBehaviour
{
    private Collider platformCollider;
    private Rigidbody playerRb;
    private Collider playerCollider;

    void Awake()
    {
        platformCollider = GetComponent<Collider>();
        platformCollider.isTrigger = true; // Trigger yapýyoruz, fizik motoru ile güvenli

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody>();
            playerCollider = player.GetComponent<Collider>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other != playerCollider) return;

        // Karakter aþaðý düþüyorsa (platformun üstüne iniyor) collider aktif
        if (playerRb.velocity.y <= 0f && playerCollider.bounds.min.y >= platformCollider.bounds.min.y)
        {
            platformCollider.isTrigger = false; // üstünde durabilir
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other != playerCollider) return;

        // Karakter düþmüyorsa veya platformun altýnda ? geçebilir
        if (playerRb.velocity.y > 0f || playerCollider.bounds.max.y < platformCollider.bounds.min.y)
        {
            platformCollider.isTrigger = true; // geçebilir
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other != playerCollider) return;

        // Platformdan çýktý ? tekrar trigger yap
        platformCollider.isTrigger = true;
    }
}
