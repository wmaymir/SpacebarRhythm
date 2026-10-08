using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Judge : MonoBehaviour
{
    //validates player's actions, whether thet are sucessful or failing (or more detailed with windows)
    //then waits for an event from input and evaluates like so

    /*
     * this is chronoligcally or just logically incorrect but the ideas are good
     * 
     * gets the next button and beat position from the composer called currentgoal
     * if its wrong it "fails"
     * 
     * 
     * if player input is wrong button it fails 
     * else
     * gets active beat from metronome to judge
     * if it's on an active beat
     * 
     * if the window is open at a different beat the goal is failed
     * 
     * 
     * must be button specifiec by the goal happening on the same beat
     * 
     * for misses( all types if you think about it), it checks for the window closed exit beat
     *
     *
     *JUDGE ONLY REPLIES WITH SUCESS OR FAIL, teh referee does scoring
     */
}
