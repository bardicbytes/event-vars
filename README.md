# event-vars
scriptable object based event architecture

[Getting Started](https://github.com/bardicbytes/event-vars/blob/main/Documentation~/getting-started.md)

# EventVars for Unity

## Overview
*EventVars* enables easy-to-use event-driven architecture in applications made with Unity. 

The system leverages Unity's ScriptableObjects and the AssetDatabase to streamline communication between different parts of your game without the need for tightly coupled references. Added benefits include a more modular and maintainable codebase.

## Features
- **Extensibility**: Provides a framework for creating new EventVars for any type of object.
- **GameObject-EventVar Instancing**: Create GameObject-associated clones of EventVars, like for health or speed, 
- **Pre-made Types**: Typed EventVars have been premade for most simple types and common types used in Unity. The EventVarListener Component makes it easy to start using EventVars for many common tasks like UI Button events.

## Requirements
- Unity 2022.3 or newer is required to ensure full compatibility with the package features.

## Installation

The EventVars system can be easily integrated into your Unity project using the Unity Package Manager (UPM) with a Git URL. Follow these steps to add EventVars to your project:

### Using Unity Package Manager

1. **Open your Unity project** and navigate to `Window` > `Package Manager` to open the Package Manager window.

2. Click on the **"+" button** in the top left corner of the Package Manager window and select **"Add package from git URL..."**.

3. **Enter the Git URL** for the EventVars repository and click "Add":

```
https://github.com/bardicbytes/event-vars.git
```
Unity will now download and import the EventVars package into your project.

## Usage
1. **Creating EventVars**: From the menu `Assets/Create/Bardic Bytes/EventVars/...` you can create new EventVar assets.
2. **Listening and Raising Events**: Add the `EventVarListener` script or a subtypes to GameObjects to listen for events, and use the `Raise` method on event variables to trigger actions.
3. **Managing Instances**: Utilize `EventVarInstancer` to manage runtime instances of your event variables, ensuring that state and listeners are correctly managed across gameplay.

## Documentation
For more detailed information on setup, usage, and examples, please refer to the [getting started guide](https://github.com/bardicbytes/event-vars/blob/main/Documentation~/getting-started.md).

## Examples
Visit the `Samples~/Demo` folder for practical examples demonstrating the capabilities and setup of EventVars.

## License
EventVars is licensed under the [CC BY-NC 4.0 License](https://creativecommons.org/licenses/by-nc/4.0/?ref=chooser-v1), permitting non-commercial use with attribution.

## Contact
- Bardic Bytes LLC
- Email: [alex@bardicbytes.com](mailto:alex@bardicbytes.com)
- Website: [https://www.bardicbytes.com](https://www.bardicbytes.com)

## Project Link
[https://github.com/bardicbytes/event-vars](https://github.com/bardicbytes/event-vars)