using UnityEngine;

public class DisplayChart : MonoBehaviour
{

    public GameObject note;
    Vector3 spawnPosition;
    Quaternion spawnRotation;

    private void Awake()
    {
        spawnPosition = new Vector3(60, 0, 0);
        spawnRotation = Quaternion.Euler(0, 90, 90);
    }

    private void OnEnable()
    {
        //Composer.OnNoteSpawnInfoReady += SpawnNote;
    }

    private void OnDisable()
    {
        //Composer.OnNoteSpawnInfoReady -= SpawnNote;
    }

    private void SpawnNote(string[,] noteInfo)
    {
        if (noteInfo[0, 1] == "Cb-A")
        {
            Instantiate(note, spawnPosition, spawnRotation);
        }
    }
}
