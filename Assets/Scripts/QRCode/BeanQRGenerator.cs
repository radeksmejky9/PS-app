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
        sp.Position.X = qrPreview.position.x;
        sp.Position.Y = qrPreview.position.y;
        sp.Position.Z = qrPreview.position.z;

        sp.Rotation.X = this.transform.rotation.eulerAngles.x;
        sp.Rotation.Y = this.transform.rotation.eulerAngles.y;
        sp.Rotation.Z = this.transform.rotation.eulerAngles.z;
    }

    public void InitGenerator()
    {
        qrGenerator = QRGenerator.CreateInstance(sp);
        QRGenerator.GenerateQR();
    }
}
