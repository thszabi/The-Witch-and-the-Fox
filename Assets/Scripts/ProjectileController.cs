using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private GameObject poofPrefab;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.position - GameObject.Find("Witch").transform.position;
        direction.y = 0;
        direction = Vector3.Normalize(direction);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos = pos + (direction * Time.deltaTime * speed);

        transform.position = pos;
    }
}
