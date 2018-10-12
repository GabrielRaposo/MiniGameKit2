using UnityEngine;

namespace Fevdo{
    public class Fixable: MonoBehaviour{
        [SerializeField] Transform fixables;
        void Reset(){
            Transform fixFolder = transform.Find("Fixables");
            if(!fixFolder){
                fixFolder = new GameObject("Fixables").transform;
                var go = GameObject.Instantiate(fixFolder,transform.root);
                go.name = "Fixables";
            }
            fixables = fixFolder.transform;
        }

        public void Fix(Transform obj){
            obj.parent = fixables;
        }
    }
}
