using UnityEditor;
using System.Collections;
using UnityEngine;
using System.Linq;
using System;

// Impact Deformable custom editor class
[CustomEditor(typeof(ImpactDeformable))]
public class ImpactDeformableEditor : Editor
{
	// Impact deformable instance being edited
    ImpactDeformable impactDeformable;    

	// Get the rigidbody connected with the object being edited
    Rigidbody RigidBody
    {
        get
        {
            if (impactDeformable == null)
                return null;

            return impactDeformable.GetComponent<Rigidbody>();
        }
    }

	// Get the collider connected with the object being edited
    Collider Collider
    {
        get
        {
            if (impactDeformable == null)
                return null;

            return impactDeformable.GetComponent<Collider>();
        }
    }

	// Get the mesh collider connected with the object being edited
    MeshCollider MeshCollider
    {
        get
        {
            if (impactDeformable == null)
                return null;

            return impactDeformable.GetComponent<MeshCollider>() ?? impactDeformable.GetComponentInChildren<MeshCollider>();
        }
    }

    // Editor OnEnable event
    public void OnEnable()
    {
		// Assign work var
        impactDeformable = target as ImpactDeformable;

		// Find the master for this instance
        FindMaster();
    }

    // Find the master for this instance
    void FindMaster()
    {
        impactDeformable.Master = impactDeformable.FindMaster();
    }

    // Check whether this object is part of a compound collider
    bool CompountCollider()
    {
        return (Collider) && (!RigidBody) && (impactDeformable.transform.GetComponentInParent<Rigidbody>() != null);
    }

    // Editor OnSpectorGUI event
    public override void OnInspectorGUI()
    {
        GUI.changed = false;

        // Check if we have a rigidbody or a collider
        if ((RigidBody == null) && (Collider == null))
        {
            EditorGUILayout.HelpBox("Impact Deformable needs a Rigidbody or a Collider attached to this game object.", MessageType.Warning);
            return;
        }

        // Check compound collider and no master
        if (CompountCollider() && (impactDeformable.Master == null))
        {
            Rigidbody root = impactDeformable.GetComponentInParent<Rigidbody>();
            if (root != null)
            {
				// Give warning to user
                EditorGUILayout.HelpBox("This instance is part of a compound collider, based on '" + root.gameObject.name + "' object, but there is no Impact Deformable script attached in this base object.", MessageType.Warning);

				// Offer automatic creating of Impact Deformable on master
                if (GUILayout.Button("Install Impact Deformable in the base object"))
                {
                    root.gameObject.AddComponent<ImpactDeformable>();
                    FindMaster();
                }

                return;
            }
        }

        // If no mesh filter assigned, search one in the same game object.
        // If none found and this is not a master instance, report error
        impactDeformable.MeshFilter = EditorGUILayout.ObjectField("Mesh Filter", impactDeformable.MeshFilter, typeof(MeshFilter), true) as MeshFilter;
        if (impactDeformable.MeshFilter == null)
            impactDeformable.MeshFilter = impactDeformable.gameObject.GetComponent<MeshFilter>();
        if ((impactDeformable.MeshFilter == null) && (RigidBody == null))
        {
            EditorGUILayout.HelpBox("A mesh filter must be assigned for the deformation process", MessageType.Warning);
            return;
        }

        // Check master settings override
        if (impactDeformable.Master != null)
        {
            impactDeformable.OverrideMaster = EditorGUILayout.Toggle("Override Master", impactDeformable.OverrideMaster);
            if ((!impactDeformable.OverrideMaster) && (RigidBody == null))
                EditorGUILayout.HelpBox("This instance is part of a compound collider (from " + impactDeformable.Master.gameObject.name + ") and is inheriting the settings from it. If this object need particular settings they can be overridden with the flag above.", MessageType.Info);
        }

        // All other properties
        if ((impactDeformable.Master == null) || (impactDeformable.OverrideMaster))
        {
            impactDeformable.Hardness = Mathf.Max(EditorGUILayout.FloatField("Hardness", impactDeformable.Hardness), 0);
            impactDeformable.MaxDeformationRadius = Mathf.Max(EditorGUILayout.FloatField("Max Deformation Radius", impactDeformable.MaxDeformationRadius), 0);
            impactDeformable.MaxVertexMov = Mathf.Max(EditorGUILayout.FloatField("Max Vertex Movement", impactDeformable.MaxVertexMov), 0);
            impactDeformable.RandomFactorDeformation = Mathf.Clamp01(EditorGUILayout.FloatField("Random Factor in Deformations (0..1)", impactDeformable.RandomFactorDeformation));
            impactDeformable.RecalculateNormals = EditorGUILayout.Toggle("Recalculate Normals", impactDeformable.RecalculateNormals);
            impactDeformable.LimitDeformationToMeshBounds = EditorGUILayout.Toggle("Limit Deformation To Mesh Bounds", impactDeformable.LimitDeformationToMeshBounds);
            if (MeshCollider)
            {
                impactDeformable.DeformMeshCollider = EditorGUILayout.Toggle("Deform Mesh Collider", impactDeformable.DeformMeshCollider);
                if (impactDeformable.DeformMeshCollider)
                    EditorGUILayout.HelpBox("Warning: Mesh collider deformation incur heavy CPU usage", MessageType.Warning);
            }
            impactDeformable.DeformationsScale = EditorGUILayout.Vector3Field("Deformation Scale", impactDeformable.DeformationsScale);
        }

        // Print structural damage %
        EditorGUILayout.HelpBox("Structural Damage: " + (impactDeformable.StructuralDamage).ToString("0.0000") + "\n" +
            "Avg Structural Damage: " + (impactDeformable.AverageStructuralDamage).ToString("0.0000"), MessageType.Info);

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}