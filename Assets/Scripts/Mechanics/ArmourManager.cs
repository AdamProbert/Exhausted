using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(AudioSource))]
public class ArmourManager : MonoBehaviour
{
    ArmourPiece[] equipedArmour;
    Player player;
    int armourLayer;

    AudioSource audioSource;

    CinemachineImpulseSource impulseSource;

    [SerializeField] LayerMask damagingLayers;
    [SerializeField] LayerMask pickupLayers;
    [SerializeField] float armourRespawnDistance;

    [SerializeField] bool debug;

    public void MessagePickupArmour()
    {
        foreach (ArmourPiece item in equipedArmour)
        {
            if(item.currentState == ArmourPiece.State.Broken)
            {
                item.EnableFlyingMode();
                StartCoroutine (MoveOverSpeed (item, Random.Range(30f,70f)));
            }
        }
    }

    private void Update() 
    {
        if(debug)
        {
            if(Keyboard.current.tKey.wasPressedThisFrame)
            {
                equipedArmour[Random.Range(0, equipedArmour.Length)].AlterStrength(-50);
            }
        }    
    }

    public void PlayerActive() 
    {
        player = transform.root.GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        if(equipedArmour == null)
        {
            equipedArmour = GetComponentsInChildren<ArmourPiece>();
        }
    }
    
    public float GetTotalArmour()
    {
        if(equipedArmour == null)
        {
            equipedArmour = GetComponentsInChildren<ArmourPiece>();
        }
        
        float totalArmour = 0;
        foreach (ArmourPiece item in equipedArmour)
        {
            if(item.currentState == ArmourPiece.State.Equipped)
            {
                totalArmour += item.MaxStrength;
            }
        }
        return totalArmour;
    }

    public IEnumerator MoveOverSpeed (ArmourPiece pieceToMove, float speed){
        Vector3 end = transform.position + pieceToMove.GetOGPosition();

        while (Vector3.Distance(pieceToMove.transform.position,end) > 1f)
        {
            end = transform.position + pieceToMove.GetOGPosition();
            pieceToMove.transform.position = Vector3.MoveTowards(pieceToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame ();
        }
        impulseSource.GenerateImpulse();
        pieceToMove.ReAttach();
        player.currentArmour += pieceToMove.MaxStrength;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.GetContact(0).thisCollider.gameObject.layer == armourLayer && damagingLayers == (damagingLayers | (1 << other.gameObject.layer)))
        {
            ArmourPiece ap = other.GetContact(0).thisCollider.gameObject.GetComponent<ArmourPiece>();
            BaseProjectile bp = other.gameObject.GetComponent<BaseProjectile>();
            ap.AlterStrength(bp.GetDamage());
        }
    }

}
