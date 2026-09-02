using Unity.Entities;
using UnityEngine;

public class HeathBarAuthoring : MonoBehaviour
{
    public GameObject barVisualGameObject;
    public GameObject healthGameObject;
    public class Baker : Baker<HeathBarAuthoring>
    {
        public override void Bake(HeathBarAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new HealthBar
            {
                barVisualEntity = GetEntity(authoring.barVisualGameObject, TransformUsageFlags.NonUniformScale),
                healthEntity = GetEntity(authoring.healthGameObject, TransformUsageFlags.Dynamic),
            });
        }
    }
}

public struct HealthBar : IComponentData
{
    public Entity barVisualEntity;
    public Entity healthEntity;

}
