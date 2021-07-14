using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using UnityEngine.UI;
using System.IO;

public class PublicPlayerController : NetworkBehaviour
{
    public GameObject wand1;
    public GameObject wand2;

    public GameObject playerPrefab;
    public GameObject privatePlayer;
    public PrivatePlayerController privatePlayerScript;
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject HandPrefab;
    public GameObject BodyPrefab;

    public GameObject headVis;
    public GameObject leftHandVis;
    public GameObject rightHandVis;

    public NetworkVariableFloat id = new NetworkVariableFloat(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = 300 });
    public NetworkVariableVector3 headPos = new NetworkVariableVector3(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = 0 }, Vector3.zero);
    public NetworkVariableVector3 leftHandPos = new NetworkVariableVector3(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = 0 }, Vector3.zero);
    public NetworkVariableVector3 rightHandPos = new NetworkVariableVector3(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = 0 }, Vector3.zero);
    public NetworkVariableQuaternion headRot = new NetworkVariableQuaternion(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = 0 });
    public NetworkVariableQuaternion leftHandRot = new NetworkVariableQuaternion(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = 0 });
    public NetworkVariableQuaternion rightHandRot = new NetworkVariableQuaternion(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = 0 });

    public Vector3 tempHeadPos;
    public Vector3 tempLeftHandPos;
    public Vector3 tempRightHandPos;

    public Vector3 headPosChange;
    public Vector3 leftHandPosChange;
    public Vector3 rightHandPosChange;

    public Vector3 privateHeadPos;
    public Vector3 privateLeftHandPos;
    public Vector3 privateRightHandPos;

    public Image healthBar;

    private float headTimer;
    private float leftHandTimer;
    private float rightHandTimer;

    public int moveItemIndexRight = 0;
    public int moveItemIndexLeft = 0;
    public StorageItem item1;
    public StorageItem item2;

    public GameObject chargeAnim;

    public List<Projectile> delayedProjectiles = new List<Projectile>();

    //stats
    public NetworkVariableFloat maxHP = new NetworkVariableFloat(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.ServerOnly, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = 60 }, 100);
    public NetworkVariableFloat hP = new NetworkVariableFloat(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.Everyone, ReadPermission = NetworkVariablePermission.Everyone, SendTickrate = 10 }, 100);

    //Level of charge
    public bool channelMana = false;

    //Earth
    public bool chargeEarth = false;

    //Water
    public bool chargeWater = false;
    public bool doubleWater = false;

    //Fire
    public bool chargeFire = false;
    public bool doubleFire = false;

    //projectiles
    public GameObject fireball;
    public GameObject iceSpear;
    public GameObject rockShard;
    public GameObject inferno;

    //Shields
    public GameObject bunker;
    public GameObject shield;

    //Debugging
    private bool isLocal = false;
    public float lastUpdate = 0;

    void Start()
    {
        item1 = Instantiate(wand1).GetComponent<StorageItem>();
        item2 = Instantiate(wand2).GetComponent<StorageItem>();
        if (IsLocalPlayer)
        {
            id.Value = Random.Range(0f, 99f);
            privatePlayer = Instantiate(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation);
            privatePlayerScript = privatePlayer.GetComponent<PrivatePlayerController>();
            head = privatePlayerScript.head;
            leftHand = privatePlayerScript.leftHand;
            rightHand = privatePlayerScript.rightHand;
            privatePlayerScript.publicPlayerScript = GetComponent<PublicPlayerController>();
            privatePlayer.GetComponentInChildren<Storage>().storageItems.Add(item1.GetComponent<StorageItem>());
            item1.isLocal = true;
            item1.prPC = privatePlayerScript;
            item1.puPC = gameObject.GetComponent<PublicPlayerController>();
            privatePlayer.GetComponentInChildren<Storage>().storageItems.Add(item2.GetComponent<StorageItem>());
            item2.isLocal = true;
            item2.prPC = privatePlayerScript;
            item2.puPC = gameObject.GetComponent<PublicPlayerController>();
            //Debugging
            isLocal = true;
            Debugger.LogError("player joined");
        }
        headVis = Instantiate(BodyPrefab);
        foreach (CollisionLink collisionLink in headVis.GetComponentsInChildren<CollisionLink>())
        {
            collisionLink.linkedObject = this.gameObject;
        }
        healthBar = headVis.GetComponent<CollisionLink>().linkedHealthBar;
        leftHandVis = Instantiate(HandPrefab);
        rightHandVis = Instantiate(HandPrefab);
    }
    void Update()
    {
        //Debugging
        lastUpdate = 0;

        if (IsLocalPlayer)
        {
            headPos.Value = head.transform.position;
            leftHandPos.Value = leftHand.transform.position;
            rightHandPos.Value = rightHand.transform.position;
            headRot.Value = head.transform.rotation;
            leftHandRot.Value = leftHand.transform.rotation;
            rightHandRot.Value = rightHand.transform.rotation;
            privateHeadPos = head.transform.position;
            privateLeftHandPos = leftHand.transform.position;
            privateRightHandPos = rightHand.transform.position;
            if (hP.Value <= 0)
            {
                privatePlayerScript.gameObject.GetComponentInChildren<XRRig>().gameObject.transform.position = new Vector3(0, 25, 0);
                hP.Value = maxHP.Value;
            }
        }
        else
        {
            //new
            privateHeadPos = headPos.Value;
            privateLeftHandPos = leftHandPos.Value;
            privateRightHandPos = rightHandPos.Value;
            //to here
            //if (headPos.Value != tempHeadPos)
            //{
            //    privateHeadPos = headPos.Value;
            //    headPosChange = headTimer != 0 ? (headPos.Value - tempHeadPos) / headTimer : (headPos.Value - tempHeadPos) / Time.deltaTime;
            //    headTimer = 0;
            //    tempHeadPos = privateHeadPos;
            //}
            //else
            //{
            //    headTimer += Time.deltaTime;
            //    privateHeadPos += headPosChange * Time.deltaTime;
            //}
            //if (leftHandPos.Value != tempLeftHandPos)
            //{
            //    privateLeftHandPos = leftHandPos.Value;
            //    leftHandPosChange = leftHandTimer != 0 ? (leftHandPos.Value - tempLeftHandPos) / leftHandTimer : (leftHandPos.Value - tempLeftHandPos) / Time.deltaTime;
            //    leftHandTimer = 0;
            //    tempLeftHandPos = privateLeftHandPos;
            //}
            //else
            //{
            //    leftHandTimer += Time.deltaTime;
            //    privateLeftHandPos += leftHandPosChange * Time.deltaTime;
            //}
            //if (rightHandPos.Value != tempRightHandPos)
            //{
            //    privateRightHandPos = rightHandPos.Value;
            //    rightHandPosChange = (rightHandPos.Value - tempRightHandPos) / rightHandTimer;
            //    rightHandPosChange = rightHandTimer != 0 ? (rightHandPos.Value - tempRightHandPos) / rightHandTimer : (rightHandPos.Value - tempRightHandPos) / Time.deltaTime;
            //    tempRightHandPos = privateRightHandPos;
            //    rightHandTimer = 0;
            //}
            //else
            //{
            //    rightHandTimer += Time.deltaTime;
            //    privateRightHandPos += rightHandPosChange * Time.deltaTime;
            //}
            //Debugging
            //            if (isLocal)
            //            {
            //                Debugger.LogError("No longer local player");
            //            }
            //Debugger.LogError(headPos.RemoteTick.ToString());
            //Debugger.LogError(headPos.LocalTick.ToString());
        }

        headVis.transform.position = privateHeadPos;
        headVis.transform.rotation = headRot.Value;
        leftHandVis.transform.position = privateLeftHandPos;
        leftHandVis.transform.rotation = leftHandRot.Value;
        rightHandVis.transform.position = privateRightHandPos;
        rightHandVis.transform.rotation = rightHandRot.Value;

        switch (moveItemIndexRight)
        {
            case 0:
                rightHandVis.transform.position = privateRightHandPos;
                rightHandVis.transform.rotation = rightHandRot.Value;
                break;
            case 1:
                item1.transform.position = privateRightHandPos;
                item1.transform.rotation = rightHandRot.Value;
                item1.transform.Translate(item1.offsetDistance);
                item1.transform.Rotate(item1.offsetRotation);
                break;
            case 2:
                item2.transform.position = privateRightHandPos;
                item2.transform.rotation = rightHandRot.Value;
                item2.transform.Translate(item2.offsetDistance);
                item2.transform.Rotate(item2.offsetRotation);
                break;
        }
        switch (moveItemIndexLeft)
        {
            case 0:
                leftHandVis.transform.position = privateLeftHandPos;
                leftHandVis.transform.rotation = leftHandRot.Value;
                break;
            case 1:
                item1.transform.position = privateLeftHandPos;
                item1.transform.rotation = leftHandRot.Value;
                item1.transform.Translate(item1.offsetDistance);
                item1.transform.Rotate(item1.offsetRotation);
                break;
            case 2:
                item2.transform.position = privateLeftHandPos;
                item2.transform.rotation = leftHandRot.Value;
                item2.transform.Translate(item2.offsetDistance);
                item2.transform.Rotate(item2.offsetRotation);
                break;
        }

        healthBar.fillAmount = hP.Value / maxHP.Value;
    }

    [ServerRpc]
    public void MoveItemToHandServerRpc(int index, string hand)
    {
        MoveItemToHandClientRpc(index, hand);
    }

    [ClientRpc]
    public void MoveItemToHandClientRpc(int index, string hand)
    {
        if (hand == "Right")
        {
            switch (index)
            {
                case -1:
                    rightHandVis.SetActive(true);
                    moveItemIndexRight = 0;
                    break;
                case 0:
                    if (moveItemIndexLeft != 1)
                    {
                        item1.gameObject.SetActive(false);
                    }
                    if (moveItemIndexLeft != 2)
                    {
                        item2.gameObject.SetActive(false);
                    }
                    rightHandVis.SetActive(true);
                    moveItemIndexRight = 0;
                    break;
                case 1:
                    item1.gameObject.SetActive(true);
                    rightHandVis.SetActive(false);
                    moveItemIndexRight = 1;
                    break;
                case 2:
                    item2.gameObject.SetActive(true);
                    rightHandVis.SetActive(false);
                    moveItemIndexRight = 2;
                    break;
            }
        }
        else if (hand == "Left")
        {
            switch (index)
            {
                case -1:
                    leftHandVis.SetActive(true);
                    moveItemIndexLeft = 0;
                    break;
                case 0:
                    if (moveItemIndexRight != 1)
                    {
                        item1.gameObject.SetActive(false);
                    }
                    if (moveItemIndexRight != 2)
                    {
                        item2.gameObject.SetActive(false);
                    }
                    leftHandVis.SetActive(true);
                    moveItemIndexLeft = 0;
                    break;
                case 1:
                    item1.gameObject.SetActive(true);
                    leftHandVis.SetActive(false);
                    moveItemIndexLeft = 1;
                    break;
                case 2:
                    item2.gameObject.SetActive(true);
                    leftHandVis.SetActive(false);
                    moveItemIndexLeft = 2;
                    break;
            }
        }
    }


    private void ClearCharge()
    {
        channelMana = false;
        chargeWater = false;
        doubleWater = false;
        chargeEarth = false;
        chargeFire = false;
        doubleFire = false;
    }
    private Projectile SpawnProjectile(GameObject projectile, Vector3 location, Quaternion rotation, float damageMultiplier, float speedMultiplier, float sizeMultiplier, float posOffset, float rotOffset)
    {
        Projectile tempProjectile;
        tempProjectile = Instantiate(projectile, location, rotation).GetComponent<Projectile>();
        tempProjectile.damage *= damageMultiplier;
        tempProjectile.nSpeed.Value = speedMultiplier;
        tempProjectile.nSize.Value = sizeMultiplier;
        tempProjectile.gameObject.transform.Translate(new Vector3(Random.Range(-posOffset, posOffset), Random.Range(-posOffset, posOffset), 0));
        tempProjectile.gameObject.transform.Rotate(new Vector3(Random.Range(-rotOffset, rotOffset), Random.Range(-rotOffset, rotOffset), Random.Range(-rotOffset, rotOffset)));
        tempProjectile.gameObject.GetComponent<NetworkObject>().Spawn(null);
        return tempProjectile;
    }

    private Shield Spawnshield(GameObject shield, Vector3 location, Quaternion rotation, float healthMultiplier, float sizeMultiplier, string hand = "null")
    {
        Shield tempShield;
        tempShield = Instantiate(shield, location, rotation).GetComponent<Shield>();
        tempShield.health *= healthMultiplier;
        tempShield.nSize.Value = sizeMultiplier;
        tempShield.hand.Value = hand;
        tempShield.id.Value = id.Value;
        tempShield.GetComponent<NetworkObject>().Spawn();
        return tempShield;
    }

    private void PlayChargeAnim(Color color)
    {
        GameObject tempChargeAnim = Instantiate(chargeAnim, headPos.Value - Vector3.up, chargeAnim.transform.rotation);
        tempChargeAnim.GetComponent<ChargeAnimation>().nColor.Value = color;
        tempChargeAnim.GetComponent<NetworkObject>().Spawn();
    }

    private string GetUnusedHand()
    {
        GameObject tempGO = privatePlayerScript.activeWand.gameObject;
        int tempItemNr = 0;
        if (item1.gameObject == tempGO)
        {
            tempItemNr = 1;
        }
        else if (item2.gameObject == tempGO)
        {
            tempItemNr = 2;
        }
        if (moveItemIndexLeft == tempItemNr)
        {
            return "Right";
        }
        else
        {
            return "Left";
        }
    }

    [ServerRpc]
    public void CastSpellServerRpc(string keyword, Vector3 location, Quaternion rotation, float damageMultiplier, float speedMultiplier, float sizeMultiplier)
    {
        switch (keyword)
        {
            //Attack spells
            case "Fireball":
                SpawnProjectile(fireball, location, rotation, damageMultiplier, speedMultiplier, sizeMultiplier, 0, 0);    
                break;
            case "Ice Spear":
                delayedProjectiles.Add(SpawnProjectile(iceSpear, location, rotation, damageMultiplier, speedMultiplier, sizeMultiplier, 0, 0));
                break;
            case "Discharge":
                foreach (Projectile projectile in delayedProjectiles)
                {
                    projectile.ActivateClientRPC(true);
                }
                delayedProjectiles.Clear();
                break;
            case "Ice Spear Barrage":
                if (doubleWater)
                {
                    for (int i = 0; i < 400; i++)
                    {
                        delayedProjectiles.Add(SpawnProjectile(iceSpear, location, rotation, damageMultiplier, speedMultiplier, sizeMultiplier, 2, 10));
                    }
                    ClearCharge();
                }
                break;
            case "Rock Blast":
                for (int i = 0; i < 10; i++)
                {
                    SpawnProjectile(rockShard, location, rotation, damageMultiplier, speedMultiplier, sizeMultiplier, 0.1f, 10);
                }
                break;
            case "Inferno":
                if (doubleFire)
                {
                    SpawnProjectile(inferno, location, rotation, damageMultiplier, speedMultiplier, sizeMultiplier, 0, 0);
                    ClearCharge();
                }
                break;

            //Shield spells
            case "Bunker":
                if (chargeEarth)
                {
                    Spawnshield(bunker, privatePlayer.GetComponentInChildren<CharacterController>().gameObject.transform.position, bunker.transform.rotation, damageMultiplier, sizeMultiplier);
                    ClearCharge();
                }
                break;
            case "Shield":
                Spawnshield(shield, location, rotation, damageMultiplier, sizeMultiplier, GetUnusedHand());
                break;

            //Charge spells
            case "Channel Mana":
                ClearCharge();
                channelMana = true;
                PlayChargeAnim(Color.blue);
                break;
            case "Charge Earth":
                if (channelMana)
                {
                    ClearCharge();
                    chargeEarth = true;
                    PlayChargeAnim(new Color(0.65f, 0.16f, 0.16f));
                }
                    break;
            case "Charge Water":
                if (channelMana)
                {
                    ClearCharge();
                    chargeWater = true;
                    PlayChargeAnim(Color.blue);
                }
                break;
            case "Dual Water":
                if (chargeWater)
                {
                    ClearCharge();
                    doubleWater = true;
                    PlayChargeAnim(Color.blue);
                }
                break;
            case "Charge Fire":
                if (channelMana)
                {
                    ClearCharge();
                    chargeFire = true;
                    PlayChargeAnim(Color.red);
                }
                break;
            case "Dual Fire":
                if (chargeFire)
                {
                    ClearCharge();
                    doubleFire = true;
                    PlayChargeAnim(Color.red);
                }
                break;

            //Test spells
            case "Test":
                SpawnProjectile(fireball, location, rotation, 100000000, speedMultiplier, sizeMultiplier, 0, 0);
                break;
        }
    }
}
