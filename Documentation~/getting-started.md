# Quickstart Guide
1. From the Assets/Create menu, navigate to Assets > Create > BardicBytes > EventVars for all included EventVar types like BoolEventVar and StringEventVar and create a new EventVar asset. Import the TypedEventVars sample for even more options.
2. Reference this EventVar asset from a script or UnityEvent such as on a Unity UI Button component.
3. Add listeners via script or use an EventVarListener component to react to the event being raised.

[Overview](overview.md)

[Samples](samples.md)

## Use Cases
### EventVars as configuration assets
- Use the initial value field on EventVars to configure the EventVar in compile/build time.
- Raising the event is optional. Access BaseTypedEventVar.Value instead

### EventVars as change events and current value repository
- create an event var asset to represent the value itself

### EventAsset (event var without a value) 
- can be used as a pure event without passing any data

## EventVarInstancer
[EventVarInstancer code documentation](classes/EventVarInstancer.md)

Add the EventVarInstancer component to any game objects.
![a screenshot of the EventVarInstancer component in the Unity inspector window](images/EventVarInstancer-Inspector.png)

For every EventVar that this GameObject will maintain a personal copy of, add an entry to the EventVarInstances list.

When the object wakes up, the instancer component will do two things.
1. clone each EventVar in the list
2. set the "initial value" of the clone to the relative value in the inspector of the EventVarInstancer.

The asset referenced still exists and maintains it's own values and events.

## EventVar Fields
[EventVar Field code documentation](classes/EventVarInstancer.md)
The Field class provides an alternative to choosing between regular inspector-configured fields and referencing an EventVar. By providing both an optional reference to an EventVar and a fallball value field, the decision to use an event var instead of a standard type can be left to designers.

```
    [SerializeField] private FloatEventVar.Field _moveSpeed;
```

In this example, _moveSpeed can be used like a standard float thanks to the Field type's implicit conversion operator. Meanwhile, in Unity's inspector, this Field could be configured to use a simple float value or the optional FloatEventVar.