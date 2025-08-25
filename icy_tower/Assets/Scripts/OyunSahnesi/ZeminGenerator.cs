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
    public float karakterZiplamaGucu = 5f; // Karakter zıplama gücü
    public float yercekimi = 9.81f;

    void Start()
    {
        // Başlangıç platformu
        Vector3 baslangicPozisyon = new Vector3(1.76999998f, -2.29999995f, 0f);
        Vector3 spawnKonumu = new Vector3(baslangicPozisyon.x, baslangicPozisyon.y + zemin.localScale.y / 2f, baslangicPozisyon.z);

        float sonX = spawnKonumu.x;

        for (int i = 0; i < zeminSayisi; i++)
        {
            // Y ekseninde mesafe: karakterin zıplama yüksekliği ile uyumlu
            float maxY = Mathf.Min(maksimumY, karakterZiplamaGucu * 0.9f);
            float minY = Mathf.Max(minimumY, karakterZiplamaGucu * 0.5f);
            spawnKonumu.y += Random.Range(minY, maxY);

            // X eksenini belirle, üst üste gelmemesi için son platforma göre mesafe
            float yeniX;
            int deneme = 0;
            do
            {
                // Karakterin ulaşabileceği mesafede sağ-sol rastgele
                float maxXMesafe = karakterZiplamaGucu; // 1 saniyede yatay mesafe tahmini
                yeniX = sonX + Random.Range(-maxXMesafe, maxXMesafe);

                // Zemin genişliği sınırları
                yeniX = Mathf.Clamp(yeniX, -zeminGenislik, zeminGenislik);

                deneme++;
                if (deneme > 20) break; // sonsuz döngü engeli
            } while (Mathf.Abs(yeniX - sonX) < minimumXMesafe); // üst üste binmesin

            spawnKonumu.x = yeniX;
            sonX = yeniX;

            // Platformu oluştur
            Instantiate(zemin.gameObject, spawnKonumu, Quaternion.identity);
        }
    }
}
