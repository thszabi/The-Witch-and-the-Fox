using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GhostInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject ghostPrefab;

    private int ghostsSummoned = 0;
    private int virusesToSummon = 0;
    private int virusesDestroyed = 0;

    private int difficulty = 0;

    private readonly float[] secondsPerWave = { 3.0f, 2.5f, 2.5f, 2.0f, 2.0f, 1.8f, 1.5f, 1.2f, 1.0f, 0.8f };
    private readonly int[] ghostsPerWave = { 1, 1, 2, 3, 3, 4, 5, 6, 6, 7 };
    private readonly float[] ghostMinSpeeds = { 10.0f, 0.8f, 1.2f, 1.2f, 1.8f, 2.5f, 4.0f, 5.0f, 6.0f, 7.0f };
    private readonly float[] ghostMaxSpeeds = { 10.0f, 0.8f, 1.2f, 1.2f, 1.8f, 2.5f, 4.0f, 5.0f, 6.0f, 7.0f };
    private readonly float[] fireballSpeeds = { 2.0f, 2.0f, 2.0f, 3.0f, 3.0f, 3.0f, 4.5f, 5.0f, 5.0f, 5.0f };
    //private readonly int[] ghostsToSummon = { 10, 15, 20, 30, 50, 65, 70, 80, 90, 100 };

    private float secondsUntilNextWave;
    private float secondsUntilNextGhost;

    public void OnGhostDestroyed()
    {
        ++virusesDestroyed;
    }

    public float GetSpraySpeed()
    {
        return fireballSpeeds[difficulty];
    }

    //----------------------------------------------------------------------------------------------------

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Resets the in-game counters.
    private void StartGame()
    {
        ghostsSummoned = 0;
        virusesToSummon = 0;
        virusesDestroyed = 0;
        secondsUntilNextWave = 1.0f;
        secondsUntilNextGhost = 0.0f;
    }

    private void Update()
    {
        if (/*!isGameOver*/ true)
        {
            secondsUntilNextWave -= Time.deltaTime;
            secondsUntilNextGhost -= Time.deltaTime;

            if (secondsUntilNextWave <= 0.0f /*&&
                (ghostsSummoned + virusesToSummon) < ghostsToSummon[difficulty]*/)
            {
                virusesToSummon += ghostsPerWave[difficulty];
                secondsUntilNextWave = secondsPerWave[difficulty];
            }

            if (virusesToSummon > 0 &&
                secondsUntilNextGhost <= 0.0f /*&&
                ghostsSummoned < ghostsToSummon[difficulty]*/)
            {
                CreateGhost();
                --virusesToSummon;
                secondsUntilNextGhost = Random.Range(0.0f, 1.0f);
            }
        }
    }

    void CreateGhost()
    {
        ++ghostsSummoned;
        GameObject newGhost = Instantiate(ghostPrefab);
        newGhost.GetComponent<GhostController>().SetSpeed(ghostMinSpeeds[difficulty]);
    }
}
