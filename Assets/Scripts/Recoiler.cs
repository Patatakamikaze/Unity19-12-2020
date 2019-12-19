using UnityEngine;





public class Recoiler : MonoBehaviour
{

    [System.Serializable]
    public struct RecoilData
    {
        public float maxRecoil_x;
        public float maxRecoil_y;

        public float maxTrans_x;
        public float maxTrans_z;

        public float recoilSpeed;

        public RecoilData(float maxRecoil_x, float maxRecoil_y, float maxTrans_x, float maxTrans_z, float recoilSpeed)
        {
            this.maxRecoil_x = maxRecoil_x;
            this.maxRecoil_y = maxRecoil_y;

            this.maxTrans_x = maxTrans_x;
            this.maxTrans_z = maxTrans_z;

            this.recoilSpeed = recoilSpeed;

        }
    }

    public RecoilData recoilData = new RecoilData(-20.0f, -10.0f, 1.0f, -1.0f, 10.0f);
    public float recoil { get; set; }

    private Vector3 smoother;

    void Update()
    {
        if (recoil > 0)
        {
            var maxRecoil = Quaternion.Euler(
                Random.Range(transform.localRotation.x, recoilData.maxRecoil_x),
                Random.Range(transform.localRotation.y, recoilData.maxRecoil_y),
                transform.localRotation.z);

            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, Time.deltaTime * recoilData.recoilSpeed);

            var maxTranslation = new Vector3(
                Random.Range(transform.localPosition.x, recoilData.maxTrans_x),
                transform.localPosition.y,
                Random.Range(transform.localPosition.z, recoilData.maxTrans_z));

            //transform.localPosition = Vector3.SLerp(transform.localPosition, maxTranslation, Time.deltaTime * recoilSpeed);
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, maxTranslation, ref smoother, Time.deltaTime * recoilData.recoilSpeed);

            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0;

            var minRecoil = Quaternion.Euler(
                Random.Range(0, transform.localRotation.x),
                Random.Range(0, transform.localRotation.y),
                transform.localRotation.z);

            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, minRecoil, Time.deltaTime * recoilData.recoilSpeed / 2);

            var minTranslation = new Vector3(
                Random.Range(0, transform.localPosition.x),
                transform.localPosition.y,
                Random.Range(0, transform.localPosition.z));

            //transform.localPosition = Vector3.SLerp(transform.localPosition, minTranslation, Time.deltaTime * recoilSpeed);
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, minTranslation, ref smoother, recoilData.recoilSpeed * Time.deltaTime);
        }
    }
}
