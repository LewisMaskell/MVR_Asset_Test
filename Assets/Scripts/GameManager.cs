using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject sunseekerPrefab;
    [SerializeField] GameObject[] waypointSets;
    [SerializeField] GameObject restartPanel;
    private BoxCollider goal;
    private GameObject player;
    private float angularSpeed = 10f;
    private bool inGoal;
    private bool goalActive = false;
    private Coroutine goalroutine;

    void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 0, 800), new Quaternion(0, 1, 0, 0));
        goal = GetComponent<BoxCollider>();

        //create ships for each of the bezier paths
        
        for (int i = 0; i < waypointSets.Length; i++)
        {
            Transform[] waypoints = new Transform[4];
            GameObject sunseeker = Instantiate(sunseekerPrefab);
            for (int j = 0; j < 4; j++)
            {
                
                waypoints[j] = waypointSets[i].transform.GetChild(j).transform;
                
                
            }
            NPCMover script = sunseeker.GetComponent<NPCMover>();
            script.SetState(new BezierPath(script, transform.position, Random.Range(5,10), angularSpeed, waypoints));
        }
        for (int i = 0;i < 5;i++)
        {
            GameObject sunseeker = Instantiate(sunseekerPrefab);
            NPCMover script = sunseeker.GetComponent<NPCMover>();
            script.SetState(new FigureEightPath(
                script, 
                new Vector3(Random.Range(-300,300), 0, Random.Range(200, 600)), //origin
                Random.Range(5,10), //speed
                Random.Range(100, 300) //scale
                ));
        }
        for (int i = 0; i < 5; i++)
        {
            GameObject sunseeker = Instantiate(sunseekerPrefab);
            NPCMover script = sunseeker.GetComponent<NPCMover>();
            script.SetState(new CirclePath(
                script,
                new Vector3(Random.Range(-300, 300), 0, Random.Range(200, 600)), //origin
                Random.Range(1,3), //speed
                Random.Range(100, 300) //scale (radius)
                
                ));
        }
    }
    private void Update()
    {

        if (player.GetComponent<Rigidbody>().velocity.magnitude <= .3f && inGoal && !goalActive)
        {
            goalroutine = StartCoroutine(GoalCoroutine());
        }
        if (goalActive && player.GetComponent<Rigidbody>().velocity.magnitude > .3f)
        {
            Debug.Log("cancelling coroutine");
            goalActive = false;
            StopCoroutine(goalroutine);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inGoal = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inGoal = false;
        }
    }
    private IEnumerator GoalCoroutine()
    {
        Debug.Log("coroutineStarted");
        goalActive = true;
        yield return new WaitForSeconds(3);
        Debug.Log("complete");
        restartPanel.SetActive(true);
        
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
