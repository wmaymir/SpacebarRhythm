using Unity.VisualScripting;
using UnityEngine;

public class Note : MonoBehaviour
{
    //in the future, the note's speed should scale

    float speed = -0.4166666666666667f;
    Vector3 boundry;

    private void Awake()
    {
        boundry = new Vector3(0, 0, 0);
        //this is set to the location of the judgementbar as a temporary crutch
    }

    //calculations for speed to make it arrive at the bar is like so:
    //the bar is 60m away from the spawn
    //the furthest away broadcasted beat is 4 beats away for the notes being spawned
    //the bar must travel 15m per beat

    //1b = 15m = 0.6s
    //what distance do we need to go every 1/60 of a second to go 15m in 0.6 seconds
    //speed = distance over time

    //total frames (availible) = 0.6 / (1/60) = 0.6 * 60 = 36 frames
    //distance (needed) per frame = 15m / 36 frames = 5/12 meters, or 0.4167m
    //the speed should eventually not be hardcoded in this way
    void Update()
    {
        transform.Translate(Vector3.forward * speed);

        if(transform.position.x < boundry.x)
        {
            Destroy(this.gameObject);
        }
    }
}
