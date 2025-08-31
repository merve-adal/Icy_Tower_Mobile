using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZeminHavuz : MonoBehaviour
{
    [Header("Platform Ayarları")]
    public Transform zeminPrefab;
    public int havuzBoyutu = 50;           // Maksimum sahnede aktif platform
    public float zeminGenislik = 3f;
    public float minimumY = 2f;
    public float maksimumY = 4f;
    public float minimumXMesafe = 1f;

    [Header("Karakter")]
    public Transform karakter;
    public float karakterZiplamaGucu = 5f;

    private List<GameObject> havuz = new List<GameObject>();
    private int aktifIndex = 0;
    private float sonX = 0f;
    private float sonY = 0f;

    void Start()
    {
        if (karakter == null)
            karakter = GameObject.FindGameObjectWithTag("Player").transform;

        // Havuz oluştur
        for (int i = 0; i < havuzBoyutu; i++)
        {
            GameObject obj = Instantiate(zeminPrefab.gameObject);
            if (obj.GetComponent<Collider>() == null)
                obj.AddComponent<BoxCollider>();

            if (obj.GetComponent<OneWayPlatform>() == null)
                obj.AddComponent<OneWayPlatform>();

            obj.SetActive(false);
            havuz.Add(obj);
        }

        // Başlangıç platformu
        Vector3 baslangicPoz = new Vector3(1.77f, -2.3f, 0f);
        sonX = baslangicPoz.x;
        sonY = baslangicPoz.y;

        for (int i = 0; i < 10; i++) // İlk 10 platform aktif
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        // Karakter yaklaştıkça yeni platform üret
        if (karakter.position.y + 5f > sonY) // karakter +5 birim yaklaşınca
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        GameObject platform = havuz[aktifIndex];
        platform.SetActive(true);

        // Y ekseni
        float maxY = Mathf.Min(maksimumY, karakterZiplamaGucu * 0.9f);
        float minY = Mathf.Max(minimumY, karakterZiplamaGucu * 0.5f);
        sonY += Random.Range(minY, maxY);

        // X ekseni
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

        sonX = yeniX;
        platform.transform.position = new Vector3(sonX, sonY, 0f);

        aktifIndex = (aktifIndex + 1) % havuzBoyutu; // döngüsel havuz
    }
}
