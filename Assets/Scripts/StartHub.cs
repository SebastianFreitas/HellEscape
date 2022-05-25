
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHub : MonoBehaviour
{
    [SerializeField] PathFloor path;
    [SerializeField] Transform pathStartPos;

    [SerializeField] GameObject ui;

    [SerializeField] GameObject message;

    internal PathFloor currentPath;



    [SerializeField] MissionSelector selector;

    [SerializeField] List<BridgeHandler> bridgePrefab;
    [SerializeField] GameObject bridgeManager;
    internal BridgeHandler currentBridge;
    void Awake()
    {
        ui.SetActive(false);
        transform.root.GetComponent<GameMan>().currentHub = this;
    }

    internal void StartPath()
    {

        if(currentBridge != null)
        {
            foreach (Transform child in bridgeManager.transform) Destroy(child.gameObject);
        }

        var dificulty = 0;
        if (PlayerPrefs.HasKey("PathLevel")) dificulty = PlayerPrefs.GetInt("PathLevel");

        currentBridge = Instantiate(bridgePrefab[Random.Range(0,bridgePrefab.Count)], pathStartPos.position, pathStartPos.rotation, bridgeManager.transform);
        currentBridge.SetDistance(dificulty * 2 + 5);
    }

    internal void Activate()
    {
        StartPath();
        ui.SetActive(true);

        if (transform.root.GetComponent<GameMan>().startedRun)
        {
            foreach (Transform child in ui.transform)
            {
                child.gameObject.SetActive(true);
            }

            GetComponentInChildren<MissionSelector>().EnableSelector();
            transform.root.GetComponent<GameMan>().startedRun = false;
        }
    }

    internal void RefreshHub()
    {
        ui.SetActive(false);
        message.SetActive(true);

        if (currentBridge != null)
        {
            foreach (Transform child in bridgeManager.transform) Destroy(child.gameObject);
        }

    }

    private void OnEnable()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Room"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("BulletEnemy"), LayerMask.NameToLayer("Room"), true);
    }

    private void OnDisable()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Room"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("BulletEnemy"), LayerMask.NameToLayer("Room"), false);
    }
}
