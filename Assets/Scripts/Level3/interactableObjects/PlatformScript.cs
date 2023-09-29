using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public GameObject explosionParticles;
    public AudioSource wallDestroySound;
    public Transform explosionPosition;
    public bool oneTime;

    // Start is called before the first frame update
    void Start()
    {
        oneTime = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ApplyPlatformStrategy(IPlatformStrategy strategy)
    {
        if (oneTime)
        {
            oneTime = false;
            strategy.BreakPlatform(this);
        }
    }

    public void SetDesiredStrategy()
    {
        //adaug strategiile posibile
        gameObject.AddComponent<BreakingPlatformWithRespawnLevel3Script>();
        gameObject.AddComponent<BreakablePlatformsLevel3Script>();
        if (gameObject.name == "TilemapBreakablePlatform1" || gameObject.name == "TilemapBreakablePlatform2" || gameObject.name == "TilemapBreakablePlatform4"
            || gameObject.name == "TilemapBreakablePlatform6" || gameObject.name == "TilemapBreakablePlatform7")
        {
            ApplyPlatformStrategy(gameObject.GetComponent<BreakablePlatformsLevel3Script>());
        }
        else if (gameObject.name == "TilemapBreakablePlatform3")
        {
            ApplyPlatformStrategy(gameObject.GetComponent<BreakingPlatformWithRespawnLevel3Script>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "PlayerLevel3")
        {
            SetDesiredStrategy();
        }
    }

}
