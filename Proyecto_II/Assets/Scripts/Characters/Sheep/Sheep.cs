using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: Sheep
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/05/2025
 * DESCRIPCIÓN: Clase que gestiona la lógica de las ovejas.
 * VERSIÓN: 1.0.
 */

public class Sheep : MonoBehaviour
{
    #region Variables
    private SheepStateMachine sheepStateMachine;
    public SFXSheep SfxSheep { get; private set; }

    public Rigidbody RbSheep { get; private set; }
    public Animator AnimSheep {  get; private set; }

    [SerializeField] public Transform PlayerTransform;
    #endregion

    [field: Header("Gizmos")]
    [SerializeField] private float sheepRange = 2f;
    [SerializeField] private float whistleRange = 15f;

    private void Awake()
    {
        RbSheep = GetComponent<Rigidbody>();
        AnimSheep = GetComponent<Animator>();
        SfxSheep = GetComponent<SFXSheep>();
        sheepStateMachine = new SheepStateMachine(this);
    }

    void Start()
    {
        RandomInitialStateSheep();
    }

    void Update()
    {
        sheepStateMachine.UpdateLogic();
    }

    private void FixedUpdate()
    {
        sheepStateMachine.UpdatePhysics();
    }

    #region Métodos Propios Sheep
    /// <summary>
    /// Método que almacena en una lista los estados posibles con los que pueden empezar las ovejas.
    /// Se utiliza aleatoriedad para que cada oveja, de manera independiente, sigan patrones diferentes.
    /// </summary>
    private void RandomInitialStateSheep()
    {
        List<IState> initialState = new List<IState>()
        {
            sheepStateMachine.SheepIdleState,
            sheepStateMachine.SheepWalkState,
            sheepStateMachine.SheepGrazeState
        };

        int randomState = Random.Range(0, initialState.Count);
        sheepStateMachine.ChangeState(initialState[randomState]);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sheepRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, whistleRange);
    }
}
