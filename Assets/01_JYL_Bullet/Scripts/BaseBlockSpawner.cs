using UnityEngine;

namespace JYL
{
    public class BaseBlockSpawner : MonoBehaviour
    {
        [SerializeField] GameObject BaseBrickSet;
        [SerializeField] Transform beforeBlock;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Destroy(beforeBlock.gameObject);
                beforeBlock = Instantiate(BaseBrickSet, beforeBlock.position, beforeBlock.rotation).transform;
            }
        }
    }
}
