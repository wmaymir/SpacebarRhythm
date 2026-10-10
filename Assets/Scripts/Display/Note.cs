using Unity.VisualScripting;
using UnityEngine;

public class Note : MonoBehaviour
{
    //in the future, the note's speed should scale

    float speed = -0.25f;
    Vector3 boundry;

    private void Awake()
    {
        boundry = new Vector3(0, 0, 0);
        //this is set to the location of the judgementbar as a temporary crutch
    }

    //calculations for speed to make it arrive at the bar is like so:
    //the bar is 60m away from the spawn, the framerate is 60
    //the furthest away broadcasted beat is 4 beats away for the notes being spawned
    //so the speed is .25
    //the speed should eventually not be hardcoded in this way
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if(transform.position.x < boundry.x)
        {
            Destroy(this.gameObject);
        }
    }
}
