using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Unity.Mathematics;

public class Parabolic : MonoBehaviour
{
    [SerializeField, BoxGroup("Parabolic Variables")] private float gravity;
    [SerializeField, BoxGroup("Parabolic Variables")] private float height;
    [SerializeField, BoxGroup("Parabolic Variables")] private float velocity;
    [SerializeField, BoxGroup("Parabolic Variables")] private float angle;
    [Space]
    [SerializeField, BoxGroup("Parabolic Obj")] private float pathLine;
    [SerializeField, BoxGroup("Parabolic Obj")] private GameObject mass;
    [SerializeField, BoxGroup("Parabolic Obj")] private Transform pathHolder;
    [SerializeField, BoxGroup("Parabolic Obj")] private GameObject platform;
    [SerializeField, BoxGroup("Parabolic Obj")] private GameObject objPrefab;
    [Space]
    [SerializeField, BoxGroup("Vector Object")] private GameObject vectorV;
    [SerializeField, BoxGroup("Vector Object")] private GameObject headV;
    [Space]
    [SerializeField, BoxGroup("Text")] private TMP_Text gravityText;
    [SerializeField, BoxGroup("Text")] private TMP_Text heightText;
    [SerializeField, BoxGroup("Text")] private TMP_Text velocityText;
    [SerializeField, BoxGroup("Text")] private TMP_Text angleText;
    [SerializeField, BoxGroup("Text")] private TMP_Text maxHeightText;
    [SerializeField, BoxGroup("Text")] private TMP_Text flightTimeText;
    [SerializeField, BoxGroup("Text")] private TMP_Text horizontalReachText;
    [SerializeField, BoxGroup("Text")] private TMP_Text energyText;
    [SerializeField, BoxGroup("Text")] private TMP_Text potentialText;
    [SerializeField, BoxGroup("Text")] private TMP_Text kineticText;
    [Space]
    [SerializeField, BoxGroup("Slider")] private Slider gravitySlider;
    [SerializeField, BoxGroup("Slider")] private Slider heightSlider;
    [SerializeField, BoxGroup("Slider")] private Slider velocitySlider;
    [SerializeField, BoxGroup("Slider")] private Slider timeSlider;
    [SerializeField, BoxGroup("Slider")] private Slider angleSlider;
    [SerializeField, BoxGroup("Slider")] private Slider energySlider;

    [SerializeField] private Image pauseButtonImage;
    [SerializeField] private Color pausedColor;
    private Color unPausedColor;

    private float energy;
    private float flightTime;

    private float initialX;
    private float vox, voy;
    private float pastX, pastY;

    private float timePassed;
    private float nextPath, distanceTravelled;

    private bool simulating = false;
    private bool paused = false;
    [ShowNonSerializedField] private bool firstRun = false;

    public void TriggerSimulation()
    {
        if (simulating) return;

        nextPath = 0;
        distanceTravelled = 0;

        initialX = mass.transform.position.x;
        
        pastX = initialX;
        pastY = height;

        vox = velocity * Mathf.Cos(angle * Mathf.PI / 180);
        voy = velocity * Mathf.Sin(angle * Mathf.PI / 180);

        flightTime =(float) (-voy + Math.Sqrt(Math.Pow(voy, 2) - (4 * (-gravity/2) * height))) / -gravity;

        if (flightTime <= 0)
            flightTime = (float)(-voy - Math.Sqrt(Math.Pow(voy, 2) - (4 * (-gravity / 2) * height))) / -gravity;

        flightTime = (float) Math.Round(flightTime, 2);
        timeSlider.maxValue = flightTime * 100;
        timeSlider.value = 0;
        flightTimeText.text = $"Flight Time:\n{flightTime}";

        maxHeightText.text = $"Max Height:\n{Math.Round(Math.Pow(voy, 2)/(2*gravity) + height, 3)}";
        horizontalReachText.text = $"Horizontal Reach:\n{Math.Round(vox * flightTime, 3)}";

        var potentialE = gravity * height;
        var kineticE =(float) (0.5 * Math.Pow(velocity, 2));
        energy = potentialE + kineticE;

        potentialText.text = $"Potential:\n{Math.Round(potentialE, 2)}";
        kineticText.text = $"Kinetic:\n{Math.Round(kineticE, 2)}";

        energyText.text = $"Energy:\n{Math.Round(energy, 2)}";
        energySlider.gameObject.SetActive(true);
        energySlider.maxValue = energy;
        energySlider.value = potentialE;

        gravitySlider.interactable = false;
        heightSlider.interactable = false;
        velocitySlider.interactable = false;
        angleSlider.interactable = false;
        timeSlider.interactable = false;

        timePassed = 0;

        firstRun = true;
        simulating = true;
    }

    public void StopSimulation()
    {
        if (!simulating) return;

        simulating = false;

        if (paused)
            PauseSimulation();

        mass.transform.localPosition = new Vector3(initialX, height + 0.3f, 0);
        mass.transform.localEulerAngles = new Vector3(0, 0, angle);

        vectorV.transform.localPosition = new Vector3((velocity * 0.2f) / 2f, 0, 0);
        vectorV.transform.localScale = new Vector3((velocity * 0.2f), 0.1f, 0);
        headV.transform.localPosition = new Vector3((velocity * 0.2f), 0, 0);

        for (var i = 0; i < pathHolder.childCount; i++)
        {
            Destroy(pathHolder.GetChild(i).gameObject);
        }

        gravitySlider.interactable = true;
        heightSlider.interactable = true;
        velocitySlider.interactable = true;
        angleSlider.interactable = true;
        timeSlider.interactable = false;

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
            pauseButtonImage.color = pausedColor;

            if (simulating && !firstRun)
                timeSlider.interactable = true;
        }
        else
        {
            timeSlider.interactable = false;
            pauseButtonImage.color = unPausedColor;
        }
    }

    public void OneFrame()
    {
        if (!paused || !simulating) return;

        timePassed += Time.fixedDeltaTime;

        if (timePassed >= flightTime)
        {
            timePassed = flightTime;

            if (firstRun)
            {
                firstRun = false;
                timeSlider.interactable = true;

                if (!paused)
                    PauseSimulation();
            }
        }

        Iteration(timePassed);
    }

    private void Start()
    {
        SetGravity(gravity * 100);
        gravitySlider.value = gravity * 100;
        SetHeight(height * 100);
        heightSlider.value = height * 100;
        SetVelocity(velocity * 100);
        velocitySlider.value = velocity * 100;
        SetAngle(angle * 100);
        angleSlider.value = angle * 100;

        timeSlider.minValue = 0;
        timeSlider.interactable = false;
        unPausedColor = pauseButtonImage.color;
    }

    public void SetGravity(float value)
    {
        gravity = value / 100;
        gravityText.text = $"Gravity:\n{gravity}m/s<sup>2</sup>";
    }

    public void SetHeight(float value)
    {
        height = value / 100;
        heightText.text = $"height:\n{height}mtr";

        platform.transform.localPosition = new Vector3(mass.transform.localPosition.x, height / 2f, 0);
        platform.transform.localScale = new Vector3(0.2f, height, 0);

        mass.transform.localPosition = new Vector3(mass.transform.localPosition.x, height + 0.3f, 0);
    }

    public void SetVelocity(float value)
    {
        velocity = value / 100;
        velocityText.text = $"Velocity:\n{velocity}m/s";

        vectorV.transform.localPosition = new Vector3((velocity * 0.2f) / 2f, 0, 0);
        vectorV.transform.localScale = new Vector3((velocity*0.2f), 0.1f, 0);
        headV.transform.localPosition = new Vector3((velocity * 0.2f), 0, 0);
    }

    public void SetAngle(float value)
    {
        angle = value / 100;
        angleText.text = $"Angle:\n{angle}°";

        mass.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void SetTime(float time)
    {
        if (firstRun || !simulating) return;

        timePassed = time / 100;
        Iteration(timePassed);
    }

    private void Iteration(float time)
    {
        if (firstRun)
            timeSlider.value = time * 100;

        var yPos = -gravity / 2 * Mathf.Pow(time, 2) + voy * (time) + height;
        var xPos = vox * time + initialX;
        var velY = -gravity * time + voy;
        var velTotal =(float) Math.Sqrt(Math.Pow(velY, 2) + Math.Pow(vox, 2));
        var velAngle = (float) Math.Atan(velY / vox) * 180 / Mathf.PI;

        var potentialE = yPos * gravity;
        energySlider.value = potentialE;

        potentialText.text = $"Potential:\n{Math.Round(potentialE, 2)}";
        kineticText.text = $"Kinetic:\n{Math.Round(energy - potentialE, 2)}";

        mass.transform.position = new Vector3(xPos, yPos, 0);
        mass.transform.localEulerAngles = new Vector3(0, 0, velAngle);

        vectorV.transform.localPosition = new Vector3((velTotal * 0.2f) / 2f, 0, 0);
        vectorV.transform.localScale = new Vector3((velTotal * 0.2f), 0.1f, 0);

        headV.transform.localPosition = new Vector3((velTotal * 0.2f), 0, 0);

        distanceTravelled +=(float) Math.Sqrt(Math.Pow(xPos - pastX, 2) + Math.Pow(yPos - pastY, 2));

        pastX = xPos;
        pastY = yPos;

        if (firstRun && distanceTravelled >= nextPath)
        {
            nextPath += pathLine;
            Instantiate(objPrefab, new Vector3(xPos, yPos, 0), quaternion.identity, pathHolder);
        }
    }

    void FixedUpdate()
    {
        if (!simulating || paused) return;

        timePassed += Time.fixedDeltaTime;

        if (timePassed >= flightTime)
        {
            timePassed = flightTime;

            if (firstRun)
            {
                firstRun = false;
                timeSlider.interactable = true;

                if (!paused)
                    PauseSimulation();
            }
        }

        Iteration(timePassed);
    }
}
