using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UnityEngine.Random ile System.Random çakışmasını önlemek için alias
using Random = UnityEngine.Random;

public class ZeminGenerator : MonoBehaviour
{
    [Header("Platform Ayarları")]
    public Transform zemin;           // Sahnedeki ilk zemin platformu
    public int zeminSayisi = 20;
    public float zeminGenislik = 3f;
    public float minimumY = 2f;
    public float maksimumY = 4f;
    public float minimumXMesafe = 1f;

    [Header("Karakter Zıplama Ayarı")]
    public Transform karakter;
    public float karakterZiplamaGucu = 5f;
    public float yercekimi = 9.81f;

    void Start()
    {
        if (karakter == null)
        {
            karakter = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Başlangıç platformu
        Vector3 baslangicPozisyon = new Vector3(1.77f, -2.3f, 0f);
        Vector3 spawnKonumu = new Vector3(
            baslangicPozisyon.x,
            baslangicPozisyon.y + zemin.localScale.y / 2f,
            baslangicPozisyon.z
        );

        float sonX = spawnKonumu.x;

        for (int i = 0; i < zeminSayisi; i++)
        {
            float maxY = Mathf.Min(maksimumY, karakterZiplamaGucu * 0.9f);
            float minY = Mathf.Max(minimumY, karakterZiplamaGucu * 0.5f);
            spawnKonumu.y += Random.Range(minY, maxY);

            float yeniX;
            int deneme = 0;
            do
            {
                float maxXMesafe = karakterZiplamaGucu;
                yeniX = sonX + Random.Range(-maxXMesafe, maxXMesafe);
                yeniX = Mathf.Clamp(yeniX, -zeminGenislik, zeminGenislik);
                deneme++;
                if (deneme > 20) break;
            } while (Mathf.Abs(yeniX - sonX) < minimumXMesafe);

            spawnKonumu.x = yeniX;
            sonX = yeniX;

            GameObject yeniZemin = Instantiate(zemin.gameObject, spawnKonumu, Quaternion.identity);

            Collider collider = yeniZemin.GetComponent<Collider>();
            if (collider == null)
            {
                collider = yeniZemin.AddComponent<BoxCollider>();
            }

            yeniZemin.AddComponent<OneWayPlatform>();
        }
    }
}
