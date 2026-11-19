using UnityEngine;

public class DisplayChart : MonoBehaviour
{

    [SerializeField] private GameObject _note;
    Vector3 spawnPosition;
    Quaternion spawnRotation;

    private int _visualOffset = 0;

    private void Awake()
    {
        spawnPosition = new Vector3(60 + _visualOffset, 0, 0);
        spawnRotation = Quaternion.Euler(0, 90, 90);
    }

    private void OnEnable()
    {
        Composer.OnNoteSpawnInfoReady += SpawnNote;
    }

    private void OnDisable()
    {
        Composer.OnNoteSpawnInfoReady -= SpawnNote;
    }

    private void SpawnNote(string[,] noteInfo)
    {
        if (noteInfo[0, 1] == "Cb-A")
        {
            Instantiate(_note, spawnPosition, spawnRotation);
        }
    }
}
