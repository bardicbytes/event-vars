# EventVars for Unity
last updated June 7, 2024
## Overview
**EventVars** is a package for Unity. This package enables a robust asset based event-driven architecture.

The system leverages Unity's **ScriptableObjects** and the **AssetDatabase** to create a loosely couples event based messaging system. This streamliens communication between different systems in your Unity application in a way that is resilient in the face of change.

### Features
- **Extensibility**: Provides a framework for creating new EventVars for any type of object. Lots of examples!
- **GameObject-EventVar Instancing**: Create GameObject-associated clones of EventVars, like for health or speed. 
- **Pre-made Types**: Comes with lots of subclass types out of the box like float, bool, Vector3, string, and more.

### Requirements
- Unity 2022.3 or newer is required to ensure full compatibility with the package features.

## Installation Using Unity Package Manager
The EventVars system can be easily integrated into your Unity project using the Unity Package Manager (UPM) with a Git URL. Follow these steps to add EventVars to your project:

[Install a UPM package from a Git URL on Unity.com](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

1. **Open your Unity project** and navigate to `Window` > `Package Manager` to open the Package Manager window.
2. Click on the **"+" button** in the top left corner of the Package Manager window and select **"Add package from git URL..."**.
3. **Enter the Git URL** for the EventVars repository and click "Add":
```
https://github.com/bardicbytes/event-vars.git
```
Unity will now download and import the EventVars package into your project.

## Usage
1. **Creating EventVars**: From the menu `Assets/Create/Bardic Bytes/EventVars/...` you can create new EventVar assets.
2. **Listening and Raising Events**: Add the `EventVarListener` component or subscribe to the events in your own script.
3. **Managing Instances**: Utilize `EventVarInstancer` component to manage per-GameObject instances of your event variables.
4. **EventVar Fields**: These can be used as inspector-exposed fields to create extremely veritile behaviour components.

## Documentation
For more detailed information on setup, usage, and examples, please refer to the [getting started guide](https://github.com/bardicbytes/event-vars/blob/main/Documentation~/getting-started.md).

## Samples
Optional samples included with this package include:
- AudioEventVars - This sample extends EventVars and adds advanced support for SoundEffect playback
- TypedEventVars - This sample includes more ready to use EventVars such as RigidBodyEventVar.
- Demo - A collection of scripts and scenes demonstrating EventVars in use.

After importing the package, the samples can be accessed in the UPM window by selecting Samples tab.

## License
EventVars is licensed under the [CC BY-NC 4.0 License](https://creativecommons.org/licenses/by-nc/4.0/?ref=chooser-v1), permitting non-commercial use with attribution.
Commercial licenses available.

## Contact
- Bardic Bytes LLC
- Email: [alex@bardicbytes.com](mailto:alex@bardicbytes.com)
- Website: [https://www.bardicbytes.com](https://www.bardicbytes.com)

## Project Link
[https://github.com/bardicbytes/event-vars](https://github.com/bardicbytes/event-vars)
