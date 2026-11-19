using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Note : MonoBehaviour
{
    // In the future, the note's speed should scale

    float speed = -0.4166666666666667f;
    Vector3 boundry;

    private void Awake()
    {
        boundry = new Vector3(0, 0, 0);
        // This is set to the location of the judgementbar as a temporary crutch
    }

    // Calculations for speed to make it arrive at the bar is like so:

    // The bar is 60m away from the spawn
    // The furthest away broadcasted beat is 4 beats away for the notes being spawned
    // The bar must travel 15m per beat

    // For 1b (0.6s), lets travel 15m
    // 25m needs to be traveled in 1s (15 / 0.6 = 25)
    // 0.4166666666666667 needs to be traveled in 1/60th of a s (25 / 60 = 5/12)

    // The speed should eventually not be hardcoded in this way

    void Update()
    {
        transform.Translate(Vector3.forward * speed);

        if(transform.position.x < boundry.x)
        {
            Destroy(this.gameObject);
        }
    }
}
