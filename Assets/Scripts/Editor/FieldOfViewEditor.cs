using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyFieldOfView fov = (EnemyFieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = fov.playerSeen ? Color.red : Color.yellow; // Cambia il colore in base alla visibilitÓ del giocatore
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        if (fov.playerSeen)
        {
            // Aggiungi un punto nella direzione del giocatore
            Handles.color = Color.green;
            Handles.DrawSolidDisc(fov.transform.position + viewAngle01 * fov.radius, Vector3.up, 0.5f);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

