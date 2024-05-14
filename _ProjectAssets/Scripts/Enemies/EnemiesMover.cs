using Narratore.AI;
using Narratore.Enums;
using Narratore.Extensions;
using Narratore.Solutions.Battle;
using UnityEngine;
using Narratore;


public class EnemiesMover : IBeginnedUpdatable
{
    public EnemiesMover(IEntitiesAspects<MovableBot> bots, SeekSteering seek, IPlayerUnitRoot playerUnit)
    {
        _bots = bots;
        _rootCharacter = playerUnit.Root;
        _seek = seek;
    }


    private readonly IEntitiesAspects<MovableBot> _bots;
    private readonly Transform _rootCharacter;
    private readonly SeekSteering _seek;
    

    public void Tick()
    {
        Vector2 targetPoint = _rootCharacter.position.To2D(TwoAxis.XZ);
        foreach (var pair in _bots.All)
        {
            MovableBot bot = pair.Value;
            if (bot.IsStun) continue;

            Vector2 position = bot.Root.position.To2D(TwoAxis.XZ);
            Vector2 targetForward = bot.Root.forward.To2D(TwoAxis.XZ);
            Vector2 seek = _seek.Get(position, targetPoint, targetForward);

            targetForward += seek;
            targetForward = targetForward.normalized;

            float dot = Vector2.Dot(targetForward, (targetPoint - position).normalized);
            float axceleration = Mathf.Lerp(bot.AxselerationRange.x, bot.AxselerationRange.y, dot.Normalized(0.95f, 1)) * Time.deltaTime;

            bot.Speed.ApplyDelta(axceleration);

            float rotateSpeed = Mathf.Lerp(bot.MaxRotateSpeed, bot.MinRotateSpeed, bot.Speed.Relation) * Time.deltaTime;
            Vector3 forward = Vector3.RotateTowards(bot.Root.forward, targetForward.To3D(TwoAxis.XZ), rotateSpeed, 0);
            float distance = (targetPoint - position).magnitude;
            float step = Mathf.Clamp(bot.Speed.Current * Time.deltaTime, 0f, distance);

           
            bot.Root.forward = forward;
            bot.Root.position += forward * step;
        }
    }
}