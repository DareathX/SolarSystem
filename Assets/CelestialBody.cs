using System;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    private Matrix4x4 celestialBody = Matrix4x4.identity;

    [field: SerializeField]
    public float DayCycle { get; set; }
    [field: SerializeField]
    public float MoonDistance { get; set; }
    [field: SerializeField]
    public float OrbitPeriodInDays { get; set; }
    public float moonOrbitPeriodInDays;
    [field: SerializeField]
    public float Size { get; set; }

    private float orbitStartingValue;
    private float orbitAngle;
    private float moonOrbitAngle;
    private float dayCycleAngle;
    private float orbitPeriodSpeed;
    private float moonOrbitSpeed;
    private float dayCycleSpeed;

    private void Start()
    {
        orbitStartingValue = transform.position.x;
        orbitPeriodSpeed = 360 / OrbitPeriodInDays;
        moonOrbitSpeed = 360 / moonOrbitPeriodInDays;
        dayCycleSpeed = -360 / DayCycle;
    }

    private void Update()
    {
        RotateYForDayCycle();
        Orbit();
        if (tag.Equals("Moon")) MoonOrbit();
        SetScale();
        transform.FromMatrix(celestialBody);
    }

    private void RotateYForDayCycle()
    {
        dayCycleAngle += dayCycleSpeed * Time.deltaTime;
        Matrix4x4 rotationMatrixY = Matrix4x4.identity;
        float rad = dayCycleAngle * Mathf.Deg2Rad;
        rotationMatrixY.m00 = rotationMatrixY.m22 = Mathf.Cos(rad);
        rotationMatrixY.m02 = Mathf.Sin(rad);
        rotationMatrixY.m20 = -rotationMatrixY.m02;
        celestialBody = rotationMatrixY;
    }

    private void Orbit()
    {
        orbitAngle += orbitPeriodSpeed * Time.deltaTime;
        float rad = orbitAngle * Mathf.Deg2Rad;
        celestialBody.m03 = Mathf.Cos(rad) * orbitStartingValue;
        celestialBody.m23 = Mathf.Sin(rad) * orbitStartingValue;
    }

    private void MoonOrbit()
    {
        moonOrbitAngle += moonOrbitSpeed * Time.deltaTime;
        float rad = moonOrbitAngle * Mathf.Deg2Rad;
        celestialBody.m03 += Mathf.Cos(rad) * MoonDistance;
        celestialBody.m23 += Mathf.Sin(rad) * MoonDistance;
    }

    private void SetScale()
    {
        float x = Mathf.Pow(celestialBody.m00, 2) + Mathf.Pow(celestialBody.m10, 2) + Mathf.Pow(celestialBody.m20, 2);
        float y = Mathf.Pow(celestialBody.m01, 2) + Mathf.Pow(celestialBody.m11, 2) + Mathf.Pow(celestialBody.m21, 2);
        float z = Mathf.Pow(celestialBody.m02, 2) + Mathf.Pow(celestialBody.m12, 2) + Mathf.Pow(celestialBody.m22, 2);
        float size = Mathf.Pow(Size, 2);

        //Stopping scale values from going below 0 
        if (x >= size)
        {
            size = x + 1;
        }
        if (y >= size)
        {
            size = y + 1;
        }
        if (z >= size)
        {
            size = z + 1;
        }

        celestialBody.m30 = Mathf.Sqrt(size - x);
        celestialBody.m31 = Mathf.Sqrt(size - y);
        celestialBody.m32 = Mathf.Sqrt(size - z);
    }
}
