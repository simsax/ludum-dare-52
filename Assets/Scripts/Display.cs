using TMPro;
using UnityEngine;

public class Display : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI display;

    Game game;
    Player player;


    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        player = game.playerScript;
    }

    void Update()
    {
        display.SetText("Current: {0}\n\nBest: {1}",
            player.hays, player.bestHays);
    }
}
