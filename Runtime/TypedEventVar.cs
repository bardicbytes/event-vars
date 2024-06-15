// alex@bardicbytes.com

namespace BardicBytes.EventVars
{
    /// <summary>
    /// This "Self-Evaluating" type is a convenience implementation for EventVars that evaluate to the same type.
    /// </summary>
    /// <typeparam name="TInput">The Type that the EventVar stores and shares when raised.</typeparam>
    public abstract class TypedEventVar<TInput> :
        BaseTypedEventVar<TInput, TInput, TypedEventVar<TInput>>,
        IEventVar, IEventVarInput<TInput>, IEventVarOutput<TInput>
    { 
        public override TInput Evaluate(TInput value) => value;
    }
}