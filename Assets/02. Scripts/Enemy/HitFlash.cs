using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [Header("Flash")]
    private MeshRenderer _mesh;
    private Color _originColor;
    private float flashTime = 0.15f;
    private Enemy _owner;

    void Awake()
    {
        _mesh = GetComponentInChildren<MeshRenderer>();
        _originColor = _mesh.material.color;
        _owner = GetComponent<Enemy>();
    }

    private void Start()
    {
        _owner.damageFlash += FlashStart;
    }

    public void FlashStart()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        _mesh.material.color = Color.red;
        yield return new WaitForSeconds(flashTime);
        _mesh.material.color = _originColor;
    }
}
