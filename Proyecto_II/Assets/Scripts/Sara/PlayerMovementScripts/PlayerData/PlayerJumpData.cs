using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerJumpData
{
    [field: SerializeField][field: Range(0f, 1f)] public float NormalJumpModif { get; private set; } = 0.2f;
    [field: SerializeField][field: Range(0f, 1f)] public float DoubleJumpModif { get; private set; } = 0.5f;
}
