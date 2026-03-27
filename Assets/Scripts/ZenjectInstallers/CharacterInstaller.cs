using UnityEngine;
using Zenject;

public class CharacterInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<CharacterCore>()
            .FromInstance(GetComponent<CharacterCore>())
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<AIInput>()
            .FromInstance(GetComponent<AIInput>())
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<Animator>()
            .FromInstance(GetComponent<Animator>())
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<CollisionDetector>()
            .FromInstance(GetComponent<CollisionDetector>())
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<CameraTargetTag>()
            .FromInstance(GetComponentInChildren<CameraTargetTag>())
            .AsSingle()
            .NonLazy();
    }
}