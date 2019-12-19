using UnityEngine;



public class HeadBobber : MonoBehaviour
{

    private float timer = 0.0f;
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;
    public float midpoint = 2.0f;
    Vector3 targetPos;

    public bool smooth = true;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3F;

    void Update()
    {

        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;

            if (timer > Mathf.PI * 2f)
            {
                timer = timer - (Mathf.PI * 2f);
            }
        }

        if (waveslice != 0f)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

            totalAxes = Mathf.Clamp(totalAxes, 0f, 1.0f);
            translateChange = totalAxes * translateChange;
            targetPos.Set(transform.localPosition.x, midpoint + translateChange, transform.localPosition.z);
        }
        else
        {
            targetPos.Set(transform.localPosition.x, midpoint, transform.localPosition.z);
        }

        if (smooth)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPos, ref velocity, smoothTime);
        }
        else
        {
            transform.localPosition = targetPos;
        }
    }
}