using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchController : MonoBehaviour
{
    [SerializeField] private GameObject foxGameObject;
    [SerializeField] private GameObject projectileGameObject;
    private AudioSource shootingSound;

    private Transform muzzleTransform;

    // Start is called before the first frame update
    private void Start()
    {
        muzzleTransform = transform.Find("GunBroom/Muzzle");
        shootingSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shootingSound.Play();

            Vector3 witchDirection = gameObject.transform.position - foxGameObject.transform.position;
            float witchAngle = Mathf.Atan2(witchDirection.x, witchDirection.z) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.Euler(90.0f, witchAngle, 0);

            Instantiate(projectileGameObject, muzzleTransform.position, Quaternion.Euler(-90.0f, witchAngle, 0.0f));
        }
    }
}
