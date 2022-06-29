using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunObject : MonoBehaviour
{
    public enum FireType
    {
        semi,
        auto
    }
    [SerializeField] private GameObject firePoint;      //จุดที่ยิง
    [SerializeField] private GameObject bulletOBJ;      //กระสุนที่จะออก
    [SerializeField] private FireType fireType;         //ประเภทการยิง
    [SerializeField] private float fireRange;           //ระยะ
    [SerializeField] private int fireCount;             //จำนวนที่จะยิง
    [SerializeField] private float fireCool;            //cooldown การยิงต่อรอบ
    [SerializeField] private int ammoMax;               //จำนวนกระสุน
    [SerializeField] private int reloadCount;           //จำนวน Reload
    [SerializeField] private float reloadTimeP;          //เวลา Reload
    [SerializeField] private float spreadRate;          //อัตราการกระจาย

    [SerializeField] private AudioClip fireSound;       //{เสียง
    [SerializeField] private AudioClip emptySound;
    [SerializeField] private AudioClip reloadSound;     //}

    private int ammoNow;
    private float fireCoolNow = 0;

    private AudioSource soundPlayer;

    private bool isReload = false;
    private bool reloading = false;
    private bool isClickHold = false;
    private bool isSemiFired = false;


    // Start is called before the first frame update
    void Start()
    {
        ammoNow = ammoMax;
        soundPlayer = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isClickHold)
        {
            if (!isReload && !reloading && fireCoolNow <= 0 && ((fireType == FireType.semi && !isSemiFired) || fireType != FireType.semi))
            {
                if (fireType == FireType.semi)
                {
                    isSemiFired = true;
                }
                if (ammoNow > 0)
                {
                    soundPlayer.clip = fireSound;
                    soundPlayer.Play();
                    ammoNow -= 1;
                    fireCoolNow = fireCool;
                    if (bulletOBJ)
                    {
                        for (int i = 0; i < fireCount; i++)
                        {
                            GameObject proj = Instantiate(bulletOBJ, firePoint.transform.position, firePoint.transform.rotation);
                            float randomX = Random.Range(-spreadRate, spreadRate);
                            float randomY = Random.Range(-spreadRate, spreadRate);
                            float randomAngleX = proj.transform.rotation.x * randomX;
                            float randomAngleY = proj.transform.rotation.y * randomY;
                            proj.transform.localEulerAngles = new Vector3(proj.transform.localEulerAngles.x + (randomAngleX), proj.transform.localEulerAngles.y + (randomAngleY), proj.transform.localEulerAngles.z);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < fireCount; i++)
                        {
                            RaycastHit hit;
                            float randomX = Random.Range(-spreadRate, spreadRate);
                            float randomY = Random.Range(-spreadRate, spreadRate);
                            Ray ray = new Ray(firePoint.transform.position, firePoint.transform.forward + (firePoint.transform.right * randomX) + (firePoint.transform.up * randomY));
                            if (Physics.Raycast(ray, out hit, fireRange))
                            {

                            }
                        }
                    }
                }
                else
                {
                    if (fireType == FireType.semi)
                    {
                        isSemiFired = true;
                    }
                    soundPlayer.clip = emptySound;
                    soundPlayer.Play();
                }
            }
            else
            {
                if (reloading && reloadCount < ammoMax)
                {
                    isReload = false;
                }
            }
        }
        fireCool -= Time.deltaTime;
    }

    public void SetClickHold(bool click)
    {
        isClickHold = click;
        if (isClickHold == false)
        {
            isSemiFired = false;
        }
    }

    public void Reload()
    {
        if (!isReload && ammoNow < ammoMax)
        {
            StartCoroutine(Reloading());
        }
    }
    private IEnumerator Reloading()
    {
        isReload = true;
        reloading = true;
        while (isReload)
        {
            soundPlayer.clip = reloadSound;
            soundPlayer.Play();
            yield return new WaitForSeconds(reloadTimeP);
            ammoNow += reloadCount;
            if (ammoNow == ammoMax)
            {
                isReload = false;
            }
        }
        reloading = false;
    }
}