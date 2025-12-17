using System;
using System.Collections.Generic;
using UnityEngine;

public static class CellCullingEvents
{
    public static Action<Renderer> AddCell;
    public static Action<Renderer> RemoveCell;
    public static Action CameraMoved;
}



public class CellCullingManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private readonly List<Renderer> _visible = new();
    private readonly List<Renderer> _hidden = new();

    private Plane[] _planes;
    private bool _dirty;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    private void OnEnable()
    {
        CellCullingEvents.AddCell += Register;
        CellCullingEvents.RemoveCell += Unregister;
        CellCullingEvents.CameraMoved += MarkDirty;
    }

    private void OnDisable()
    {
        CellCullingEvents.AddCell -= Register;
        CellCullingEvents.RemoveCell -= Unregister;
        CellCullingEvents.CameraMoved -= MarkDirty;
    }

    private void MarkDirty()
    {
        _dirty = true;
    }

    public void Register(Renderer r)
    {
        if (r == null)
            return;

        EnsurePlanes();

        bool isVisible = GeometryUtility.TestPlanesAABB(_planes, r.bounds);
        r.enabled = isVisible;

        if (isVisible)
            _visible.Add(r);
        else
            _hidden.Add(r);
    }

    public void Unregister(Renderer r)
    {
        if (r == null)
            return;

        _visible.Remove(r);
        _hidden.Remove(r);
    }

    private void LateUpdate()
    {
        if (!_dirty)
            return;

        EnsurePlanes();

        UpdateList(_visible, expectedVisible: true);
        UpdateList(_hidden, expectedVisible: false);

        _dirty = false;
    }

    private void EnsurePlanes()
    {
        if (_planes == null)
            _planes = new Plane[6];

        _planes = GeometryUtility.CalculateFrustumPlanes(_camera);
    }

    private void UpdateList(List<Renderer> list, bool expectedVisible)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            Renderer r = list[i];

            if (r == null)
            {
                list.RemoveAt(i);
                continue;
            }

            bool isVisible = GeometryUtility.TestPlanesAABB(_planes, r.bounds);

            if (isVisible == expectedVisible)
                continue;

            r.enabled = isVisible;

            list.RemoveAt(i);
            if (isVisible)
                _visible.Add(r);
            else
                _hidden.Add(r);
        }
    }
}
