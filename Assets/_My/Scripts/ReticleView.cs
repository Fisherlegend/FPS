
using UnityEngine;


/// <summary>
/// 圆点控制
/// zhuangwei
/// </summary>
public class ReticleView : MonoBehaviour
{

    /// <summary>
    /// 默认距离
    /// </summary>
    public float m_DefaultDistance = 2f;      
   /// <summary>
   /// 是否平行于表面
   /// </summary>
    public bool m_UseNormal;                  
   /// <summary>
   /// 圆点组件
   /// </summary>
    public  GameObject m_Image;                     
    /// <summary>
    /// 设置圆点的位置变换
    /// </summary>
    public Transform m_ReticleTransform;     
    /// <summary>
    /// 相机的位置
    /// </summary>
    public Transform m_Camera;                
    /// <summary>
    /// 圆点的起始大小
    /// </summary>
    private Vector3 m_OriginalScale;
    /// <summary>
    /// 起始角度
    /// </summary>
    private Quaternion m_OriginalRotation;

    public bool UseNormal
    {
        get { return m_UseNormal; }
        set { m_UseNormal = value; }
    }

    public Transform ReticleTransform
    {
        get { return m_ReticleTransform; }
    }

    private void Awake()
    {       
        m_OriginalScale = m_ReticleTransform.localScale;
        m_OriginalRotation = m_ReticleTransform.localRotation;
    }

    /// <summary>
    /// 圆点隐藏
    /// </summary>
    public void Hide()
    {
        m_Image.SetActive(false);
    }

    /// <summary>
    /// 圆点显示
    /// </summary>
    public void Show()
    {
        m_Image.SetActive(true);
    }

    /// <summary>
    /// 射线为检测到对象设置位置
    /// </summary>
    public void SetPosition()
    {
        // 设置为摄像机前面的默认距离
        m_ReticleTransform.position = m_Camera.position + m_Camera.forward * m_DefaultDistance*5;

        // 根据摄像机的原始距离和距离设置大小
        m_ReticleTransform.localScale = m_OriginalScale * m_DefaultDistance/2;

        // 旋转默认
        m_ReticleTransform.localRotation = m_OriginalRotation;
    }

    /// <summary>
    /// 射线检测到到对象设置位置
    /// </summary>
    /// <param name="hit"></param>
    public void SetPosition(RaycastHit hit)
    {
        m_ReticleTransform.position = hit.point + new Vector3(0, 0.1f, 0.0f);
       
        m_ReticleTransform.localScale = m_OriginalScale * hit.distance/10;

        // 被击中是否使用法线
        if (m_UseNormal)
            // 根据正向法线方向的方向来设置它的角度
            m_ReticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        else
            // 设置默认角度
            m_ReticleTransform.localRotation = m_OriginalRotation;
    }
}

