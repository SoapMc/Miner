using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VariableReference<T> : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private T _initialValue = default;
    [System.NonSerialized] private T _runtimeValue;

    public delegate void ValueChange(T oldValue, T newValue);
    public event ValueChange ValueChanged = delegate { };

    public T Value
    {
        get { return _runtimeValue; }
        set
        {
            if (!EqualityComparer<T>.Default.Equals(_runtimeValue, value))
            {
                T previousValue = _runtimeValue;
                _runtimeValue = value;
                ValueChanged.Invoke(previousValue, _runtimeValue);  //runtime value means actual value
            }
        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        _runtimeValue = _initialValue;
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {

    }

    public void SetValueWithoutNotification(T value)
    {
        _runtimeValue = value;
    }

    public static implicit operator T(VariableReference<T> variableRef) => variableRef.Value;
}