using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    /// <summary>
    /// Le blackboard est partager entre toute les nodes.
    /// Utiliser le pour stocker/lire/ecrire des donner utiliser dans les nodes.
    /// </summary>
    /// 
    [System.Serializable]
    public class Blackboard {
        public Vector3 moveToPosition;
        public GameObject Agent;
    }
}