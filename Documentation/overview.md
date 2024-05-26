# EventVar
An exentsible scriptable object that exposes a Unity Event and possibly a value.

# EventVarInstancer
A monobehaviour that allows instances of EventVars with values to be assocaited with the game object, using the original asset's GUID as the key.

# EventVarField
A type to be used as an inspector field for a given EventVar that facilitates scripts that may use the value in the referenced EventVar or a serialized value on the object itself if no EventVar asset is referenced. An implicit conversion allows seemless use of EventVarFields instead of types like float and bool for character configurations.

# EventVarListener
A component that attaches a UnityEvent to the raising of an EventVar.