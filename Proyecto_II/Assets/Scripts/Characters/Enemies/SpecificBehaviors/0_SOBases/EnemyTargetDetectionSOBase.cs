using UnityEngine;

/*
 * NOMBRE CLASE: EnemyTargetDetectionSOBase
 * AUTOR: Jone Sainz Egea
 * FECHA: 22/04/2025
 * DESCRIPCIÓN: Clase abstracta que sirve de base para los Scriptable Object que definen la detección de objetivo de cada enemigo.
 *              Hereda de ScriptableObject.
 *              Contiene la información de Enemy, Player y Beast, y sus respectivos Transform.
 *              Las clases que hereden de esta pueden sobreescribir sus métodos y tienen acceso a sus variables.
 * VERSIÓN: 1.0. Script que sirve de molde para las distintas detecciones de objetivo de los enemigos.
 *          1.1. Sara Yue Madruga Martín - Disminución del tamaño de detección de Player cuando este está en modo sigilo. (17/05/2025).
 */
public abstract class EnemyTargetDetectionSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;

    protected Player player;
    protected Transform playerTransform;

    protected Beast beast;
    protected Transform beastTransform;

    public static bool isCrouching = false;

    /*
     * Método que se encarga de inicializar el script de detección, simula el constructor
     */
    public virtual void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
        transform = enemy.transform;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerTransform = player.transform;

        beast = GameObject.FindGameObjectWithTag("Beast").GetComponent<Beast>();
        beastTransform = beast.transform;

        EventsManager.CallSpecialEvents<bool>("PlayerCrouchState", SetBoolPlayerIsCrouching);
    }

    protected virtual void OnDisable()
    {
        EventsManager.StopCallSpecialEvents<bool>("PlayerCrouchState", SetBoolPlayerIsCrouching);
    }

    public virtual bool LookForTarget() { return false; }

    public virtual Vector3 SetTarget(Transform targetTransform) { return Vector3.zero; }

    private void SetBoolPlayerIsCrouching(bool _isCrouching)
    {
        isCrouching = _isCrouching;
    }
}
