using UnityEngine;

public class Task_Wander : AgentTaskBase
{
    [Header ("Move Randomly")]
    private float _radius;
    private Vector3 _targetPosition;
    private bool _isFinished = false;
    private Transform _agentTransform;

    [Header ("Scanner Flower")]
    private float _scanCooldown;
    private float _scanTimer;
    private float _flowerDetectionRadius;


    public Task_Wander(string taskName, Blackboard bb, float radius, float scanCooldown, float scanTimer, float flowerDetectionRadius) : base(taskName, bb)
    {
        _radius = radius;
        _scanCooldown = scanCooldown;
        _scanTimer = scanTimer;
        _flowerDetectionRadius = flowerDetectionRadius;
        _agentTransform = (Transform)bb.GetValue("AgentTransform");
    }

    public override void OnStart()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _radius;
        _targetPosition = _agentTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        _isFinished = false;
    }

    public override void OnUpdate()
    {
        MoveRandomly();
        _scanTimer += Time.deltaTime;
        if (_scanTimer >= _scanCooldown)
        {
            _scanTimer = 0f;
            ScanForFlowers();
        }
    }

    public void MoveRandomly()
    {
        float speed = 2f;
        _agentTransform.position = Vector3.MoveTowards(_agentTransform.position, _targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(_agentTransform.position, _targetPosition) < 0.1f)
        {
            _isFinished = true;
            OnFinish();
            return;
        }
    }

    public void ScanForFlowers()
    {
        Collider[] hits = Physics.OverlapSphere(_agentTransform.position, _flowerDetectionRadius);

        foreach (var hit in hits)
        {
            Flower flower = hit.GetComponent<Flower>();

            if (flower != null && flower.ContainsPollen())
            {
                _bb.ModifyValue("TargetFlower", flower);
                return;
            }
        }
    }

    public override void OnFinish()
    {
        if (_isFinished)
        { 
            _isFinished = false;
            OnStart();
        }
    }

    public override void OnCancel() { }

    public override float GetUtility()
    {
        var target = _bb.GetValue("TargetFlower");
        return target == null ? 1f : 0f;
    }

    public override int GetTaskPriority()
    {
        return 0;
    }

    public override bool IsTaskFinished()
    {
        return _isFinished;
    }
}
