using UnityEngine;

public class BulletHitController : MonoBehaviour {

    private RaycastHit hit;
    public HitLayerData[] hitLayersData;


    public void handleHit(Ray ray, float distance, float damage ) {

        if (Physics.Raycast(ray, out hit, distance)) {
            for (int i = 0; i < hitLayersData.Length; i++) {
                if(hitLayersData[i].layerMask == (hitLayersData[i].layerMask | (1 << hit.transform.gameObject.layer))) {
                    hitLayersData[i].hitEvent(hit, damage);
                }
            }
        }
    }

    [System.Serializable]
    public struct HitLayerData {

        public string layerName;
        public LayerMask layerMask;
        public GameObject bulletImpactPrefab;

        public void hitEvent(RaycastHit hit, float damage) {
            // We need a variable to hold the position of the prefab
            // The point of contact with the model is given by the hit.point
            Vector3 bulletHolePosition = hit.point + hit.normal * 0.01f;

            // We need a variable to hold the rotation of the prefab
            // The new rotation will be a match between the quad vector forward axis and the hit normal
            Quaternion bulletHoleRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            GameObject hole = Instantiate(bulletImpactPrefab, bulletHolePosition, bulletHoleRotation);
            hole.transform.parent = hit.transform;

            CharacterHealth ch = hit.transform.gameObject.GetComponentInParent<CharacterHealth>();

            if (ch != null && !ch.gameObject.CompareTag("Player")) {
                ch.hurt((int)damage);
            }
        }
    }
}
