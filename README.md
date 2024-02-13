# event-vars
scriptable object based event architecture

[Getting Started](https://github.com/bardicbytes/event-vars/blob/main/Documentation~/getting-started.md)

# EventVars for Unity

## Overview
EventVars is a comprehensive system designed for Unity that leverages ScriptableObjects to create a robust and flexible event management system. Created by Bardic Bytes LLC, it streamlines the communication between different parts of your game without the need for tightly coupled references, facilitating a more modular and maintainable codebase.

## Features
- **Unique Identifiers**: Utilizes GUIDs as unique identifiers for each event variable, ensuring reliable reference across your projects.
- **Unity Editor Support**: Includes custom editor utilities to streamline the management and debugging of event variables.
- **Dynamic Event Listening**: Facilitates the dynamic addition and removal of event listeners at runtime, enhancing flexibility.
- **Extensibility**: Provides a framework for creating typed event variables to accommodate various data types and structures.
- **Instance Management**: Through the `EventVarInstancer` component, manage instances of event variables at runtime, ensuring correct event handling and state management across scenes and prefabs.

## Requirements
- Unity 2022.3.0b5 or newer is required to ensure full compatibility with the package features.

## Installation
1. Clone the EventVars repository into your Unity project's `Assets` folder:

```
git clone https://github.com/bardicbytes/event-vars.git
```

2. The EventVars system is now ready to be used within Unity. Navigate to the cloned directory to get started.

## Usage
1. **Creating Event Variables**: Use `Assets/Create/Bardic Bytes/EventVar without Data` to create new event variables.
2. **Listening and Raising Events**: Add the `EventVarListener` script to GameObjects to listen for events, and use the `Raise` method on event variables to trigger actions.
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