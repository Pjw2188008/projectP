using UnityEngine;

public static class KeyList
{
    public static KeyCode move_left = KeyCode.A;
    public static KeyCode move_right = KeyCode.D;

    public static KeyCode run = KeyCode.LeftShift;

    public static KeyCode crouch = KeyCode.K; // crouch/slid

    public static KeyCode dash = KeyCode.L;

    public static KeyCode jump = KeyCode.Space;

    // Battle
    public static KeyCode attack = KeyCode.J;
    public static KeyCode guard = KeyCode.K;

    // Event
    public static KeyCode communication = KeyCode.P;
    public static KeyCode com_select_up = KeyCode.UpArrow;
    public static KeyCode com_select_down = KeyCode.DownArrow;
}
