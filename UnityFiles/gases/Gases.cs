using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] Slider aTypeSlider;
    [SerializeField] Slider bTypeSlider;

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

    private void Start()
    {
        aTypeSlider.minValue = 0;
        bTypeSlider.minValue = 0;
        aTypeSlider.maxValue = g.gases.Length-1;
        bTypeSlider.maxValue = g.gases.Length - 1;

        aTypeSlider.value = 0;
        bTypeSlider.value = g.gases.Length-1;
    }

    public void Simulate()
    {
        Stop();

        particles = new Rigidbody2D[particleCountA + particleCountB];
        var sacaleA =(float) System.Math.Pow(g.gases[gasA].weigth, 1f/2.5f) / 20;
        var volumeA = new Vector3(0.1f + sacaleA, 0.1f + sacaleA, 0);
        var sacaleB =(float) System.Math.Pow(g.gases[gasB].weigth, 1f/2.5f) / 20 ;
        var volumeB = new Vector3(0.1f + sacaleB, 0.1f + sacaleB, 0);

        bigger = g.gases[gasB].weigth > g.gases[gasA].weigth;
        ratio =(float) Math.Round(math.sqrt(bigger ? g.gases[gasB].weigth / g.gases[gasA].weigth : g.gases[gasA].weigth / g.gases[gasB].weigth), 3);
        ratioText.text = $"Graham\nLaw\nRatio\n{ratio}";

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
        massAText.text = $"Weight:\n{g.gases[gasA].weigth}";
        gasAText.text = $"{g.gases[gasA].name} {g.gases[gasA].formula}";
    }
    public void SetGasB(float b)
    {
        gasB = (int)b;
        massBText.text = $"Weight:\n{g.gases[gasB].weigth}";
        gasBText.text = $"{g.gases[gasB].name} {g.gases[gasB].formula}";
    }
    public void CountA(float v) => particleCountA =(int) v;
    public void CountB(float v) => particleCountB = (int) v;
    public void InitialSpeedA(float v) => vA = v;
    public void InitialSpeedB(float v) => vB = v;
   
    public void Stop()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}