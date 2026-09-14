using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICuttable
{
    public bool TryCut(Vector3 position, Vector3 normal);
}
