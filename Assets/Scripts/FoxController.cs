using UnityEngine;
using System.Collections.Generic;

public class FoxController : MonoBehaviour
{
    [SerializeField] private float speed = 1500.0f;

    private CharacterController characterController;
    private LevelController levelController;
    private Animator animator;

    [SerializeField]
    private List<AudioSource> m_audioSources;

    private Vector3 prevVelocity;
    private Vector3 playerVelocity;

    private int spiritsInInventory = 0;
    private bool isDizzy = false;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
    }

    private void Start()
    {
        prevVelocity = Vector3.zero;
        playerVelocity = Vector3.zero;

        spiritsInInventory = 0;
        isDizzy = false;
}

    private void FixedUpdate()
    {
        Vector3 v = CalculateMovementDirection();
        if (!isDizzy)
        {
            v = v * speed * Time.deltaTime;

            //100 Unity unit = 2 Position unit
            characterController.SimpleMove(v);

            SetOrientation(v);
        }
        else
        {
            v = Vector3.zero;
        }

        SetAnimation(v);

        prevVelocity = v;
    }

    // Calculate the direction of the movement based on camera orientation.
    private Vector3 CalculateMovementDirection()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    private void SetOrientation(Vector3 v)
    {
        if (v.magnitude > 0.01f)
        {
            float angle = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg + 180.0f;

            Quaternion originalRotation = transform.rotation;
            Quaternion goalRotation = Quaternion.AngleAxis(angle, Vector3.up);
            transform.rotation = Quaternion.Lerp(originalRotation, goalRotation, 0.25f);
        }
    }

    private void SetAnimation(Vector3 v)
    {
        if (prevVelocity.magnitude < 0.01f && v.magnitude > 0.01f)
        {
            animator.SetTrigger("StartRunning");
        }
        else if (v.magnitude < 0.01f && prevVelocity.magnitude > 0.01f)
        {
            animator.SetTrigger("StopRunning");
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            m_audioSources[0].Play();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Fox: OnCollisionEnter: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            m_audioSources[1].Play();

            if (!isDizzy)
            {
                animator.SetTrigger("GotHit");
                isDizzy = true;
                playerVelocity = Vector3.zero;
                Invoke(nameof(OnDizzinessEnded), 2.535f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Fox: OnTriggerEnter: " + other.gameObject.name);

        if (other.CompareTag("Spirit"))
        {
            Destroy(other.gameObject);
            ++spiritsInInventory;
        }
        else if (other.name.Equals("Cauldron"))
        {
            levelController.OnSpiritsCollected(spiritsInInventory);
            spiritsInInventory = 0;
        }
    }

    private void OnDizzinessEnded()
    {
        isDizzy = false;
    }
}
