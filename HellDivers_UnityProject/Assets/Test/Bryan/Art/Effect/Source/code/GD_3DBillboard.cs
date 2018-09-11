using UnityEngine;

// http://gamesonytablet.blogspot.tw/2013/07/unity-tips-40.html

public class GD_3DBillboard : MonoBehaviour
{
    // ゲームオブジェクトをビルボード化させる対象のカメラ
    public Camera Camera = null;
    private Camera m_Camera = null;
    // true の場合ゲームオブジェクトはクリッピング平面のカメラの前に配置されます。
    public bool PositionInFrontOfCamera;
    // PositionInFrontOfCamera が true の場合のオブジェクトのオフセット幅
    public float Offset = 0.001f;

    private Transform thisTrans = null;
    private Transform cameraTrans = null;

    void Awake()
    {
        thisTrans = this.transform;

        // カメラが指定されてない場合はメインカメラを使用
        if (Camera == null)
        {
            Camera = m_Camera = Camera.main;
            cameraTrans = Camera.transform;
        }
    }

    void Update()
    {
        if (Camera != m_Camera)
        {
            m_Camera = Camera;
            cameraTrans = Camera.transform;
        }

        // カメラの forward ベクトルを取得して正規化
        var vec = cameraTrans.forward;
        vec.Normalize();

        // ゲームオブジェクトのポジションをカメラのクリッピング平面のすぐ内側にセットしてカメラビューをブロックするようにする
        if (this.PositionInFrontOfCamera) thisTrans.position = cameraTrans.position + (vec * (Camera.nearClipPlane + this.Offset));

        // ゲームオブジェクトの向きがカメラの方へ向くようにする
        thisTrans.LookAt(cameraTrans.position + cameraTrans.rotation * Vector3.forward, cameraTrans.rotation * Vector3.up);
    }
}