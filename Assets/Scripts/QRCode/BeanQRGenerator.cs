using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class BeanQRGenerator : MonoBehaviour
{
    [SerializeField] private Transform qrPreview;

    [SnappingPoint(false, false)]
    public SnappingPoint sp;

    private QRGenerator qrGenerator;
    public QRGenerator QRGenerator { get { return qrGenerator; } }

    private void Update()
    {
        if (Application.isPlaying)
        {
            gameObject.SetActive(false);
            return;
        }

        sp.Position.x = this.transform.position.x;
        sp.Position.y = this.transform.position.y;
        sp.Position.z = this.transform.position.z;
        sp.Rotation = this.transform.rotation.eulerAngles.y;

    }

    public void InitGenerator()
    {
        qrGenerator = QRGenerator.CreateInstance(sp);
        QRGenerator.GenerateQR();
    }
}
