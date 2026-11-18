// alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    public abstract class EventVarField<TInput, TOutput, TEventVar>
        : BaseEventVarField where TEventVar
        : BaseTypedEventVar<TInput, TOutput, TEventVar>
    {
        // this enables the implicit conversion of an EventVar's field class to the event var's output type
        public static implicit operator TOutput(EventVarField<TInput, TOutput, TEventVar> f) => f.Evaluate();

        [Tooltip("If no initial source is set, the value of this field is this Fallback Value.")]
        [SerializeField] private TOutput _fallbackValue = default;
        [Tooltip("Source EventVar. During play, an assigned instancer supplies the clone. This field does not create a clone.")]
        [SerializeField] private TEventVar _initialSource = default;

        private TEventVar _currentSource = default;
        private bool _warnedMissingInstance;

        /// <summary>
        /// This EventVar is the current source eventVar used when the field is evaluated.
        /// </summary>
        public TEventVar Source => _currentSource;

        /// <summary>
        /// Gets the relevant value for this field based on the configuartion.
        /// </summary>
        /// <returns>a value of the output type</returns>
        public TOutput Evaluate()
        {
            if (_currentSource == null) _currentSource = _initialSource;

            if (_currentSource == null) return _fallbackValue;

            if (_instancer != null && Application.isPlaying)
            {
                if (_instancer.HasInstance(_currentSource))
                {
                    var instance = _instancer.GetInstance(_currentSource);
                    if (instance != null) return instance.Value;
                }

                if (!_warnedMissingInstance)
                {
                    _warnedMissingInstance = true;
                    Debug.LogWarning($"No instance of {_currentSource.name} on {_instancer.name}. Using the fallback value.", _instancer);
                }
                return _fallbackValue;
            }

            return _currentSource.Value;
        }

        /// <summary>
        /// sets the source override 
        /// </summary>
        /// <param name="newSource">if null, the source will be reset to the initial source whether or not the initial source is null</param>
        public void SetSourceOverride(TEventVar newSource)
        {
            var nextSource = newSource;
            if (nextSource == null) nextSource = _initialSource;
            _currentSource = nextSource;
        }
    }
}