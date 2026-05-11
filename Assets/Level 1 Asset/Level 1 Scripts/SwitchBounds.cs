using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SwitchConfinerShape;
    }
    //�л���������ĵ���
    //private void Start()
    //{
    //    SwitchConfinerShape();
    //}
    
    public void SwitchConfinerShape()
    {
        //FindGameObjectWithTag��FindGameObjectsWithTag������
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D = confinerShape;
        //ÿһ���л���ʱ��Ҫ���ã�����InvalidatePathCache()������֪ͨCinemachineConfiner�߽���״�Ѹ��£���Ҫ���¼���·������
        confiner.InvalidatePathCache();
    }
}
