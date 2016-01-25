using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Attest.Fake.Builders;

namespace LogoFX.Client.Tests.EndToEnd.Shared
{
    /// <summary>
    /// Allows to manage builders collection, including serialization/deserialization.
    /// </summary>
    public static class BuildersCollectionContext
    {
        //TODO: The file name should be scenario-specific in case of parallel End-To-End tests
        //which run in the same directory - highly unlikely and thus has low priority.
        private const string SerializedBuildersPath = "SerializedBuildersCollection.Data";

        private static readonly BuildersCollection _buildersCollection = new BuildersCollection();

        /// <summary>
        /// Gets the builders of the specified service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        public static IEnumerable<FakeBuilderBase<TService>> GetBuilders<TService>() where TService : class
        {
            return _buildersCollection.GetBuilders<TService>();
        }

        /// <summary>
        /// Adds the builder of the specified service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="builder">The builder.</param>
        public static void AddBuilder<TService>(FakeBuilderBase<TService> builder) where TService : class
        {
            _buildersCollection.AddBuilder(builder);
        }

        /// <summary>
        /// Serializes the builders.
        /// </summary>
        public static void SerializeBuilders()
        {
            var fileStream = new FileStream(SerializedBuildersPath, FileMode.Create);

            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, _buildersCollection);
            fileStream.Close();
        }

        /// <summary>
        /// Deserializes the builders.
        /// </summary>
        public static void DeserializeBuilders()
        {
            var fs = new FileStream(SerializedBuildersPath, FileMode.Open);
            var bf = new BinaryFormatter();

            var data = (BuildersCollection)bf.Deserialize(fs);
            _buildersCollection.ResetBuilders(data.GetAllBuilders());
            fs.Close();
        }
    }

    /// <summary>
    /// Represents builders collection.
    /// </summary>
    [Serializable]    
    public class BuildersCollection
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
