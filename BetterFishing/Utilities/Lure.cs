using System.Collections.Generic;
using UnityEngine;

namespace BetterFishing
{
    internal sealed class Lure
    {
        private readonly string _name;
        private readonly int _peakLatitude;
        private readonly int _targetFishPrefabIndex;
        private readonly GameObject _item;
        private readonly Material _material;
        private readonly Mesh _mesh;
        private readonly Vector3 _offset;
        private readonly float _saveData;

        public string Name => _name;
        public int PeakLatitude => _peakLatitude;
        public int TargetFishPrefabIndex => _targetFishPrefabIndex;
        public GameObject Item => _item;
        public Mesh LureMesh => _mesh;
        public Material LureMaterial => _material;        
        public Vector3 Offset => _offset;
        public float SaveData => _saveData;

        private Lure(string name, int peakLatitude, int targetFishPrefabIndex, GameObject item, Mesh mesh, Material material, Vector3 offset, float saveData)
        {
            _name = name;
            _peakLatitude = peakLatitude;
            _targetFishPrefabIndex = targetFishPrefabIndex;
            _item = item;
            _mesh = mesh;
            _material = material;
            _offset = offset;
            _saveData = saveData;
        }

        public static readonly Lure SpoonLure = 
            new Lure(
                "spoon lure",
                30,
                46,
                Items.SpoonLure,
                Items.SpoonLure.GetComponent<MeshFilter>().mesh,
                Items.SpoonLure.GetComponent<MeshRenderer>().material,
                new Vector3(0, -0.08f, 0),
                1);

        public static readonly Lure SwimbaitLure = 
            new Lure(
                "swimbait lure",
                35,
                34,
                Items.SwimbaitLure,
                Items.SwimbaitLure.GetComponent<MeshFilter>().mesh,
                Items.SwimbaitLure.GetComponent<MeshRenderer>().material,
                new Vector3(0, -0.22f, 0),
                2);

        public static readonly Lure TopwaterLure =
            new Lure(
                "topwater lure",
                45,
                37,
                Items.TopwaterLure,
                Items.TopwaterLure.GetComponent<MeshFilter>().mesh,
                Items.TopwaterLure.GetComponent<MeshRenderer>().material,
                new Vector3(0, -0.24f, 0),
                3);

        public static readonly IReadOnlyList<Lure> Lures = new List<Lure>
        {
            SpoonLure,
            SwimbaitLure,
            TopwaterLure,
        }.AsReadOnly();
    }
}
