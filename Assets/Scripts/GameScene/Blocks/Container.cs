﻿using UnityEngine;

public class Container : MonoBehaviour
{
    public int visualIndex;
    public int rowID;

    public GameObject[] blockTypes = new GameObject[8];

    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = LevelManager.Instance;
    }

    public void SpawnType(BlType type , bool applayBonus = false, bool addInListFC = true)
    {
        if (type == BlType.space && addInListFC)
        {
            AddInListFreeConts();
            return;
        }

        for (int i = 0; i < blockTypes.Length; i++)
        {
            if (blockTypes[i].GetComponent<BlockType>().Bltype == type && type != BlType.space)
            {
                GameObject go = Instantiate(blockTypes[i], transform) as GameObject;

                if (applayBonus && type == BlType.ball)
                {
                    go.GetComponent<CollectBall>().isByBonus = true;
                }
                else if (applayBonus && type == BlType.bonus)
                {
                    go.GetComponent<CollectBonus>().isByBonus = true;
                }
            }
        }
        
    }

    public ObjInfo LoadData()
    {
        return GetComponentInParent<Row>().rowMapOrg[this.visualIndex];
    }

    public void UpdateData(int newType, int newHP = -1, int newShield = -1, int newShieldON = 1)
    {
        ObjInfo objInfo = new ObjInfo
        {
            type = newType,
            hp = newHP,
            shield = newShield,
            shieldON = newShieldON
        };
        GetComponentInParent<Row>().rowMapOrg[this.visualIndex] = objInfo;
    }

    public void AddInListFreeConts()
    {
        UpdateData(8);
        levelManager.listFreeConts.Add(this);
    }

    public void RemoveContInLFC()
    {
        levelManager.listFreeConts.Remove(this);
    }

    public void DeSpawnBlock()
    {
        RemoveContInLFC();
        GetComponentInParent<Row>().evDeSpawnContainer -= DeSpawnBlock;
        //if (visualIndex == 8)
        //{
        //    EventManager.StartEvSpawn();
        //}
    }

    public void ApplySquare_Bonus()
    {
        BlType blType = BlType.ball;

        int r = Random.Range(0, 100);
        if (r >= 60)
        {
            blType = BlType.ball;
            UpdateData(2);
        }
        else if (r >= 20)
        {
            blType = BlType.bonus;
            UpdateData(3);
        }
        else if (r >= 10)
        {
            blType = BlType.square;
            UpdateData(1);
        }

        RemoveContInLFC();
        SpawnType(blType, true);
    }

}
