#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SuggestLightmapStaticObjects : MonoBehaviour
{
    [MenuItem("Tools/Suggest Lightmap Static Objects")]
    public static void SuggestStaticCandidates()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int suggestedCount = 0;

        foreach (GameObject obj in allObjects)
        {
            if (!obj.activeInHierarchy) continue;

            // Skip if already static
            if (obj.isStatic) continue;

            // Skip common dynamic tags
            string tag = obj.tag.ToLower();
            if (tag == "player" || tag == "animal" || tag == "prey" || tag == "predator")
                continue;

            
            if (obj.GetComponent<Animator>() || obj.GetComponent<UnityEngine.AI.NavMeshAgent>())
                continue;

           
            bool isCandidate = 
                (obj.GetComponent<MeshRenderer>() || obj.GetComponent<Collider>()) &&
                obj.GetComponent<Rigidbody>() == null;

            if (isCandidate)
            {
                Debug.Log($"🔍 {obj.name} is a good candidate for Lightmap Static", obj);
                suggestedCount++;
            }
        }

        Debug.Log($"✅ Suggested {suggestedCount} potential static objects.");
    }
}
#endif
