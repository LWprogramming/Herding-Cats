using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int score; // note: MonoBehaviour is single-threaded according to this: https://answers.unity.com/questions/394643/avoiding-race-conditions-1.html
    // so we don't need semaphores.
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateScore() {
        score++;
        scoreText.text = score.ToString();
    }
}
