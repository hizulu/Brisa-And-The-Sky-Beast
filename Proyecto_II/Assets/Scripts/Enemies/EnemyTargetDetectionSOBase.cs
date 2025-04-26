using UnityEngine;

/*
 * NOMBRE CLASE: EnemyTargetDetectionSOBase
 * AUTOR: Jone Sainz Egea
 * FECHA: 22/04/2025
 * DESCRIPCI�N: Clase abstracta que sirve de base para los Scriptable Object que definen la detecci�n de objetivo de cada enemigo.
 *              Hereda de ScriptableObject.
 *              Contiene la informaci�n de Enemy, Player y Beast, y sus respectivos Transform.
 *              Las clases que hereden de esta pueden sobreescribir sus m�todos y tienen acceso a sus variables.
 * VERSI�N: 1.0. Script que sirve de molde para las distintas detecciones de objetivo de los enemigos.
 */
public abstract class EnemyTargetDetectionSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;

    protected Player player;
    protected Transform playerTransform;

    protected Beast beast;
    protected Transform beastTransform;

    /*
     * M�todo que se encarga de inicializar el script de detecci�n, simula el constructor
     */
    public virtual void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
        transform = enemy.transform;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerTransform = player.transform;

        beast = GameObject.FindGameObjectWithTag("Beast").GetComponent<Beast>();
        beastTransform = beast.transform;

    }

    public virtual bool LookForTarget() { return false; }

    public virtual Vector3 SetTarget(Transform targetTransform) { return Vector3.zero; }
}
