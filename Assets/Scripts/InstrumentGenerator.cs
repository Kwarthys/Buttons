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

    private List<InstrumentNameData> takenNames = new List<InstrumentNameData>();

    public struct InstrumentNameData
    {
        public int coreID;
        public int techID;
        public int compID;
        public int suffixID;
        public string name;

        public static InstrumentNameData empty()
        {
            InstrumentNameData data;
            data.coreID = -1; data.techID = -1; data.compID = -1; data.suffixID = -1; data.name = "";
            return data;
        }
    }

    private void loadNames()
    {
        TextAsset jsonTextFile = Resources.Load<TextAsset>("names");
        data = JsonUtility.FromJson<NamesData>(jsonTextFile.text);
        Resources.UnloadAsset(jsonTextFile);
    }

    public List<string> generateInstruments(int numberOfInstruments, int difficulty)
    {
        List<string> names = new List<string>();

        for(int i = 0; i < numberOfInstruments; ++i)
        {
            InstrumentNameData instrument = generateInstrumentName(takenNames, difficulty);

            if(instrument.coreID != -1)
            {
                //Successful creation
                takenNames.Add(instrument);
                names.Add(instrument.name);
            }
        }

        return names;
    }

    private InstrumentNameData generateInstrumentName(List<InstrumentNameData> bannedIDs, int difficulty)
    {
        if(data == null)
        {
            loadNames();
        }

        bool valid = false;
        int tries = 0;
        InstrumentNameData instrument = InstrumentNameData.empty();

        while(!valid && tries < 1000)
        {
            tries++;
            instrument = InstrumentNameData.empty();

            instrument.coreID = (int)(Random.value * data.Cores.Length);
            if(Random.value < techChance)
                instrument.techID = (int)(Random.value * data.Technologies.Length);
            if(Random.value < compChance)
                instrument.compID = (int)(Random.value * data.Complements.Length);
            if(Random.value < suffixChance)
                instrument.suffixID = (int)(Random.value * data.Suffixes.Length);

            valid = isAllowed(instrument, bannedIDs, difficulty);
        }

        if(!valid) //all attempts failed
        {
            Debug.LogWarning("Name Creation failed");
            return InstrumentNameData.empty();
        }

        //ID Combination is valid -> build name and return instrumentNameData [COMPLEMENT TECHNOLOGY CORE SUFFIX] (only core is mandatory)
        string name = data.Cores[instrument.coreID];
        if(instrument.techID != -1)
            name = data.Technologies[instrument.techID] + " " + name;
        if(instrument.compID != -1)
            name = data.Complements[instrument.compID] + " " + name;
        if(instrument.suffixID != -1)
            name = name + " " + data.Suffixes[instrument.suffixID];

        instrument.name = name;

        return instrument;
    }

    private bool isAllowed(InstrumentNameData candidate, List<InstrumentNameData> bannedIDs, int difficulty)
    {
        int core = 0, tech = 1, comp = 2, suffix = 3;

        foreach(InstrumentNameData banned in bannedIDs)
        {
            bool[] matches = { false, false, false, false };

            if(candidate.coreID == banned.coreID)
                matches[core] = true;
            if(candidate.techID == banned.techID)
                matches[tech] = true;
            if(candidate.compID == banned.compID)
                matches[comp] = true;
            if(candidate.suffixID == banned.suffixID)
                matches[suffix] = true;

            //evaluate
            switch(difficulty)
            {
                case (0): //easy
                {
                    if(matches[core] == true)
                        return false;
                }break;
                case (1): //normal
                {
                    if(matches[core] == true && matches[tech] == true)
                        return false;
                }break;
                case (2): //hard
                {
                    if(matches[core] == true && matches[tech] == true && matches[comp] == true)
                        return false;
                }break;
                case (3): //very hard
                {
                    if(matches[core] == true && matches[tech] == true && matches[comp] == true && matches[suffix] == true)
                        return false;
                }break;
            }
        }

        return true;
    }
}

public class NamesData
{
    public string[] Cores;
    public string[] Technologies;
    public string[] Complements;
    public string[] Suffixes;
}
