using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    float nextTimeToFire = 0;
    private Animator anim;

    [SerializeField] private Transform BarrelEnd;
    [SerializeField] private Transform cameraRotation;
    [SerializeField] private GameObject bullet;
    [SerializeField] private ParticleSystem muzzleFlash;
    public AImanager aiManager;

    public enemyAI enemyAi;

    public Vector3 enemyDeadTans;

    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("playerCamera").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Temporary killing button
        if (Input.GetKeyDown(KeyCode.K))
        {
            KillEnemy();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale += 10;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Time.timeScale -= 10;
        }

        //Mouse click shooting
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            Shoot();
            anim.SetTrigger("bang");
            muzzleFlash.Play();

        }
    }

    public void KillEnemy()
    {
        aiManager.createEnemy();
        isDead = true;
        enemyDeadTans = enemyAi.transform.position;
        aiManager.hits++;
        aiManager.isTraining = false;
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraRotation.position, cameraRotation.forward, out hit, 100))
        {
            Debug.Log(hit.transform.tag);

            if (hit.transform.tag == "Enemy")
            {
                KillEnemy();
            }
        }
    }
}
