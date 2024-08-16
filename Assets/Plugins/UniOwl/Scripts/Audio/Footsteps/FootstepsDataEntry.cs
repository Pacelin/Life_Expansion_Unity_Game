using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UniOwl.Audio
{
    [Serializable]
    public class FootstepsDataEntry
    {
        public PhysicsMaterial material;
        [FormerlySerializedAs("cue")]
        public AudioContainer container;
    }
}