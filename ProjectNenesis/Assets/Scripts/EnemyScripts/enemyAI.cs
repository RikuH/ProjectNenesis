using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyAI : MonoBehaviour
{

    private float speed = 6f; //2.5f
    float y = 0;

    public Text showInfo;
    string infoStr;

    private bool initialize = false;
    private Transform player;
    private NeuralNetwork net;
    private Rigidbody rb;
    private Material mats;

    public AImanager aiManager;

    public float[] inputs;
    public float[] output;

    float[] newInputs;
    public bool isDead = false;

    bool isShooting = false;

    string delayedInfo = "";

    Animator anim;

    private NavMeshAgent agent;

    public Gun gun;

    bool startDelay = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        newInputs = new float[2];
        newInputs[0] = new float();
        newInputs[1] = new float();
        inputs = new float[2];
        output = new float[2];
        //output[0] = 0;
        //inputs[0] = 0;
        //inputs[1] = 0;
        gun = FindObjectOfType<Gun>();
        gun.enemyAi = this;
        //gun.enemyDeadTans.x = 0;
        //gun.enemyDeadTans.z = 0;
        newInputs[0] = gun.enemyDeadTans.x;
        newInputs[1] = gun.enemyDeadTans.z;
        StartCoroutine(killDelay());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (initialize == true)
        {
            inputs[0] = newInputs[0];
            inputs[1] = newInputs[1];

            output[0] = net.FeedForward(inputs)[0];
            output[1] = net.FeedForward(inputs)[1];

            if (!isShooting)
            {
                //rb.velocity = speed * transform.forward;
                agent.SetDestination(new Vector3(output[0] * 100, this.transform.position.y, output[1] * 100));

/*
                //Kills enemy if it stops
                if(agent.velocity.magnitude <= 0.5f && startDelay){
                    Debug.Log("Input[0] :" + inputs[0] + " Input[1] :" + inputs[1] + " Output: " +  output[0]);
                   gun.KillEnemy();
                }
*/
                //agent.SetDestination(this.transform.position + (transform.forward * speed));
                //rb.angularVelocity = new Vector3(0, output[0] * 500f, 0);

            }

            net.AddFitness((Mathf.Abs(inputs[0] + inputs[1])));
        }

        badPathFinder();
        raycastSystem();
    }

    IEnumerator killDelay(){
        yield return new WaitForSeconds(1);
        startDelay = true;
    }

    void raycastSystem()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
        }
        else
        {
           // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }
    }

    public void init(NeuralNetwork net, Transform player)
    {
        this.player = player;
        this.net = net;
        initialize = true;
    }

    IEnumerator ShootDelay(){
        yield return new WaitForSeconds(1f);           
        //aiManager.createEnemy();
        gun.KillEnemy();
    }

    void badPathFinder()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(this.transform.position.x, this.transform.position.y + 0.85f, this.transform.position.z), this.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.transform.name == "Player")
            {
                Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 0.85f, this.transform.position.z), this.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

                //Shooting animation
                anim.SetBool("eAim", true);
                anim.SetBool("eShoot", true);
                isShooting = true;
                Debug.Log("Ampuu");
                StartCoroutine(ShootDelay());
            }
            else
            {
                isShooting = false;
                anim.SetBool("eAim", false);
                anim.SetBool("eShoot", false);
            }

            //if (hit.transform.tag == "Box" || hit.transform.tag == "Wall")
            //{
            //    //Debug.Log("cube eessä " + rb.velocity.x + " " + rb.velocity.z);
            //    Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 0.85f, this.transform.position.z), this.transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            //    GameObject nextCube;
            //    nextCube = hit.transform.gameObject;
//
            //    float xMinus, zMinus;
            //    xMinus = nextCube.transform.position.x - this.transform.position.x;
            //    zMinus = nextCube.transform.position.z - this.transform.position.z;
//
            //    if (rb.velocity.x > 2 || rb.velocity.x < -1)
            //    {
            //        //Debug.Log("Kaantyy x");
            //        if (xMinus < 2 && xMinus > -2)
            //        {
            //            y += 4;
            //            //rb.angularVelocity += new Vector3(0, 30, 0);
            //            this.transform.Rotate(0f, y, 0f);
            //            //Debug.Log("x: " + xMinus);
            //        }
            //    }
            //    else if (rb.velocity.z > 2 || rb.velocity.z < -1)
            //    {
            //        //Debug.Log("Kaantyy z");
            //        if (zMinus < 2 && zMinus > -2)
            //        {
            //            y += 4;
            //            //rb.angularVelocity += new Vector3(0, 30, 0);
            //            this.transform.Rotate(0f, y, 0f);
            //            //Debug.Log("z: " + zMinus);
            //        }
            //    }
            //}

            //Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 0.85f, this.transform.position.z), this.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
        else
        {
            //Debug.DrawRay(new Vector3(this.transform.position.x, this.transform.position.y + 0.85f, this.transform.position.z), transform.TransformDirection(Vector3.forward) * 1000, Color.green);
        }
    }

    public string enemyInfo()
    {
        StartCoroutine(delay());
        return delayedInfo;
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1);
        delayedInfo = "Input 1: " + inputs[0] + "\nInput 2: " + inputs[1] + "\nOutput 1: " + output[0] + "\nOutput 2: " + output[1] + "\nFitness: " + net.GetFitness();
    }
}
