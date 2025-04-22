using UnityEngine;

/*
 * NOMBRE CLASE: EnemyStateSOBase
 * AUTOR: Jone Sainz Egea
 * FECHA: 21/04/2025
 * DESCRIPCIÓN: Clase abstracta que sirve de base para los Scriptable Object que definen el comportamiento de cada enemigo.
 *              Hereda de ScriptableObject.
 *              Contiene la información del enemigo, su Transform, el GameObject del enemigo y el Transform de player.
 *              Las clases que hereden de esta pueden sobreescribir sus métodos y tienen acceso a sus variables.
 * VERSIÓN: 1.0. Script que sirve de molde para los comportamientos de todos los estados de los enemigos.
 */
public abstract class EnemyStateSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;
    protected Transform playerTransform;

    /*
     * Método que se encarga de inicializar el estado, simula el constructor del estado
     * @param1 gameObject - Recibe una referencia al GameObject del enemigo para acceder a todos sus componentes.
     * @param2 enemy - Recibe una referencia al Enemy para poder acceder a sus variables y funcionalidades.
     */
    public virtual void Initialize(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public virtual void DoEnterLogic() { }
    public virtual void DoExitLogic() { ResetValues(); }
    public virtual void DoFrameUpdateLogic() { }
    public virtual void DoPhysicsLogic() { }
    public virtual void ResetValues() { }
}
