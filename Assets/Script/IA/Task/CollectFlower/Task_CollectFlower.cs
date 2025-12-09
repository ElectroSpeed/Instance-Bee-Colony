using UnityEngine;

public class Task_CollectFlower : AgentTaskBase
{
    private Flower _flower;
    private bool _isFinished = false;
    private int _pollenCollected = 0;

    public Task_CollectFlower(string taskName, Blackboard bb, int pollenCollected): base(taskName, bb)
    {
        _pollenCollected = pollenCollected;
    }

    public override void OnStart()
    {
        _flower = (Flower)_bb.GetValue("TargetFlower");

        if (_flower == null)
        {
            _isFinished = true;
            return;
        }

        _pollenCollected = _flower.GetPollen();

        _bb.ModifyValue("CollectedPollen", _pollenCollected);

        _bb.ModifyValue("TargetFlower", null);

        _isFinished = true;
    }

    public override void OnUpdate()
    {

    }

    public override void OnFinish() { }

    public override void OnCancel() { }

    public override float GetUtility()
    {
        Flower flower = (Flower)_bb.GetValue("TargetFlower");
        if (flower == null) return 0f;

        Transform agent = (Transform)_bb.GetValue("AgentTransform");
        float dist = Vector3.Distance(agent.position, flower.transform.position);

        float cond_CloseEnough = dist < 0.5f ? 1f : 0f;
        float cond_FlowerHasPollen = flower.ContainsPollen() ? 1f : 0f;
        float cond_InventoryEmpty = ((int)_bb.GetValue("CollectedPollen") == 0) ? 1f : 0f;

        Hunger h = GetHunger();
        Tiredness t = GetTiredness();

        float cond_NotHungry = Normalize(h._currentHungerValue, h._minHungerValue, h._maxHungerValue);
        float cond_NotTired = Normalize(t._currentTirednessValue, t._minTirednessValue, t._maxTirednessValue);

        return Combine(cond_CloseEnough, cond_FlowerHasPollen, cond_InventoryEmpty, cond_NotHungry, cond_NotTired);

    }

    public override int GetTaskPriority() => 2;

    public override bool IsTaskFinished() => _isFinished;
}
