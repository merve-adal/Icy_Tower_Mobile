using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZeminGenerator : MonoBehaviour
{
    [Header("Platform Ayarları")]
    public GameObject zeminPrefab;
    public int baslangicSegmentSayisi = 30;
    public int havuzBoyutu = 150;
    public float minimumY = 2f;
    public float maksimumY = 4f;

    [Header("Karakter Zıplama Ayarı")]
    public Transform karakter;

    [Header("Sonsuz Üretim Ayarları")]
    public float uretilmeAraligi = 2f;
    public float temizlemeMesafesi = 20f;

    [Header("Duvar Referansları")]
    public Transform solDuvar;
    public Transform sagDuvar;

    private List<GameObject> havuz = new List<GameObject>();
    private float sonX;
    private float sonY;
    private float zamanSayaci;

    void Start()
    {
        if (karakter == null)
            karakter = GameObject.FindGameObjectWithTag("Player").transform;

        // Havuzu oluştur
        for (int i = 0; i < havuzBoyutu; i++)
        {
            GameObject obj = Instantiate(zeminPrefab);
            obj.SetActive(false);

            // OneWayPlatform ekle
            if (obj.GetComponent<OneWayPlatform>() == null)
                obj.AddComponent<OneWayPlatform>();

            // Collider ekle
            if (obj.GetComponent<Collider>() == null)
                obj.AddComponent<BoxCollider>();

            havuz.Add(obj);
        }

        Vector3 baslangicPozisyon = new Vector3(1.77f, -2.3f, 0f);
        sonX = baslangicPozisyon.x;
        sonY = baslangicPozisyon.y;

        // Başlangıç segmentleri oluştur
        for (int i = 0; i < baslangicSegmentSayisi; i++)
            YeniZeminOlustur();
    }

    void Update()
    {
        zamanSayaci += Time.deltaTime;
        if (zamanSayaci >= uretilmeAraligi)
        {
            YeniZeminOlustur();
            zamanSayaci = 0f;
        }

        ZeminleriTemizle();
    }

    void YeniZeminOlustur()
    {
        GameObject zemin = HavuzdanAlVeyaEnAltiGetir();
        if (zemin == null) return;

        float ortalamaYArtis = (minimumY + maksimumY) / 2f;
        float rastgeleKayma = Random.Range(-0.5f, 0.5f);
        sonY += ortalamaYArtis + rastgeleKayma;

        float solSinir = solDuvar.position.x + (solDuvar.localScale.x / 2f) + (zeminPrefab.transform.localScale.x / 2f);
        float sagSinir = sagDuvar.position.x - (sagDuvar.localScale.x / 2f) - (zeminPrefab.transform.localScale.x / 2f);

        sonX = Random.Range(solSinir, sagSinir);

        zemin.transform.position = new Vector3(sonX, sonY, 0f);
        zemin.SetActive(true);
    }

    GameObject HavuzdanAlVeyaEnAltiGetir()
    {
        foreach (var go in havuz)
            if (!go.activeInHierarchy) return go;

        GameObject enAlttaki = null;
        float minY = float.MaxValue;

        foreach (var go in havuz)
        {
            if (!go.activeInHierarchy) continue;
            float y = go.transform.position.y;
            if (karakter != null && y > karakter.position.y - 2f) continue;
            if (y < minY)
            {
                minY = y;
                enAlttaki = go;
            }
        }

        return enAlttaki;
    }

    void ZeminleriTemizle()
    {
        if (karakter == null) return;

        float altSinir = karakter.position.y - temizlemeMesafesi;

        foreach (var zemin in havuz)
        {
            if (zemin.activeInHierarchy && zemin.transform.position.y < altSinir)
                zemin.SetActive(false);
        }
    }
}
