using TMPro;
using UnityEngine;
using NaughtyAttributes;
using Unity.Mathematics;

public class Gases : MonoBehaviour
{
    [SerializeField] GasList g;
    [SerializeField] GameObject a;
    [SerializeField] GameObject b;

    [SerializeField] TMP_Text gasAText;
    [SerializeField] TMP_Text gasBText;
    [SerializeField] TMP_Text massAText;
    [SerializeField] TMP_Text massBText;
    [SerializeField] TMP_Text ratioText;

    [SerializeField] int particleCountA;
    [SerializeField] int particleCountB;
    [SerializeField] Vector2 xSpawn;
    [SerializeField] Vector2 ySpawn;

    public float vA;
    public float vB;
    public int gasA;
    public int gasB;

    Rigidbody2D[] particles;

    [ShowNonSerializedField] bool bigger;
    [ShowNonSerializedField] float ratio;

    [Button()]
    public void Simulate()
    {
        Stop();
        Stop();
        Stop();
        Stop();
        Stop();
        Stop();
        Stop();
        Stop();
        Stop();
        Stop();

        particles = new Rigidbody2D[particleCountA + particleCountB];
        var sacaleA =(float) System.Math.Pow(g.gases[gasA].weigth, 1f/2.5f) / 20;
        var volumeA = new Vector3(0.1f + sacaleA, 0.1f + sacaleA, 0);
        var sacaleB =(float) System.Math.Pow(g.gases[gasB].weigth, 1f/2.5f) / 20 ;
        var volumeB = new Vector3(0.1f + sacaleB, 0.1f + sacaleB, 0);

        bigger = g.gases[gasB].weigth > g.gases[gasA].weigth;
        ratio = math.sqrt(bigger ? g.gases[gasB].weigth / g.gases[gasA].weigth : g.gases[gasA].weigth / g.gases[gasB].weigth);
        ratioText.text = "Ley De Graham\n" + ratio.ToString();

        for (var i = 0; i < particleCountA; i++)
        {
            var _a = Instantiate(a, new Vector3(UnityEngine.Random.Range(xSpawn.x, xSpawn.y), 
                UnityEngine.Random.Range(ySpawn.x, ySpawn.y), 0), quaternion.identity, transform);

            _a.transform.localScale = volumeA;
            particles[i] = _a.GetComponent<Rigidbody2D>();

            particles[i].velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f) * vA * (bigger ? ratio : 1), 
                UnityEngine.Random.Range(-1f, 1f) * vA * (bigger ? ratio : 1));
        }

        for (var i = 0; i < particleCountB; i++)
        {
            var _b = Instantiate(b, new Vector3(UnityEngine.Random.Range(-xSpawn.y, -xSpawn.x),
                UnityEngine.Random.Range(ySpawn.x, ySpawn.y), 0), quaternion.identity, transform);
            
            _b.transform.localScale = volumeB;
            particles[particleCountA - 1 + i] = _b.GetComponent<Rigidbody2D>();

            particles[particleCountA - 1 + i].velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f) * vB * (bigger ? 1 : ratio),
                UnityEngine.Random.Range(-1f, 1f) * vB * (bigger ? 1 : ratio));
        }
    }

    public void SetGasA(float a)
    {
        gasA = (int)a;
        massAText.text = "Peso: " + g.gases[gasA].weigth.ToString();
        gasAText.text = g.gases[gasA].name + " " + g.gases[gasA].formula;
    }
    public void SetGasB(float b)
    {
        gasB = (int)b;
        massBText.text = "Peso: " + g.gases[gasB].weigth.ToString();
        gasBText.text = g.gases[gasB].name + " " + g.gases[gasB].formula;
    }
    public void CountA(float v) => particleCountA =(int) v;
    public void CountB(float v) => particleCountB = (int) v;
    public void InitialSpeedA(float v) => vA = v;
    public void InitialSpeedB(float v) => vB = v;

    [Button()]
    public void Stop()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}