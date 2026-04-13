using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObjectLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask grassLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask fovLayer;

    public static GameLayers i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    public LayerMask SolidObjectLayer { get => solidObjectLayer; }
    public LayerMask InteractableLayer { get => interactableLayer; }
    public LayerMask GrassLayer { get => grassLayer; }
    public LayerMask PlayerLayer { get => playerLayer; }
    public LayerMask FovLayer { get => fovLayer; }

}
