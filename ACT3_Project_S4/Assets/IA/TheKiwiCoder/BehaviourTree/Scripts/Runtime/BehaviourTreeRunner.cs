using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class BehaviourTreeRunner : MonoBehaviour {

        // Principal BehaviourTree
        public BehaviourTree tree;

        // Stockage des sous systeme du GameObject
        Context context;

        void Start() {
            context = CreateBehaviourTreeContext();
            tree = tree.Clone();
            tree.Bind(context);
        }

        void Update() {
            if (tree) {
                tree.Update();
            }
        }

        Context CreateBehaviourTreeContext() {
            return Context.CreateFromGameObject(gameObject);
        }

        private void OnDrawGizmosSelected() {
            if (!tree) {
                return;
            }

            BehaviourTree.Traverse(tree.rootNode, (n) => {
                if (n.drawGizmos) {
                    n.OnDrawGizmos();
                }
            });
        }
    }
}