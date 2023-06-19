using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<MathGameLevel> levels;
    [SerializeField] MathGameLevel noLevel;
    [SerializeField] float ballGravity;
    [SerializeField] Rigidbody2D ball;
    [SerializeField] int ignoreLayer;

    [SerializeField] GameObject objectivePrefab;
    [SerializeField] GameObject objHolder;

    [SerializeField] TMP_Dropdown levelDropdown;
    [SerializeField] Camera cam;
    [SerializeField] Grapher grapher;

    [SerializeField] Button spawn;
    [SerializeField] Button unspawn;

    [HideInInspector] public bool isPlaying;

    int defaultBallLayer;
    int level;
    [ShowNonSerializedField] List<Transform> objectives;

    void Start()
    {
        defaultBallLayer = ball.gameObject.layer;
        ball.gameObject.layer = ignoreLayer;
        ball.transform.position = objHolder.transform.position;
        ball.gravityScale = 0;

        objectives = new List<Transform>();
        grapher.canGraph = true;

        levelDropdown.ClearOptions();
        var options = new List<string>();
        for (int i = 0; i < levels.Count; i++)
        {
            options.Add($"Level{i+1}");
        }
        options.Add("No Level");
        
        levelDropdown.AddOptions(options);
        levelDropdown.value = levels.Count;
    }

    void Update()
    {
        if(isPlaying && ball.transform.position.y < -(grapher.yRange / 2))
        {
            StopGame();
        }
    }

    public void StartGame()
    {
        if (isPlaying)
            return;

        isPlaying = true;
        ball.gravityScale = ballGravity;
        ball.gameObject.layer = defaultBallLayer;

        grapher.canGraph = false;
        spawn.enabled = false;
        unspawn.enabled = false;

        if (level == levels.Count)
        {
            noLevel.ballPosition = ball.transform.position;
            noLevel.objPositions.Clear();
            foreach (var objective in objectives)
            {
                noLevel.objPositions.Add(objective.transform.position);
            }
        }
    }

    public void StopGame()
    {
        if (!isPlaying)
            return;

        isPlaying = false;
        ball.gravityScale = 0;
        ball.velocity = Vector2.zero;
        ball.gameObject.layer = ignoreLayer;

        grapher.canGraph = true;
        spawn.enabled = true;
        unspawn.enabled = true;

        if (level == levels.Count)
        {
            objectives.Clear();
            for (int i = 0; i < objHolder.transform.childCount; i++)
            {
                Destroy(objHolder.transform.GetChild(i).gameObject);
            }

            ball.transform.position = noLevel.ballPosition;
            foreach (var objective in noLevel.objPositions)
            {
                Spawn(objective);
            }
        }
        else
        {
            LoadLevel(level);
        }
    }

    public void LoadLevel(int level)
    {
        this.level = level;

        objectives.Clear();
        for (int i = 0; i < objHolder.transform.childCount; i++)
        {
            Destroy(objHolder.transform.GetChild(i).gameObject);
        }

        if (level == levels.Count)
            return;

        ball.position = levels[level].ballPosition;
        foreach (var position in levels[level].objPositions)
        {
            Spawn(position);
        }
    }

    public void Score(Transform objScored)
    {
        objectives.Remove(objScored);

        StartCoroutine(Wait(.5f, objScored));
    }

    public void UnSpawn()
    {
        if(objectives.Count > 0)
        {
            objectives.RemoveAt(objectives.Count-1);
            Destroy(objHolder.transform.GetChild(objectives.Count).gameObject);
        }
    }

    public void Spawn() => Spawn(objHolder.transform.position);

    public void Spawn(Vector2 position)
    {
        var instance = Instantiate(objectivePrefab, position, Quaternion.identity, objHolder.transform);
        objectives.Add(instance.transform);

        instance.GetComponent<Objective>().manager = this;
        var dragable = instance.GetComponent<Dragable>();
        dragable.cam = cam;
        dragable.manager = this;
    }

    IEnumerator Wait(float x, Transform objScored)
    {
        yield return new WaitForSeconds(x);
        try
        {
            Destroy(objScored.gameObject);
        }
        catch
        {

        }

        if (objectives.Count == 0)
        {
            grapher.UnGraph();
            StopGame();
        }
    }
}