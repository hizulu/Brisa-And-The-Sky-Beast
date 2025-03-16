using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementData
{
    public Vector2 MovementInput { get; set; }
    public float MovementSpeedModifier { get; set; } = 2f;
    public float JumpForceModifier { get; set; } = 5f;
}
