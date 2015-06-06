
namespace Model.Map
{
    using System;
    using System.Collections.Generic;

    public class FeatureDefinition
    {
        private IDictionary<int, MapFeature> _featureById;
        private IDictionary<string, MapFeature> _featureByName;

        public FeatureDefinition()
        {
            _featureById = new Dictionary<int, MapFeature>();
            _featureByName = new Dictionary<string, MapFeature>();
        }

        public void Add(MapFeature feature)
        {
            _featureById[feature.Id] = feature;
            _featureByName[feature.Name] = feature;
        }

        public MapFeature ByName(string name)
        {
            if (_featureByName.ContainsKey(name) == false)
            {
                throw new InvalidOperationException(String.Format("Unable to locate feature '{0}'.", name));
            }
            return _featureByName[name];
        }

        public MapFeature ById(int id)
        {
            return _featureById[id];
        }

        public ICollection<MapFeature> Terrain
        {
            get
            {
                return _featureById.Values;
            }
        }
    }
}

