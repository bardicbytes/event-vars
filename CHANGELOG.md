
# Change Log
All notable changes to this project will be documented in this file.

## [0.3.0] - 2024-06-11
This change is a a significant refactor with great improvements for usability, readability and maintainability

- The EventVar.cs has been broken out into separate files with new names.
    - BaseTypedEventVar is responsible for the logic of the typed event and implements both IEventVarInput and IEventVarOutput
    - TypedEventVar is now the most common generic EventVar type to inherit from in cases where there's one type, such as in FloatEventVar
    - The base class of the event var inheritence tree is now named EventAsset to better represent the lack of variable storage
    - The abstract class BaseEventVar has been added to be the concrete type for referencing typed event var assets
    - The partial keyword has been added to BaseTypedEventVar, and the nested class Field has been moved to BaseTypedEventVar.Field.cs
- EventAsset, formerly EventVar, has all references to the stored values have been removed
- More interfaces have been introduced such as IEventVar and IEventVarOutput
- private fields have been renamed to include a prefix underscore
- additional comments and documentation has been added along the way
- Samples: added "Big Demo" and renamed "demo" to "Small Demos"

### Issues
- BaseTypedEventVar is still overriding the add/remove listener methods from EventAsset, but these methods shouldn't be used in BaseTypedEventVar.
- EventVar members CloneSource, InstanceOwner, and IsActorInstance feel out of place. should move them to a instancer specific type

## [0.2.8] - 2024-06-11
- Added "initilizer" component and some typed subclasses for gameObject and Transform
- Moved Typed event var and listener scripts from samples to the main package
- added much more documentation and comments across the board

## [0.2.7] - 2024-06-05
- Fixed design/implementation issue with the event var fields relationship to the instancer

## [0.2.6] - 2024-06-03
- EventVar powered SFX system included in samples
- more EventVarTypes added to samples

## [0.2.5] - 2024-05-26
- RigidbodyEventVar added to samples
- EventVarCollection now has more import/export functions

## [0.2.4] - 2024-05-26
- bug fixes
- comments and documentation
- simplified and fixed instancer functionality and custom editor
- reorganization of files

## [0.2.3] - 2024-02-17
- bug fixes

## [0.2.2] - 2024-02-16
- fix EventVar custom editor
- new colored asset icons for the scripts
- completely removed MinMaxEventVar from inheritance tree

## [0.2.1] - 2024-02-15
### Bug Fixes
- fixed issue with instancer supporting functionality

### Added
- documentation
- code comments

## [0.2.0] - 2024-02-10

### Improvements
- Code Cleanup
- Reduced number of files
- Pruned inheritence tree

### Added
- Editor improvements for EventVars and EventVarListeners
- Initial documentation
- Demo in Samples~ folder

## [Unreleased] - 2023-11-29
 
### Added
- Initial Commit