using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nethereum.Generators;
using Nethereum.Generators.Model;
using Nethereum.Generators.Core;
using Nethereum.Generators.Net;
using Newtonsoft.Json.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Truffle.Editor
{
    public class TruffleImporter : AssetPostprocessor
    {
        public struct TruffleArtifact
        {
            [JsonProperty("contractName")]
            public string ContractName;
            
            [JsonProperty("abi")]
            public object[] ABI;

            [JsonProperty("bytecode")]
            public string ByteCode;

            [JsonIgnore] public ContractABI ContractABI;

            [JsonIgnore] public string JsonPath;

        }
        
        public static string rootNamespace = "Truffle";
        public static string dataModelNamespace = "Data";
        public static string functionNamespace = "Functions";
        public static string eventNamespace = "Contracts";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var artifacts = importedAssets.Where(x => x.EndsWith(".json")).Select(ReadAbi).Where(ca => ca != null).Cast<TruffleArtifact>();

            Dictionary<string, FunctionABI> fAbiCache = new Dictionary<string, FunctionABI>();
            Dictionary<string, EventABI> eAbiCache = new Dictionary<string, EventABI>();
            Dictionary<string, ErrorABI> eRAbiCache = new Dictionary<string, ErrorABI>();
            Dictionary<string, StructABI> sAbiCache = new Dictionary<string, StructABI>();
            foreach (var abi in artifacts)
            {
                FixContractAbi(abi.ContractABI);

                foreach (var function in abi.ContractABI.Functions)
                {
                    if (!fAbiCache.ContainsKey(function.Name))
                    {
                        fAbiCache.Add(function.Name, function);
                        continue;
                    }
                    
                    // Check if input parameters match
                    var original = fAbiCache[function.Name];

                    if (!AllParametersMatch(original.InputParameters, function.InputParameters))
                    {
                        // If we got here, we have a conflicting function
                        function.Name += "FixMeOverloaded";
                        continue;
                    }
                    
                    // If we get here, we have a duplicate function
                    function.InputParameters = original.InputParameters;
                    
                    if (!AllParametersMatch(original.OutputParameters, function.OutputParameters))
                    {
                        // If we got here, we have a conflicting function
                        continue;
                    };

                    function.OutputParameters = original.OutputParameters;
                }
                
                foreach (var eventABI in abi.ContractABI.Events)
                {
                    if (!eAbiCache.ContainsKey(eventABI.Name))
                    {
                        eAbiCache.Add(eventABI.Name, eventABI);
                        continue;
                    }
                    
                    // Check if input parameters match
                    var original = eAbiCache[eventABI.Name];

                    if (!AllParametersMatch(original.InputParameters, eventABI.InputParameters))
                    {
                        // If we got here, we have a conflicting function
                        // Rename it to something else more unique
                        continue;
                    };

                    // If we get here, we have a duplicate function
                    eventABI.InputParameters = original.InputParameters;
                }
                
                foreach (var errorAbi in abi.ContractABI.Errors)
                {
                    if (!eRAbiCache.ContainsKey(errorAbi.Name))
                    {
                        eRAbiCache.Add(errorAbi.Name, errorAbi);
                        continue;
                    }
                    
                    // Check if input parameters match
                    var original = eRAbiCache[errorAbi.Name];

                    if (!AllParametersMatch(original.InputParameters, errorAbi.InputParameters))
                    {
                        // If we got here, we have a conflicting function
                        // Rename it to something else more unique
                        continue;
                    };

                    // If we get here, we have a duplicate function
                    errorAbi.InputParameters = original.InputParameters;
                }
                
                foreach (var structABI in abi.ContractABI.Structs)
                {
                    if (!sAbiCache.ContainsKey(structABI.Name))
                    {
                        sAbiCache.Add(structABI.Name, structABI);
                        continue;
                    }
                    
                    // Check if input parameters match
                    var original = sAbiCache[structABI.Name];

                    if (!AllParametersMatch(original.InputParameters, structABI.InputParameters))
                    {
                        // If we got here, we have a conflicting function
                        // Rename it to something else more unique
                        structABI.Name += abi.ContractName;
                        continue;
                    };

                    // If we get here, we have a duplicate function
                    structABI.InputParameters = original.InputParameters;
                }
                
                ImportAsset(abi);
            }
        }

        public static TruffleArtifact? ReadAbi(string jsonPath)
        {
            TextAsset jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);

            var text = jsonAsset.text;

            var outputDirectory = Path.Join(Directory.GetParent(new FileInfo(jsonPath).Directory.FullName).FullName,
                "csharp");

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            var artifact = JsonConvert.DeserializeObject<TruffleArtifact>(text);

            if (string.IsNullOrEmpty(artifact.ContractName))
                return null;

            var abiJson = JsonConvert.SerializeObject(artifact.ABI);

            var abi = new GeneratorModelABIDeserialiser().DeserialiseABI(abiJson);

            artifact.ContractABI = abi;
            artifact.JsonPath = jsonPath;

            return artifact;
        }

        public static void ImportAsset(TruffleArtifact artifact)
        {
            try
            {
                var outputDirectory = Path.Join(Directory.GetParent(new FileInfo(artifact.JsonPath).Directory.FullName).FullName,
                    "csharp");

                if (!Directory.Exists(outputDirectory))
                    Directory.CreateDirectory(outputDirectory);
                
                var generator = new ContractProjectGenerator(artifact.ContractABI, artifact.ContractName, artifact.ByteCode,
                    rootNamespace, eventNamespace, functionNamespace, dataModelNamespace, outputDirectory,
                    Path.DirectorySeparatorChar.ToString(), CodeGenLanguage.CSharp);

                var generatedFiles = generator.GenerateAll()
                    .Concat(generator.GenerateAllStructs());

                new GeneratedFileWriter().WriteFiles(generatedFiles);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("Failed to import truffle artifact: " + artifact.JsonPath);
            }
        }

        private static void FixContractAbi(ContractABI abi)
        {
            var typeMapper = new ABITypeToCSharpType();
            var parameterAbiModelTypeMap = new ParameterABIModelTypeMap(typeMapper, CodeGenLanguage.CSharp);
            
            foreach (var function in abi.Functions)
            {
                function.InputParameters = function.InputParameters.Select(p => FixInputParameter(p, typeMapper, parameterAbiModelTypeMap)).ToArray();
                function.OutputParameters = function.OutputParameters.Select(p => FixOutputParameter(p, typeMapper, parameterAbiModelTypeMap)).ToArray();
            }
        }

        private static ParameterABI FixInputParameter(ParameterABI parameterABI, ITypeConvertor converter, ParameterABIModelTypeMap map)
        {
            if (string.IsNullOrWhiteSpace(map.GetParameterDotNetInputMapType(parameterABI)))
            {
                // For now, convert function types to bytes24 types
                return new ParameterABI("bytes24", parameterABI.Name, parameterABI.Order, parameterABI.StructType)
                {
                    Indexed = parameterABI.Indexed
                };
            }

            return parameterABI;
        }
        
        private static ParameterABI FixOutputParameter(ParameterABI parameterABI, ITypeConvertor converter, ParameterABIModelTypeMap map)
        {
            if (string.IsNullOrWhiteSpace(map.GetParameterDotNetOutputMapType(parameterABI)))
            {
                return new ParameterABI("bytes24", parameterABI.Name, parameterABI.Order, parameterABI.StructType)
                {
                    Indexed = parameterABI.Indexed
                };
            }

            return parameterABI;
        }

        private static bool AllParametersMatch(ParameterABI[] a, ParameterABI[] b, bool matchName = false)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                ParameterABI aParam = a[i];
                ParameterABI bParam = b[i];

                if (aParam.Indexed == bParam.Indexed && aParam.StructType == bParam.StructType &&
                    aParam.Order == bParam.Order && aParam.Type == bParam.Type) continue;
                if ((matchName && aParam.Name != bParam.Name) || !matchName) return false;
            }

            return true;
        }
    }
}