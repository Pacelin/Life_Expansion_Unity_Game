using System.Runtime.CompilerServices;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniOwl
{
    public abstract class ScriptableStructObject<T> : ScriptableObject where T : unmanaged
    {
        [SerializeField] private T _value;

        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value;
            set
            {
                #if UNITY_EDITOR
                Undo.RecordObject(this, $"Set: {nameof(T)}");
                #endif

                _value = value;
                
                #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
                #endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(ScriptableStructObject<T> so) => so._value;
    }
}