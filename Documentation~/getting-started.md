# Quickstart Guide
1. From the Assets/Create menu, navigate to Assets/Create/Bardic Bytes/EventVars/ for all included EventVar types like BoolEventVar and StringEventVar and create a new EventVar asset.
2. Reference this EventVar asset from a script or UnityEvent such as on a Unity UI Button component.
3. Add listeners via script or use an EventVarListener component to react to the event being raised.


[Overview](overview.md)

[Samples](samples.md)

## Duplicating EventVar assets

The EventVar system caches the GUID Unity's asset database. If you duplicate an EventVar, the new asset will have the same GUID cached. There is a couple ways to fix this easy and automatically, but my laptop is running out of power and I need to save. good luck!
