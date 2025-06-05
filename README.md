# General architecture based on MV* patterns

## Core

### Enter Point
Represented by the **GameRunner** class. This class is responsible for creating _GameRoot_, which in turn starts the life cycles of the finite state machine.

# CompositionRoot

## Contents

- [Purpose](#purpose)
- [How to Use](#how-to-use)
- [Source Code](#source-code)

## Purpose

Within the architecture of this project, an approach is used with the `CompositionRoot` composition root,
using `DiContainer`
to build a more explicit order of application component calls, as well as division into contexts with different
life cycles.

The base class `SceneCompositionRoot` is responsible for initializing the sub-container for the scene and registering dependencies using Zenject DiContainer.

This allows:

- Create scene-specific dependencies.
- Manage scene objects' lifecycle.
- Add transparency to the order of application subsystem calls within the scene.

## How to Use

> [!WARNING]
> The `Initialize()` method has a `UniTask` return type, which forces us to use some
> constructs in the calling code!
>
> In case of async wait:
> ```csharp
> await _compositionRoot.Initialize(_container);
> ```
> Or when we don't wait for initialization to complete and just run the method:
> ```csharp
>_compositionRoot.Initialize(_container).Forget();
>```

<details>

<summary>Simple example</summary>

1. Create a class inherited from `SceneCompositionRoot`.
2. Implement the `Initialize` method to set up DiContainer.

```csharp
public class SomeSpecificCompositionRoot : SceneCompositionRoot
{
private DiContainer _sceneContainer;

public override UniTask Initialize(DiContainer diContainer)
{
_sceneContainer = diContainer.CreateSubContainer();

// Example bindings
_sceneContainer.Bind<SomeDependency>().AsSingle();

return default;
}
}
```
3. Use `SceneInitializer` to initialize the scene.

```csharp
public class SomeGameState : IPayloadedState<string>
{
private readonly SceneInitializer _sceneInitializer;

public SomeGameState(SceneInitializer sceneInitializer) 
{ 
_sceneInitializer = sceneInitializer; 
} 

public void OnEnter(string sceneName) 
{ 
_sceneInitializer.Initialize().Forget(); 
} 
} 
```

</details>

<details>

<summary>An example of dependency binding for the GameEvents feature</summary>

```csharp
public class GameplayCompositionRoot : SceneCompositionRoot
{ 
[SerializeField] private GameEventConfig _gameEventConfig; 
[SerializeField] private ChoiceEventView _choiceEventView; 
[SerializeField] private NotifyEventView _notifyEventView; 
[SerializeField] private QuestEventView _questEventView; 

private GameEventService_gameEventService; 
private GameEventFactory _eventFactory; 
private DiContainer_sceneContainer; 

public override UniTask Initialize(DiContainer diContainer) 
{ 
// Create a sub-container for the scene 
_sceneContainer = diContainer.CreateSubContainer(); 

// Inject dependencies 
_sceneContainer.BindInstance(_choiceEventView).AsSingle(); 
_sceneContainer.BindInstance(_notifyEventView).AsSingle(); 
_sceneContainer.BindInstance(_questEventView).AsSingle(); 
_sceneContainer.Bind<ITagDataContainer>().To<TestTagDataContainer>().AsSingle(); 
_sceneContainer.Bind<RequirementFactory>().AsSingle(); 
_sceneContainer.Bind<ImpactFactory>().AsSingle(); 
_sceneContainer.Bind<ChoiceFactory>().AsSingle(); 
_sceneContainer.Bind<StrategyFactory>().AsSingle(); 
_sceneContainer.Bind<GameEventFactory>().AsSingle(); 
_sceneContainer.Bind<GameEventService>().AsSingle(); 
_sceneContainer.Bind<WithChoicesEventPresenter>().AsTransient(); 
_sceneContainer.Bind<NotifyEventPresenter>().AsTransient(); 
_sceneContainer.Bind<QuestEventPresenter>().AsTransient(); 

// Resolve dependencies 
_eventFactory = _sceneContainer.Resolve<GameEventFactory>(); 
_gameEventService = _sceneContainer.Resolve<GameEventService>(); 
_choiceEventView.Construct(_sceneContainer.Resolve<WithChoicesEventPresenter>()); 
_notifyEventView.Construct(_sceneContainer.Resolve<NotifyEventPresenter>()); 
_questEventView.Construct(_sceneContainer.Resolve<QuestEventPresenter>()); 

// In case of asynchronous initialization, return UniTask
return default;
}
}
```

</details>

## Source Code

<details>

<summary>SceneCompositionRoot</summary>

```csharp
public abstract class SceneCompositionRoot : MonoBehaviour
{
public abstract UniTask Initialize(DiContainer diContainer);
}
```

</details>

<details>

<summary>SceneInitializer</summary>

```csharp
public class SceneInitializer
{
private readonly DiContainer _diContainer;
public SceneInitializer(DiContainer