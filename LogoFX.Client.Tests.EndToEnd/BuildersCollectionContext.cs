using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Attest.Fake.Builders;

namespace LogoFX.Client.Tests.EndToEnd
{
    internal static class BuildersCollectionContext
    {
        //TODO: The file name should be scenario-specific
        private const string SerializedBuildersPath = "SerializedBuildersCollection.Data";

        private static readonly BuildersCollection _buildersCollection = new BuildersCollection();

        public static IEnumerable<FakeBuilderBase<TService>> GetBuilders<TService>() where TService : class
        {
            return _buildersCollection.GetBuilders<TService>();
        }

        public static void AddBuilder<TService>(FakeBuilderBase<TService> builder) where TService : class
        {
            _buildersCollection.AddBuilder(builder);
        }

        public static void SerializeBuilders()
        {
            var fileStream = new FileStream(SerializedBuildersPath,
                                      FileMode.Create);

            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, _buildersCollection);
            fileStream.Close();
        }

        public static void DeserializeBuilders()
        {
            var fs = new FileStream(SerializedBuildersPath,
                                       FileMode.Open);
            var bf = new BinaryFormatter();

            var data = (BuildersCollection)bf.Deserialize(fs);
            _buildersCollection.ResetBuilders(data.GetAllBuilders());
            fs.Close();
        }
    }

    [Serializable]
    internal class BuildersCollection
    {
        private readonly List<object> _allBuilders = new List<object>();

        internal IEnumerable<FakeBuilderBase<TService>> GetBuilders<TService>() where TService : class
        {
            return _allBuilders.OfType<FakeBuilderBase<TService>>();
        }

        internal IEnumerable<object> GetAllBuilders()
        {
            return _allBuilders;
        }

        internal void AddBuilder<TService>(FakeBuilderBase<TService> builder) where TService : class
        {
            _allBuilders.Add(builder);
        }

        internal void ResetBuilders(IEnumerable<object> builders)
        {
            _allBuilders.Clear();
            _allBuilders.AddRange(builders);
        }
    }
}
