using UnityEngine;

public class GhostController : MonoBehaviour
{
    public AudioClip gotHitSound;

    [SerializeField] private GameObject spiritPrefab;
    [SerializeField] private float minSpeed = 5.0f;
    [SerializeField] private float maxSpeed = 10.0f;

    private GameObject witchGameObject;
    private LevelController levelController;
    private AudioSource audioSource;

    private float speed = 1.0f;
    private bool isAlive = true;

    private const float FLOATING_HEIGHT = 1.6f;

    public void SetSpeed(float p_fSpeed)
    {
        //unused, should be deleted during cleanup
        //m_fSpeed = p_fSpeed;
    }

    void Start()
    {
        witchGameObject = GameObject.Find("Witch");
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        audioSource = GetComponent<AudioSource>();
        isAlive = true;

        float randomDegree = Random.Range(0f, 359f);

        float r = 25;
        //Select a random direction
        float degree = randomDegree * (Mathf.PI / 180f);
        Vector3 witchPos = witchGameObject.transform.position;

        transform.position = new Vector3(r * Mathf.Cos(degree), FLOATING_HEIGHT, r * Mathf.Sin(degree)) + witchPos;

        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }

        Vector3 position = transform.position;

        Vector3 destination = witchGameObject.transform.position;

        if (Vector3.Distance(position, destination) > 2.0f)
        {
            Vector3 direction = destination - position;
            direction.Normalize();
            direction.y = 0;

            position = position + (direction * Time.deltaTime * speed);

            transform.position = position;

            Vector3 ghostDirection = position - destination;
            float ghostAngle = Mathf.Atan2(ghostDirection.x, ghostDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(90.0f, ghostAngle, 0);
        }
        else
        {
            levelController.LoseSpirit();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            audioSource.PlayOneShot(gotHitSound);

            Vector3 spiritPos = new Vector3(transform.position.x, 1.0f, transform.position.z);
            Instantiate(spiritPrefab, spiritPos, Quaternion.identity);
            Destroy(collision.gameObject);
            
            Destroy(gameObject, 1.0f);
            GetComponent<MeshRenderer>().enabled = false;
            isAlive = false;
        }
    }

    void OnExplosionAnimationFinished()
    {
        Destroy(gameObject);
    }
}
