using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Jone Sainz Egea
// 27/03/2025

// Interfaz común para todos los objetos golpeables, asegurando intercambiabilidad
public interface IHittableElement
{
    void OnHit();
}

// Interfaz común para objetos que se deben mover al interactuar
public interface IMovableElement
{
    void StartMoving(Vector3 target, float speed);
    bool IsMoving();
}

// Clase abstracta abierta a extensiones, permitiendo nuevos elementos golpeables
public abstract class HittableElement : MonoBehaviour, IHittableElement
{
    public abstract void OnHit();
}
