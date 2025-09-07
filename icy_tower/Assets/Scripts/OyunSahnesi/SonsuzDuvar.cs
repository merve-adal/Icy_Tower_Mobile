using System.Collections.Generic;
using UnityEngine;

public class SonsuzDuvar : MonoBehaviour
{
    [Header("Duvar Ayarlarý")]
    public GameObject duvarPrefab;
    public int baslangicSegmentSayisi = 30;
    public float uretilmeAraligi = 0.5f;
    public int havuzBoyutu = 150;

    [Header("Referanslar")]
    public Transform[] baslangicDuvarlar;
    public Transform karakter;                 // Player inspector’dan atanmalý
    public float temizlemeMesafesi = 20f;      // Kaç birim aþaðýsýnda kalýnca devre dýþý olacak

    private readonly List<GameObject> havuz = new List<GameObject>();
    private readonly Dictionary<float, float> sonYukseklik = new Dictionary<float, float>();
    private float duvarHeight;
    private float zamanSayaci;

    void Start()
    {
        if (duvarPrefab == null || baslangicDuvarlar == null || baslangicDuvarlar.Length == 0)
        {
            UnityEngine.Debug.LogError("SonsuzDuvar: duvarPrefab veya baslangicDuvarlar atanmamýþ!");
            enabled = false;
            return;
        }

        if (karakter == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) karakter = player.transform;
        }

        for (int i = 0; i < havuzBoyutu; i++)
        {
            GameObject obj = Instantiate(duvarPrefab);
            if (i == 0)
            {
                var r = obj.GetComponentInChildren<Renderer>();
                if (r == null)
                {
                    UnityEngine.Debug.LogError("SonsuzDuvar: Prefab'ta Renderer bulunamadý!");
                    Destroy(obj);
                    enabled = false;
                    return;
                }
                duvarHeight = r.bounds.size.y;
            }
            obj.SetActive(false);
            havuz.Add(obj);
        }

        foreach (var taban in baslangicDuvarlar)
        {
            sonYukseklik[taban.position.x] = taban.position.y;
            for (int i = 0; i < baslangicSegmentSayisi; i++)
                UsteYeniParcaGetir(taban.position.x);
        }
    }

    void Update()
    {
        zamanSayaci += Time.deltaTime;
        if (zamanSayaci >= uretilmeAraligi)
        {
            foreach (var taban in baslangicDuvarlar)
                UsteYeniParcaGetir(taban.position.x);
            zamanSayaci = 0f;
        }

        // Temizlik
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
        for (int i = 0; i < havuz.Count; i++)
            if (!havuz[i].activeInHierarchy)
                return havuz[i];

        GameObject enAlttaki = null;
        float minY = float.MaxValue;

        for (int i = 0; i < havuz.Count; i++)
        {
            var go = havuz[i];
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
        if (karakter == null) return;

        float altSinir = karakter.position.y - temizlemeMesafesi;

        foreach (var duvar in havuz)
        {
            if (duvar.activeInHierarchy && duvar.transform.position.y < altSinir)
            {
                duvar.SetActive(false);
            }
        }
    }
}
