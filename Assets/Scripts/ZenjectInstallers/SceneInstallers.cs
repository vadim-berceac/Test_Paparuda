using UnityEngine;
using Zenject;

public class SceneInstallers : MonoInstaller
{
    [SerializeField] private GameObject sceneCameraPrefab;
    [SerializeField] private PlayerInput playerInput;
    
    public override void InstallBindings()
    {
       Container
           .Bind<SceneCamera>()
           .FromComponentInNewPrefab(sceneCameraPrefab)
           .AsSingle()
           .NonLazy();
       
       Container
           .BindInterfacesAndSelfTo<PlayerInput>()
           .FromScriptableObject(playerInput)
           .AsSingle();
       
       Container
           .Bind<SceneCharacterContainer>()
           .AsSingle()
           .NonLazy();
       
       Container
           .Bind<CharacterSelector>()
           .AsSingle()
           .NonLazy();
    }
}