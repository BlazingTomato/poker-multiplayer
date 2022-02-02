using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{

    public GameObject[] spawnPoints;
    private void Start() {
        GameObject parent = GameObject.FindGameObjectWithTag("CommunityCards");
        spawnPoints = GameObject.FindGameObjectsWithTag("CardPoints");

        this.transform.SetParent(parent.transform, false);

    }

}
