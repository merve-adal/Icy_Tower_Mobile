using System.Collections.Generic;
using UnityEngine;

public class SonsuzDuvar : MonoBehaviour
{
    [Header("Duvar Ayarlarý")]
    public GameObject duvarPrefab;
    public Transform[] baslangicDuvarlar; // sol ve sað duvar baþlangýç noktalarý
    public float temizlemeMesafesi = 20f;

    [Header("Referanslar")]
    public Transform karakter;

    private List<GameObject> havuz = new List<GameObject>();
    private Dictionary<float, float> sonYukseklik = new Dictionary<float, float>();
    private float duvarHeight;
    private int havuzBoyutu = 150;

    void Start()
    {
        if (karakter == null)
            karakter = GameObject.FindGameObjectWithTag("Player").transform;

        // Havuz oluþtur
        for (int i = 0; i < havuzBoyutu; i++)
        {
            GameObject obj = Instantiate(duvarPrefab);
            obj.SetActive(false);
            havuz.Add(obj);

            if (i == 0)
                duvarHeight = obj.GetComponentInChildren<Renderer>().bounds.size.y;
        }

        // Baþlangýç segmentleri
        foreach (var taban in baslangicDuvarlar)
        {
            sonYukseklik[taban.position.x] = taban.position.y;

            while (sonYukseklik[taban.position.x] < karakter.position.y + 10f)
                UsteYeniParcaGetir(taban.position.x);
        }
    }

    void Update()
    {
        foreach (var taban in baslangicDuvarlar)
        {
            while (sonYukseklik[taban.position.x] < karakter.position.y + 10f)
                UsteYeniParcaGetir(taban.position.x);
        }

        ZeminleriTemizle();
    }

    void UsteYeniParcaGetir(float xPozisyon)
    {
        GameObject parca = HavuzdanBosVeyaEnAltiGeriDonustur(xPozisyon);
        if (parca == null) return;

        float yeniY = sonYukseklik[xPozisyon] + duvarHeight;
        parca.transform.position = new Vector3(xPozisyon, yeniY, 0f);
        parca.SetActive(true);

        sonYukseklik[xPozisyon] = yeniY;
    }

    GameObject HavuzdanBosVeyaEnAltiGeriDonustur(float xPozisyon)
    {
        foreach (var go in havuz)
            if (!go.activeInHierarchy) return go;

        GameObject enAlttaki = null;
        float minY = float.MaxValue;

        foreach (var go in havuz)
        {
            if (!go.activeInHierarchy) continue;
            if (!Mathf.Approximately(go.transform.position.x, xPozisyon)) continue;

            float y = go.transform.position.y;
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
        float altSinir = karakter.position.y - temizlemeMesafesi;

        foreach (var duvar in havuz)
        {
            if (duvar.activeInHierarchy && duvar.transform.position.y < altSinir)
                duvar.SetActive(false);
        }
    }
}
