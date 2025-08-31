using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OneWayPlatform : MonoBehaviour
{
    public static List<OneWayPlatform> TumPlatformlar = new List<OneWayPlatform>();
    public Transform karakter;
    private Collider col;
    public float aktifMesafe = 10f; // Karakterden bu mesafeye kadar platform aktif

    void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true; // Baþlangýçta geçilebilir
        TumPlatformlar.Add(this);

        if (karakter == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) karakter = player.transform;
        }
    }

    void OnDestroy()
    {
        TumPlatformlar.Remove(this);
    }

    void LateUpdate()
    {
        if (karakter == null) return;

        float mesafe = Vector3.Distance(new Vector3(transform.position.x, 0, 0),
                                        new Vector3(karakter.position.x, 0, 0));

        // Karakter yakýndaysa collider aktif/pasif durumu
        if (mesafe <= aktifMesafe)
        {
            if (karakter.position.y > transform.position.y + 0.1f)
                col.isTrigger = false; // üstünde durabilir
            else
                col.isTrigger = true;  // altýndan geçebilir
        }
        else
        {
            col.isTrigger = true;      // uzak platformlar hep geçilebilir
        }
    }
}
