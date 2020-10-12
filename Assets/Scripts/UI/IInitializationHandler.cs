using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializationHandler<T>
{
    void Initialize(T arg1);
}

public interface IInitializationHandler<T1, T2>
{
    void Initialize(T1 arg1, T2 arg2);
}

public interface IInitializationHandler<T1, T2, T3>
{
    void Initialize(T1 arg1, T2 arg2, T3 arg3);
}

public interface IInitializationHandler<T1, T2, T3, T4>
{
    void Initialize(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
}