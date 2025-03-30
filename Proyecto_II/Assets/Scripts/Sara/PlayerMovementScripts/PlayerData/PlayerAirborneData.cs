using System;
using UnityEngine;

[Serializable]
public class PlayerAirborneData
{
    [field: SerializeField][field: Range(0f, 25f)] public float BaseForceJump { get; private set; } = 10f;
    [field: SerializeField][field: Range(0f, 25f)] public float AirSpeed { get; private set; } = 2f;
    [field: SerializeField][field: Range(0f, 25f)] public float AirControl { get; private set; } = 0.5f;
    [field: SerializeField] public PlayerJumpData JumpData { get; private set; }
}
