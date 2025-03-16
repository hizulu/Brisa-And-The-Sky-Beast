using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerWalkData
{
    [field: SerializeField][field: Range(0f, 1f)] public float WalkSpeedModif { get; private set; } = 0.5f;
    [field: SerializeField][field: Range(0f, 1f)] public float RunSpeedModif { get; private set; } = 2f;
    [field: SerializeField][field: Range(0f, 1f)] public float CrouchSpeedModif { get; private set; } = 0.1f;
}
