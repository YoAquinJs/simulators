using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System;

public class Pendulum : MonoBehaviour
{
    /// <summary>
    /// Mass of object
    /// </summary>
    [SerializeField, BoxGroup("Pendulum Variables")] private float mass;
    [SerializeField, BoxGroup("Pendulum Variables")] private float minMass;
    [SerializeField, BoxGroup("Pendulum Variables")] private float maxMass;
    /// <summary>
    /// Distance from joint to object
    /// </summary>
    [SerializeField, BoxGroup("Pendulum Variables")] private float dist;
    [SerializeField, BoxGroup("Pendulum Variables")] private float minDist;
    [SerializeField, BoxGroup("Pendulum Variables")] private float maxDist;
    /// <summary>
    /// Initial Angle of object with Y axis
    /// </summary>
    [SerializeField, BoxGroup("Pendulum Variables")] private float angle;
    /// <summary>
    /// Friction 
    /// </summary>
    [SerializeField, BoxGroup("Pendulum Variables")] private float fricCoeficient;
    [SerializeField, BoxGroup("Pendulum Variables")] private float minFric;
    [SerializeField, BoxGroup("Pendulum Variables")] private float maxFric;
    /// <summary>
    /// Gravity
    /// </summary>
    [SerializeField, BoxGroup("Pendulum Variables")] private float g;
    [Space]
    [SerializeField, BoxGroup("Pendulum Obj")] private GameObject circle;
    [SerializeField, BoxGroup("Pendulum Obj")] private float maxScale;
    [SerializeField, BoxGroup("Pendulum Obj")] private float minScale;
    [SerializeField, BoxGroup("Pendulum Obj")] private GameObject rope;
    [SerializeField, BoxGroup("Pendulum Obj")] private float ropeWidth;
    [SerializeField, BoxGroup("Pendulum Obj")] private GameObject center;
    [Space]
    [SerializeField, BoxGroup("Text")] private TMP_Text angleText;
    [SerializeField, BoxGroup("Text")] private TMP_Text distanceText;
    [SerializeField, BoxGroup("Text")] private TMP_Text gravityText;
    [SerializeField, BoxGroup("Text")] private TMP_Text frictionText;
    [SerializeField, BoxGroup("Text")] private TMP_Text massText;
    [SerializeField, BoxGroup("Text")] private TMP_Text periodText;
    [SerializeField, BoxGroup("Text")] private TMP_Text energyText;
    [SerializeField, BoxGroup("Text")] private TMP_Text potentialText;
    [SerializeField, BoxGroup("Text")] private TMP_Text kineticText;
    [SerializeField, BoxGroup("Text")] private TMP_Text cronometerText;
    [Space]
    [SerializeField, BoxGroup("Slider")] private Slider angleSlider;
    [SerializeField, BoxGroup("Slider")] private Slider distanceSlider;
    [SerializeField, BoxGroup("Slider")] private Slider gravitySlider;
    [SerializeField, BoxGroup("Slider")] private Slider massSlider;
    [SerializeField, BoxGroup("Slider")] private Slider frictionSlider;
    [SerializeField, BoxGroup("Slider")] private Slider energySlider;
    [SerializeField, BoxGroup("Slider")] private Toggle frictionToogle;
    [Space]
    [SerializeField, BoxGroup("Vector Object")] private GameObject vectorF;
    [SerializeField, BoxGroup("Vector Object")] private GameObject headF;
    [SerializeField, BoxGroup("Vector Object")] private GameObject vectorV;
    [SerializeField, BoxGroup("Vector Object")] private GameObject headV;
    [Space]
    [SerializeField] private Image pauseButtonImage;
    [SerializeField] private Color pausedColor;

    /// <summary>
    /// Time when simulation start
    /// </summary>
    float playTime;
    /// <summary>
    /// initial angle in radians
    /// </summary>
    float aRads;
    /// <summary>
    /// Sine of Initialθ/2
    /// </summary>
    float k;
    /// <summary>
    /// Square Root of gravity/distance
    /// </summary>
    float u;
    /// <summary>
    /// Period time elapsed to return to maximum angle
    /// </summary>
    float T;
    /// <summary>
    /// Aproximated Period value
    /// </summary>
    float _T;
    /// <summary>
    /// Square root constant in friction equation
    /// </summary>
    float sqrtFric;
    /// <summary>
    /// Time from θ = 0 to θ = Initialθ
    /// </summary>
    float initT;
    /// <summary>
    /// Previous θ(t) value
    /// </summary>
    float _theta;
    /// <summary>
    /// Total energy of the pendulum
    /// </summary>
    float energy;
    /// <summary>
    /// Compute with or without friction
    /// </summary>
    [ShowNonSerializedField] bool friction;

    float pauseTime;
    bool simulating = false;
    bool paused = false;
    bool firstMaxAngle = false;

    [Button()]
    public void GetPeriod()
    {
        aRads = angle * Mathf.PI / 180;//Sets angle in radians
        k = Mathf.Sin(Mathf.Abs(aRads) / 2);//Abs for avoiding Jacobi SN Exception
        u = Mathf.Sqrt(g / dist);
        _T = 2 * Mathf.PI * Mathf.Sqrt(dist / g);//Only first term of sucesion (1)
        var succession = 1f;
        for (var i = 1; i <= 9; i++)//10 first Period terms for more acurracy
        {
            var iFact = 1f;//Factorial of i
            for (var j = 1; j <= i; j++) { iFact *= j; }
            var i2Fact = 1f;//Factorial of 2i
            for (var j = 1; j <= i * 2; j++) { i2Fact *= j; }

            succession += Mathf.Pow(i2Fact / (Mathf.Pow(2, i * 2) * Mathf.Pow(iFact, 2)), 2) *
                Mathf.Pow(k, i * 2);//Sucesion term (see period formula)
        }

        T = _T * succession;
        Debug.Log($"Period: {T}");
    }

    public void TriggerSimulation()
    {
        if (simulating) return;
        aRads = angle * Mathf.PI / 180;//Sets angle in radians
        k = Mathf.Sin(Mathf.Abs(aRads)/2);//Abs for avoiding Jacobi SN Exception
        u = Mathf.Sqrt(g / dist);
        _T = 2 * Mathf.PI * Mathf.Sqrt(dist / g);//Only first term of sucesion (1)
        var succession = 1f;
        for (var i = 1; i <= 9; i++)//10 first Period terms for more acurracy
        {
            var iFact = 1f;//Factorial of i
            for (var j = 1; j <= i; j++) { iFact *= j; }
            var i2Fact = 1f;//Factorial of 2i
            for (var j = 1; j <= i * 2; j++) { i2Fact *= j; }
            
            succession += Mathf.Pow(i2Fact / (Mathf.Pow(2, i*2) * Mathf.Pow(iFact, 2)), 2) * 
                Mathf.Pow(k, i*2);//Sucesion term (see period formula)
        }

        T = _T * succession;
        initT = T / 4;

        _theta = aRads + 1;

        sqrtFric = Mathf.Sqrt((g / dist) - (Mathf.Pow(fricCoeficient, 2) / (4 * Mathf.Pow(mass, 2))));
        energy = mass * g * dist * (1 - Mathf.Cos(aRads));
        energyText.text = $"Energy:\n{Math.Round(energy, 3)}";
        energySlider.gameObject.SetActive(true);
        energySlider.maxValue = energy;

        angleSlider.interactable = false;
        distanceSlider.interactable = false;
        gravitySlider.interactable = false;
        massSlider.interactable = false;
        frictionSlider.interactable = false;
        frictionToogle.interactable = false;

        periodText.text = $"Periodo:\n{Math.Round(T, 3)}";
        
        playTime = Time.time;
        firstMaxAngle = true;

        if (paused)
            pauseTime = Time.time;

        simulating = true;
    }
    
    public void StopSimulation()
    {
        if(!simulating) return;
        
        simulating = false;
        center.transform.localEulerAngles = new Vector3(0, 0, angle);

        vectorF.transform.localScale = new Vector3(0, 0.1f, 1);
        vectorV.transform.localScale = new Vector3(0, 0.1f, 0);
        headV.SetActive(false);
        headF.SetActive(false);

        angleSlider.interactable = true;
        distanceSlider.interactable = true;
        gravitySlider.interactable = true;
        massSlider.interactable = true;
        frictionSlider.interactable = true;
        frictionToogle.interactable = true;

        energyText.text = "";
        potentialText.text = "";
        kineticText.text = "";
        energySlider.gameObject.SetActive(false);
    }

    public void PauseSimulation()
    {
        paused = !paused;
        if (paused)
        {
            pauseTime = Time.time;
            pauseButtonImage.color = pausedColor;
        }
        else
        {
            playTime += Time.time - pauseTime;
            pauseButtonImage.color = new Color(0.75f, 0.75f, 0.75f);
        }
    }

    public void OneFrame()
    {
        if (!paused || !simulating) return;

        playTime += Time.time - pauseTime;
        pauseTime = Time.time;

        Iteration();
        playTime -= Time.fixedDeltaTime;
    }

    /// <summary>
    /// θ(t) Function of Angle over time
    /// </summary>
    /// <param name="t">Time</param>
    /// <returns></returns>
    float Theta(float t)
    {
        alglib.jacobianellipticfunctions(u * t, k, out var sn, out _, out var _, out var _);//Jacobi Elliptic SN
        return 2 * Mathf.Asin(k * (float)sn * (aRads < 0 ? -1 : 1));//Angle calculation (see θ(t) formula)
    }

    //Aproximated, not found acurrete formula
    float ThetaFric(float t)
    {
        return aRads * Mathf.Pow((float) Math.E, (-1 * fricCoeficient * t) / (2 * mass)) * Mathf.Cos(sqrtFric * t);//Angle calculation (see θ(t) with friction formula)
    }

    void Iteration()
    {
        var theta = 0f;

        if (!friction)
        {
            if (firstMaxAngle)//Adjust initial time
            {
                headV.SetActive(true);
                headF.SetActive(true);

                firstMaxAngle = false;
                for (var i = initT; i < 120; i += 0.001f)
                {
                    if (angle - (Theta(i) * 180 / Mathf.PI) > 0.05f) continue;
                    initT = i;
                    break;
                }
            }

            theta = Theta(Time.time - playTime + initT);
        }
        else
        {
            theta = ThetaFric(Time.time - playTime);
        }

        center.transform.localEulerAngles = new Vector3(0, 0, (float)theta * 180 / Mathf.PI);//Rotate object

        //Velocity Vectors
        if (!friction)
        {
            var dir = theta < _theta;//Angle increasing, right, decreasing, left
            var angularVelocity = Mathf.Sqrt(2 * g / dist) * Mathf.Sqrt(Mathf.Cos(theta) - Mathf.Cos(aRads));//Angular velocity (see d/dx θ(t) formula)

            vectorV.transform.localPosition = new Vector3(dir ? -angularVelocity / 2f : angularVelocity / 2f, -0.2f, 0);
            vectorV.transform.localScale = new Vector3(angularVelocity, 0.1f, 0);

            headV.transform.localPosition = new Vector3(dir ? -angularVelocity : angularVelocity, -0.2f, 0);
            headV.transform.localScale = new Vector3(dir ? -0.2f : 0.2f, dir ? -0.2f : 0.2f, 0);
            _theta = theta;
        }

        //Net Force
        var force = (mass) * g * -Mathf.Sin(theta);

        vectorF.transform.localPosition = new Vector3(force / 2f, 0.2f, 0);
        vectorF.transform.localScale = new Vector3(Mathf.Abs(force), 0.1f, 1);

        headF.transform.localPosition = new Vector3(force, 0.2f, 0);
        headF.transform.localScale = new Vector3(force > 0 ? 0.2f : -0.2f, force > 0 ? 0.2f : -0.2f, 0);

        //TODO: aproximate velocity for calculating each energy
        //Energies
        if (!friction)
        {
            var Ep = mass * g * dist * (1 - Mathf.Cos(theta));
            var Ec = energy - Ep;
            energySlider.value = Ep;

            potentialText.text = $"Potencial:\n{Math.Round(Ep, 3)}";

            kineticText.text = $"Cinetica:\n{Math.Round(Ec, 3)}";
        }
        else
        {
            var Em = (mass * g * dist * (1 - Mathf.Cos(theta))) /*+ (mass/2 * Mathf.Pow(angularVelocity * dist, 2))*/;
            var Ef = energy - Em;
            energySlider.value = Em;

            potentialText.text = $"Mecanica:\npendiente";

            kineticText.text = $"Friccion:\npendiente";
        }

        //Timer
        var t = Math.Round(Time.time - playTime, 3);
        var extraChar = t.ToString().Length - t.ToString().IndexOf('.') < 4 ? "0" : "";
        cronometerText.text = $"Time:\n{t}{extraChar}";
    }

    void Start() => SetValues();

    void SetValues()
    {
        SetAngle(angle * 100);
        angleSlider.value = angle * 100;
        
        SetDistance(dist * 1000);
        distanceSlider.minValue = minDist * 1000;
        distanceSlider.maxValue = maxDist * 1000;
        distanceSlider.value = dist * 1000;
        
        SetMass(mass * 1000);
        massSlider.minValue = minMass * 1000;
        massSlider.maxValue = maxMass * 1000;
        massSlider.value = mass * 1000;

        SetGravity(g * 100);
        gravitySlider.value = g * 100;

        SetFrictionCoeficient(fricCoeficient * 100);
        frictionSlider.minValue = minFric * 100;
        frictionSlider.maxValue = maxFric * 100;
        frictionSlider.value = fricCoeficient * 100;

        frictionToogle.isOn = friction;
    }

    public void SetMass(float value)
    {
        mass = value/1000;
        massText.text = $"Masa:\n{mass}Kg";
        var scale = (mass - minMass) * ((maxScale - minScale) / (maxMass - minMass)) + minScale;

        circle.transform.localScale = new Vector2(scale, scale);
    }
    
    public void SetDistance(float value)
    {
        dist = value/1000;
        distanceText.text = $"Distancia:\n{dist}mtr";

        rope.transform.localPosition = new Vector3(0, -dist/2f, 0);
        rope.transform.localScale = new Vector3(ropeWidth, dist, 0);

        circle.transform.localPosition = new Vector3(0, -dist, 0);
    }
    
    public void SetAngle(float value)
    {
        angle = value/100;
        angleText.text = $"Angulo:\n{angle}°";

        center.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void SetGravity(float value)
    { 
        g = value/100;
        gravityText.text = $"Gravedad:\n{g}m/s<sup>2</sup>";
    }

    public void SetFrictionCoeficient(float value)
    {
        fricCoeficient = value / 100;
        frictionText.text = $"Friccion:\n{fricCoeficient}";
    }

    public void SetFriction(bool value) => friction = value;


    void FixedUpdate()
    {
        if (!simulating || paused) return;

        Iteration();
    }
}