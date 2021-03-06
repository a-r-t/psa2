﻿using PSA2.src.FileProcessor.MovesetHandler.Configs;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class CharacterParamsHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public CharacterSpecificParametersConfig CharacterSpecificParametersConfig { get; private set; }

        public CharacterParamsHandler(PsaFile psaFile, int dataSectionLocation, string movesetBaseName, PsaCommandHandler psaCommandHandler)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandHandler = psaCommandHandler;
            CharacterSpecificParametersConfig = Utils.LoadJson<CharacterSpecificParametersConfig>($"data/char_specific/{movesetBaseName}.json");
        }

        // the result of this * 4 is the offset for the params that shows in PSA when you go to No Select and look at Data Table
        public int GetCharacterParametersDataLocation(int paramsId)
        {
            int paramsLocation = Convert.ToInt32(CharacterSpecificParametersConfig.Parameters[paramsId].Location, 16);
            return PsaFile.DataSection[DataSectionLocation + paramsLocation / 4] / 4;
        }

        // formally known as the "No Selects", I don't believe these are article related at all
        // or at least related to an instance of an article or anything
        // I think it's parameters set for a character as a whole
        public List<int> GetCharacterParameterValues(int paramsId)
        {
            List<int> characterParameterValues = new List<int>();
            int characterParametersDataLocation = GetCharacterParametersDataLocation(paramsId);
            Console.WriteLine(characterParametersDataLocation.ToString("X"));

            int numberOfParamValues = CharacterSpecificParametersConfig.Parameters[paramsId].Values.Count;
            for (int i = 0; i < numberOfParamValues; i++)
            {
                characterParameterValues.Add(PsaFile.DataSection[characterParametersDataLocation + i]);
            }
            characterParameterValues.ForEach(x => Console.WriteLine(x.ToString("X")));
            return characterParameterValues;
        }

        // I guess a series of locations need to be followed to get to these "extra" params, and some characters (like Kirby) have A LOT of them and may have even more locations?
        // idk, it works though
        // also the result of this * 4 is the offset for the params that shows in PSA when you go to No Select and look at Data Table
        public int GetCharacterExtraParametersDataLocation(int paramsId)
        {
            int numberOfLocations = CharacterSpecificParametersConfig.ExtraParameters[paramsId].ParameterLocationPath.Count;
            int nextStartLocation = DataSectionLocation;
            int valuesDataLocation = 0;
            for (int i = 0; i < numberOfLocations; i++)
            {
                int nextValuesLocation = Convert.ToInt32(CharacterSpecificParametersConfig.ExtraParameters[paramsId].ParameterLocationPath[i], 16);
                valuesDataLocation = PsaFile.DataSection[nextStartLocation + nextValuesLocation] / 4;
                nextStartLocation = valuesDataLocation;
            }

            Console.WriteLine(valuesDataLocation);
            return valuesDataLocation;
        }

        public List<int> GetCharacterExtraParameterValues(int paramsId)
        {
            List<int> characterExtraParameterValues = new List<int>();
            int characterExtraParametersDataLocation = GetCharacterExtraParametersDataLocation(paramsId);
            Console.WriteLine((characterExtraParametersDataLocation * 4).ToString("X"));
            int numberOfParamValues = CharacterSpecificParametersConfig.Parameters[paramsId].Values.Count;
            for (int i = 0; i < numberOfParamValues; i++)
            {
                characterExtraParameterValues.Add(PsaFile.DataSection[characterExtraParametersDataLocation + i]);
            }
            characterExtraParameterValues.ForEach(x => Console.WriteLine(x.ToString("X")));
            return characterExtraParameterValues;
        }
    }
}
