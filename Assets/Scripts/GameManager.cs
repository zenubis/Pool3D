using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject whiteBallPrefab;
    public GameObject redBallPrefab;

    public GameObject whiteBall;
    public Rigidbody whiteBallRB;

    private int score;
    
    private Text txtScore;
    private List<GameObject> redBallList;

    // constants
    private const int MAX_RED_BALL = 10;

    // Use this for initialization
    void Start ()
    {
        txtScore = GameObject.Find("TxtScore").GetComponent<Text>();
        
        whiteBall = Instantiate(whiteBallPrefab);
        whiteBallRB = whiteBall.GetComponent<Rigidbody>();
        whiteBall.tag = "WhiteBall";

        redBallList = new List<GameObject>(MAX_RED_BALL);
        for (int i = 0; MAX_RED_BALL > i; ++i) {
            GameObject rb = Instantiate(redBallPrefab);
            rb.SetActive(true);
            rb.tag = "RedBall";
            redBallList.Add(rb);
        }
        ResetTable();
    }

    public void ResetTable()
    {
        // layout the white and red balls nicely on the table
        score = 0;
        UpdateScoreLabel();
        ResetWhiteBallPosition();

        redBallList[0].transform.position = new Vector3(-0.45f - (0 * 0.08f), 0.04f, 0);

        redBallList[1].transform.position = new Vector3(-0.45f - (1 * 0.08f), 0.04f, -0.04f + (0 * 0.08f));
        redBallList[2].transform.position = new Vector3(-0.45f - (1 * 0.08f), 0.04f, -0.04f + (1 * 0.08f));

        redBallList[3].transform.position = new Vector3(-0.45f - (2 * 0.08f), 0.04f, -0.08f + (0 * 0.08f));
        redBallList[4].transform.position = new Vector3(-0.45f - (2 * 0.08f), 0.04f, -0.08f + (1 * 0.08f));
        redBallList[5].transform.position = new Vector3(-0.45f - (2 * 0.08f), 0.04f, -0.08f + (2 * 0.08f));

        redBallList[6].transform.position = new Vector3(-0.45f - (3 * 0.08f), 0.04f, -0.12f + (0 * 0.08f));
        redBallList[7].transform.position = new Vector3(-0.45f - (3 * 0.08f), 0.04f, -0.12f + (1 * 0.08f));
        redBallList[8].transform.position = new Vector3(-0.45f - (3 * 0.08f), 0.04f, -0.12f + (2 * 0.08f));
        redBallList[9].transform.position = new Vector3(-0.45f - (3 * 0.08f), 0.04f, -0.12f + (3 * 0.08f));
        for (int i = 0; MAX_RED_BALL > i; ++i) {
            redBallList[i].SetActive(true);
            Rigidbody rb = redBallList[i].GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
        }
    }

    public void DropBall(GameObject ball)
    {
        if (ball == null) {
            return;
        }

        if (ball.tag.Equals("WhiteBall")) {
            score--;
            ResetWhiteBallPosition();
        }
        else if (ball.tag.Equals("RedBall")) {
            score++;
            ball.SetActive(false);
        }
        else {
            Debug.Log("Unknown ball or gameobject:" + ball.name + " tag:" + ball.tag);
            ball.SetActive(false);
        }

        UpdateScoreLabel();

        // check if we need to reset the table
        bool noMoreRedBall = true;
        for (int i = 0; MAX_RED_BALL > i; ++i) {
            if (redBallList[i].activeInHierarchy) {
                noMoreRedBall = false;
                break;
            }
        }

        if (noMoreRedBall) {
            ResetTable();
        }
    }

    void UpdateScoreLabel()
    {
        if (txtScore) {
            txtScore.text = "Score: " + score;
        }
    }

    void ResetWhiteBallPosition()
    {
        // reset white ball back to original place
        whiteBall.transform.position = new Vector3(0.75f, 0.04f, 0);
        whiteBallRB.velocity = new Vector3(0, 0, 0);
        whiteBallRB.angularVelocity = new Vector3(0, 0, 0);
    }
}
