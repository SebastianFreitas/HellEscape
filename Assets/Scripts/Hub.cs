using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour
{
    public GameObject[] objects;
    public Transform[] propSpawns;
    public Transform statspos;
    public int[] weight;
    public GameObject player;

    [SerializeField] internal MissionSelector missionSelector;

    [SerializeField] StartHub startHub;
    // Start is called before the first frame update
    void Start()
    {
        //SpawnObjects(1);
    }

    private void OnEnable()
    {
       // missionSelector.EnableSelector();
    }



    public void SpawnObjects(int more)
    {
        for (int i = 0; i < propSpawns.Length; i++)
        {
            for (int j = 0; j < Random.Range(1, 6); j++)
            {
                var x = GetRandomWeightedIndex(weight);
                Instantiate(objects[x], new Vector3(propSpawns[i].position.x, propSpawns[i].position.y + Random.Range(2, 6), propSpawns[i].position.z), transform.rotation, transform);
            }
        }
    }

    internal void VoidPlayer()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = player.GetComponent<PlayerBasicMovement>().lastPos.position;
        player.transform.position = statspos.position;
        player.GetComponent<CharacterController>().enabled = true;

    }

    internal void ResetPath()
    {
        if (startHub == null) startHub = GetComponentInChildren<StartHub>();
        startHub.StartPath();
    }
    public int GetRandomWeightedIndex(int[] weights)
    {
        // Get the total sum of all the weights.
        int weightSum = 0;
        for (int i = 0; i < weights.Length; ++i)
        {
            weightSum += weights[i];
        }

        // Step through all the possibilities, one by one, checking to see if each one is selected.
        int index = 0;
        int lastIndex = weights.Length - 1;//elementCount
        while (index < lastIndex)
        {
            // Do a probability check with a likelihood of weights[index] / weightSum.
            if (Random.Range(0, weightSum) < weights[index])
            {
                return index;
            }

            // Remove the last item from the sum of total untested weights and try again.
            weightSum -= weights[index++];
        }

        // No other item was selected, so return very last index.
        return index;
    }
}
