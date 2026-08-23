using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct RandomWalkingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((
            RefRW<RandomWalking> randomWalking, 
            RefRW<UnitMover> unitMover, 
            RefRO<LocalTransform> localtransform) 
            in SystemAPI.Query<
                RefRW<RandomWalking>, 
                RefRW<UnitMover>,
                RefRO<LocalTransform>>())
        {
            if (math.distancesq(localtransform.ValueRO.Position, randomWalking.ValueRO.targetPosition) <= UnitMoverSystem.REACHED_TARGET_POSITION_SQ)
            {
                // reached the target distance
                Random random = randomWalking.ValueRO.random;
                float3 randomDirection = new float3(random.NextFloat(-1f,+1f), 0 , random.NextFloat(-1f, +1f));

                randomWalking.ValueRW.targetPosition =
                    randomWalking.ValueRO.originPosition +
                    randomDirection * random.NextFloat(randomWalking.ValueRO.distanceMin, randomWalking.ValueRO.distanceMax);

                randomWalking.ValueRW.random = random;
            }
            else
            {
                // too far, move closer
                unitMover.ValueRW.targetPosition = randomWalking.ValueRW.targetPosition;
            }
        }
    }
}
