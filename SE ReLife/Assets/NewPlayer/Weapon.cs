using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    //private Animator anim;
    private AudioSource _AudioSource;

    public float range = 100f;
    public int bulletsPerMag = 30;
    public int bulletsLeft;
    public float reloadTime = 2f;

    public int currentBullets;

    public enum ShootMode { Auto,Semi }
    public ShootMode shootingMode;

    [Header("UI")]
    public Text ammoText;

    [Header("Setup")]
    public Transform shootPoint;
    public GameObject hitParticles;
    public GameObject bulletImpact;
    public LineRenderer bulletTrail;
    public ParticleSystem muzzleFlash; //
    public float damage = 20f;
    
    [Header("Sound Effects")]
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip emptySound;

    public float fireRate = 0.1f;

    float fireTimer;
    private bool isReloading;
    private bool shootInput;


    private void OnEnable()
    {
        // Update when active state is changed
        UpdateAmmoText(); // Update ammo text

        isReloading = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();
        bulletTrail = GetComponent<LineRenderer>();

        currentBullets = bulletsPerMag;

        UpdateAmmoText(); // Update ammo Text
    }

    // Update is called once per frame
    void Update()
    {
        switch(shootingMode)
        {
            case ShootMode.Auto:
                shootInput = Input.GetButton("Fire1");
            break;

            case ShootMode.Semi:
                shootInput = Input.GetButtonDown("Fire1");
            break;
        }

        if (shootInput)
        {
            if (currentBullets > 0)
                Fire(); // Execute the fire funtion if we press/hold the left mouse botton
            else if(bulletsLeft > 0 && !isReloading)
            {
                StartCoroutine(DoReload());
            }
            else if(currentBullets <= 0)
                _AudioSource.PlayOneShot(emptySound);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentBullets < bulletsPerMag && bulletsLeft > 0 && !isReloading)
                StartCoroutine(DoReload());
        }

        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime; //Add into time counter
    }

    void FixedUpdate()
    {
        //AnimatorStateInfo info = Animation.GetCurrentAnimtorStateInfo(0);

        //isReloading = info.IsName("Reload");
        //if (info.IsName("Fire")) anim.SetBool("Fire , false");

    }
    private void Fire()
    {
        if (fireTimer < fireRate || currentBullets <= 0 || isReloading) return;
        Debug.Log("Fire-d!");

        RaycastHit hit;


        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name + " found!");

            GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            
            GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

            Destroy(hitParticleEffect, 1f);
            Destroy(bulletHole, 2f);

            if(hit.transform.GetComponent<HealthController>())
            {
                hit.transform.GetComponent<HealthController>().ApplyDamage(damage);
            }

            //SpawnBulletTrail(hit.point);
        }

        //anim.CrossFadeInFixedTime("Fire", 0.01f); //Play the fire animation
        
        muzzleFlash.Play(); //Show the muzzle flash
        PlayShootSound(); // Play the shooting sound effect
        

        currentBullets--; // Deduct one bullet

        UpdateAmmoText();// Update ammo text

        fireTimer = 0.0f; //Reset fire timer

    }
    
    private void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject bulletTrailEffect = Instantiate(bulletTrail.gameObject, shootPoint.position, Quaternion.identity);

        LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();

        lineR.SetPosition(0, shootPoint.position);
        lineR.SetPosition(1, hitPoint);

        Destroy(bulletTrailEffect, 1f);
    }

    public void Reload()
    {
        if (bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerMag - currentBullets;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;
        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;

        _AudioSource.PlayOneShot(reloadSound);

        UpdateAmmoText(); // Update ammo text
    }

    private IEnumerator DoReload()
    {
        //AnimationStateInfo info = Animation.GetCurrenAnimetorStateInfo(0);
        //anim.CrossFadeInFixedTime("Reload", 0.01f);
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
        Reload();

    }
    private void PlayShootSound()
    {
        _AudioSource.PlayOneShot(shootSound);
        //_AudioSource.clip = shootSound;
        //_AudioSource.Play();
    }

    private void UpdateAmmoText()
    {
        
        ammoText.text = currentBullets+ "/" +bulletsLeft;
    }
}
