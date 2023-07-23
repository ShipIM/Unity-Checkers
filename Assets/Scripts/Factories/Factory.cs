using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Factory<T>
{
    T Create(Transform root, int type);
}
