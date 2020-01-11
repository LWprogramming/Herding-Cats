using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    private Rigidbody catRigidBody;
    private float secondsSinceLastRun; // the number of seconds since the last time the cat ran away from the player
    private bool isMovingRandomly;
    private bool isHome; // if the cat has arrived home. Ensure they don't leave again.
    private GameObject home;
    private float homeRadius; // for convenience.
    private GameObject player;
    private Score score; // reference to the Score script.

    public float restTimeAfterRun; // how long the cat should rest after running away from the player before starting random motion again.
    public float distanceToRun; // how close the player can get before the cat starts running away
    public float runAwaySpeed;

    // Start is called before the first frame update
    void Start()
    {
        home = GameObject.Find("Home");
        Physics.IgnoreCollision(home.GetComponent<SphereCollider>(), this.GetComponent<SphereCollider>()); // so the cat can pass into home, but the collider will still be active.
        homeRadius = home.transform.localScale.x / 2; // TODO: this isn't great but since we know it's a sphere, we just take one of the coordinates and we're set. NOTE: DIVIDE BY 2 BECAUSE THE SCALE IS FOR THE DIAMETER

        player = GameObject.Find("Player");
        catRigidBody = GetComponent<Rigidbody>();
        secondsSinceLastRun = 0f;
        isMovingRandomly = false;
        score = GameObject.FindObjectOfType<Score>();
    }

    void FixedUpdate()
    {
        float distanceToPlayer = (player.transform.position - this.transform.position).magnitude;
        if (distanceToPlayer < distanceToRun) {
            secondsSinceLastRun = 0;
            RunAway();
        }
        else {
            secondsSinceLastRun += Time.deltaTime; // add in the time from the past frame.
            if (secondsSinceLastRun < restTimeAfterRun) {
                // we ran away within the past restTimeAfterRun seconds, so we can't run away right now.
                // do nothing
            }
            else {
                if (!isMovingRandomly) {
                    isMovingRandomly = true;
                    // restart invoking RandomMotion.
                    float startRandomMovementAtTime = Random.Range(0f, 4f);
                    float randomMovementInterval = Random.Range(3f, 6f);
                    InvokeRepeating("RandomMotion", startRandomMovementAtTime, randomMovementInterval);
                }
            }
        }

        float distanceToHomeCenter = (this.transform.position - home.transform.position).magnitude;
        if (homeRadius - distanceToHomeCenter > homeRadius / 20) {
            if (!isHome) {
                // this is the first and only time we update the score.
                // after this, we'll update isHome so we won't ever update the score again for the same cat.
                score.updateScore();
            }
            isHome = true; // if cat is home, it stays home
        }
        if (isHome) {
            // going to manually make the collider act "hollow" by reflecting velocity whenever the cat attempts to leave home.
            // technically we probably could do this with a mesh collider, but this is easier to reason about for me right now.
            // source: https://answers.unity.com/questions/1074916/create-hollow-sphere-with-objects-bouncing-around.html
            // note that changing the velocity with Vector3.Reflect probably requires that the normal vector param be normalized. Not sure why-- it's not documented or noted in the answer anywhere.
            // Also note that changing velocity directly is discouraged, so ehh.... TODO maybe.
            if (distanceToHomeCenter > homeRadius) {
                // cat's attempting to leave!
                catRigidBody.velocity = Vector3.Reflect(catRigidBody.velocity, (home.transform.position - this.transform.position).normalized);
                catRigidBody.position = home.transform.position + (this.transform.position - home.transform.position).normalized * homeRadius * 0.999f;
            }
        }
    }

    void RunAway() {
        Debug.Log("run away!");
        Vector3 direction = (this.transform.position - player.transform.position).normalized;
        catRigidBody.AddForce(direction * runAwaySpeed * Time.deltaTime);
    }

    void RandomMotion() {
        Debug.Log("called random motion!");
        float randomForceMagnitude = Random.Range(10000f, 40000f);
        Vector2 randomForce2D = randomForceMagnitude * Random.insideUnitCircle;
        Vector3 randomForce = new Vector3(randomForce2D.x, 0, randomForce2D.y);
        catRigidBody.AddForce(randomForce * Time.deltaTime);
    }
}
