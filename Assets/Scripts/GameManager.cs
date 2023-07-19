using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    // Start is called before the first frame update

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
