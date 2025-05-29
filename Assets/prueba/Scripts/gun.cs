using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gun : MonoBehaviour
{
    //bullet
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //graficos temporales
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    //bug fixing
    public bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //Display de la municion
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletPerTap + " / " + magazineSize / bulletPerTap);
    }
    private void MyInput()
    {
        //se fija si podes mantener apretado con una metralleta o tapeas como con una escopeta
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Recargar
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //recarga automaticamente cuando dispara sin balas
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)Reload();

        //Disparar
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //disparos seteados a 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //busca el hit point con un rayo
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));//apunta al medio de donde se esta mirando
        RaycastHit hit;

        //se fija si el rayo toca algo
        Vector3 targetPoint;
        if(Physics.Raycast(ray,out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //solo es un punto alejado del jugador


        //calcular direccion de pounto de attackpoint a targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //calcular spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread,spread);

        //Calcular nuva direccion con spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);//agrega spread a la ultima direccion

        //Instantiate Bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position,Quaternion.identity);
        //rota la bala a la direccion
        currentBullet.transform.forward = directionWithSpread.normalized;

        //add force a la bala
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);


        //muzzle flash
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot() entre disparos
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //si es un arma que tapea sale esto
        if (bulletsShot < bulletPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        //permmite disparar e Invoke
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
