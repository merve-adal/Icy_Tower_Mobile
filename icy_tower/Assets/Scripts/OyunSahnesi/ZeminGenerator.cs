using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class ZeminGenerator : MonoBehaviour
{
    [Header("Platform Ayarları")]
    public Transform zemin;
    public int zeminSayisi = 20;
    public float zeminGenislik = 3f;
    public float minimumY = 2f;
    public float maksimumY = 4f;
    public float minimumXMesafe = 1f;

    [Header("Karakter Zıplama Ayarı")]
    public Transform karakter;
    public float karakterZiplamaGucu = 5f;
    public float yercekimi = 9.81f;

    [Header("Duvar Referansları")]
    public Transform solDuvar;
    public Transform sagDuvar;

    private float solSinir;
    private float sagSinir;

    void Start()
    {
        if (karakter == null)
            karakter = GameObject.FindGameObjectWithTag("Player").transform;

        // Duvarların sınırlarını hesapla (BoxCollider varsa)
        if (solDuvar != null && sagDuvar != null)
        {
            BoxCollider solCol = solDuvar.GetComponent<BoxCollider>();
            BoxCollider sagCol = sagDuvar.GetComponent<BoxCollider>();

            if (solCol != null && sagCol != null)
            {
                solSinir = solCol.bounds.max.x; // Sol duvarın iç yüzeyi
                sagSinir = sagCol.bounds.min.x; // Sağ duvarın iç yüzeyi
            }
            else
            {
                UnityEngine.Debug.LogWarning("Duvarlarda BoxCollider yok, varsayılan genişlik kullanılacak!");
                solSinir = -zeminGenislik;
                sagSinir = zeminGenislik;
            }
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

                // Duvarların içinde kalmasını sağla
                yeniX = Mathf.Clamp(yeniX, solSinir + 1f, sagSinir - 1f);

                deneme++;
                if (deneme > 20) break;
            } while (Mathf.Abs(yeniX - sonX) < minimumXMesafe);

            spawnKonumu.x = yeniX;
            sonX = yeniX;

            GameObject yeniZemin = Instantiate(zemin.gameObject, spawnKonumu, Quaternion.identity);

            Collider collider = yeniZemin.GetComponent<Collider>();
            if (collider == null)
                collider = yeniZemin.AddComponent<BoxCollider>();

            yeniZemin.AddComponent<OneWayPlatform>();
        }
    }
}
