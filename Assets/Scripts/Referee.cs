using UnityEngine;

public class Referee : MonoBehaviour
{
// move code so this class and object starts the song
// make this the only dontdestroyonload object that recieves events from the difficulty from the ui
// no more ui weird rules. Use the screenshot from the boot scene idea from professor Moore
// Handles special objectives like health, asking objects to display results, etc, and acts as the central game manager
// when recieving messages for stopping a song, it should then stop all the components so they don't waste processing power somehow,
// perhaps it disables them directly... or makes sure everything for active songs with update functions would stop to save processing
//( make that its own method) or vise versa for end-game components like scoring ones
}
