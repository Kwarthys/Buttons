using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class InstrumentGenerator : MonoBehaviour
{
    public NamesData data;

    [SerializeField]
    private float techChance = 0.5f;
    [SerializeField]
    private float compChance = 0.5f;
    [SerializeField]
    private float suffixChance = 0.2f;

    public struct InstrumentNameData
    {
        public int coreID;
        public string name;
    }

    private void Awake()
    {
        TextAsset jsonTextFile = Resources.Load<TextAsset>("names");
        data = JsonUtility.FromJson<NamesData>(jsonTextFile.text);

        List<int> takenCores = new List<int>();

        for(int i = 0; i < 7; ++i)
        {
            InstrumentNameData instrument = generateInstrumentName(takenCores);

            if(instrument.coreID != -1)
            {
                takenCores.Add(instrument.coreID);
                Debug.Log(instrument.name);
            }
        }
    }

    public InstrumentNameData generateInstrumentName(List<int> bannedIDs)
    {
        InstrumentNameData instruent = new InstrumentNameData();
        instruent.coreID = -1;

        if(data == null)
        {
            Debug.LogError("Trying to generate an instrument with names data not loaded");
            return instruent;
        }

        string name = "";
        int coresSize = data.Cores.Length;
        int coreID = (int)(Random.value * coresSize);

        if(bannedIDs.Contains(coreID))
        {
            for(int i = coreID; i < coresSize && bannedIDs.Contains(coreID); ++i)
                coreID = (coreID + i) % coresSize;

            if(bannedIDs.Contains(coreID))
            {
                Debug.LogError("Could not create instrument, all cores taken");
                return instruent;
            }
        }

        name += data.Cores[coreID];

        if(Random.value < techChance)
            name = data.Technologies[(int)(Random.value * data.Technologies.Length)] + " " + name;

        if(Random.value < compChance)
            name = data.Complements[(int)(Random.value * data.Complements.Length)] + " " + name;

        if(Random.value < suffixChance)
            name = name + " " + data.Suffixes[(int)(Random.value * data.Suffixes.Length)];

        instruent.coreID = coreID;
        instruent.name = name;

        return instruent;
    }
}

public class NamesData
{
    public string[] Cores;
    public string[] Technologies;
    public string[] Complements;
    public string[] Suffixes;
}
