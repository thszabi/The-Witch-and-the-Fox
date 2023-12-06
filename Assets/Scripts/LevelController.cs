using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelController : MonoBehaviour
{
    private int spiritsCollected = 0;

    private TMP_Text spiritCounterText;

    public void OnSpiritsCollected(int spiritsInInventory)
    {
        spiritsCollected += spiritsInInventory;
        spiritCounterText.text = spiritsCollected.ToString();

        if (spiritsCollected >= 50)
        {
            SceneManager.LoadScene("WinScene");
        }
    }

    public void LoseSpirit()
    {
        --spiritsCollected;
        spiritCounterText.text = spiritsCollected.ToString();

        if (spiritsCollected <= 0)
        {
            SceneManager.LoadScene("LoseScene");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spiritCounterText = GameObject.Find("/Canvas/SpiritCounter").GetComponent<TMP_Text>();

        Physics.IgnoreLayerCollision(6, 0, true); //Ghost goes through forest
        Physics.IgnoreLayerCollision(6, 3, true); //Ghost goes through Fox
        Physics.IgnoreLayerCollision(6, 6, true); //Ghost goes through Ghost
        Physics.IgnoreLayerCollision(7, 0, true); //Projectile goes through forest
        Physics.IgnoreLayerCollision(7, 7, true); //Projectile goes through projectile

        spiritsCollected = 10;
        spiritCounterText.text = spiritsCollected.ToString();
    }
}
