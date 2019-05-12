using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitArray
{
    #region Variables
    private uint[] array;
    private uint baseSize;
    #endregion

    #region Properties
    public uint BaseSize { get { return baseSize; } }
    #endregion

    #region Methods
    public BitArray(uint Size)
    {
        baseSize = Size;
        Size = (uint)(1 << (int)Mathf.Ceil(Mathf.Log(Size, 2)));
        uint arraySize = (uint)Mathf.Ceil((float)Size / 32);

        array = new uint[arraySize];
        InitDefaultArray();
    }

    private void InitDefaultArray()
    {
        for (uint i = 0; i < array.Length; i++)
            array[i] = 0;
    }

    uint GetNbBool()
    {
        return (uint)array.Length * 32;
    }

    public bool GetBit(uint index)
	{
        int indexTab = (int)((float)index / (GetNbBool() - 1) * array.Length);
        int offset = (int)index - (indexTab * 32);

		return ((array[indexTab] & (1 << offset)) > 0);
	}

    public void SetBit(uint index, bool set)
    {
        uint mask = 1;
        int indexTab = (int)(index / (GetNbBool() - 1) * array.Length);
        int offset = (int)index - (indexTab * 32);

        mask = mask << offset;

        if (set)
            array[indexTab] = array[indexTab] | mask;
        else
        {
            mask = ~mask;
            array[indexTab] = array[indexTab] & mask;
        }
    }

    int FindFirstSetBit(uint startIndex)
    {
        if (startIndex >= array.Length)
            return -1;

        for (uint index = startIndex; index < GetNbBool(); index++)
        {
            if (GetBit(index))
                return (int)array[index];
        }

        return -1;
    }
    #endregion
};