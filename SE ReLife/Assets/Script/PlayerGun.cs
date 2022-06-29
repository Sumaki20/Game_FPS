using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private GunObject[] guns;

    private GunObject currentGun;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GunObject gun in guns)
        {
            gun.gameObject.SetActive(false);
        }
        currentGun = guns[0];
        currentGun.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i <= guns.Length; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                foreach (GunObject gun in guns)
                {
                    gun.gameObject.SetActive(false);
                }
                currentGun = guns[i - 1];
                currentGun.gameObject.SetActive(true);
            }
        }

        currentGun.SetClickHold(Input.GetMouseButton(0));

        if (Input.GetKeyDown("r"))
        {
            currentGun.Reload();
        }
    }
}
