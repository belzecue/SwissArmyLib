using UnityEngine;

namespace Archon.SwissArmyLib.Gravity
{
    /// <summary>
    /// A sphere-shaped gravitational point.
    /// 
    /// <remarks>The force is currently constant and not dependent on how close the entities are.</remarks>
    /// </summary>
    public class SphericalGravitationalPoint : MonoBehaviour, IGravitationalPoint
    {
        [SerializeField] private float _strength = 9.82f;
        [SerializeField] private float _radius = 1;
        [SerializeField] private bool _isGlobal;

        private float _radiusSqr;

        /// <summary>
        /// The gravitational pull of this point.
        /// </summary>
        public float Strength
        {
            get { return _strength; }
            set { _strength = value; }
        }

        /// <summary>
        /// Gets or sets the radius of this gravitational point.
        /// 
        /// <remarks>If <see cref="IsGlobal"/> is true, then this property is ignored.</remarks>
        /// </summary>
        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                _radiusSqr = value * value;
            }
        }

        /// <summary>
        /// Gets or sets whether this point should affect all entities regardless of whether they're in range.
        /// </summary>
        public bool IsGlobal
        {
            get { return _isGlobal; }
            set { _isGlobal = value; }
        }

        private void Awake()
        {
            _radiusSqr = _radius * _radius;
        }

        private void OnEnable()
        {
            GravitationalSystem.Register(this);
        }

        private void OnDisable()
        {
            GravitationalSystem.Unregister(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }

        public Vector3 GetForceAt(Vector3 location)
        {
            var deltaPos = transform.position - location;

            if (IsGlobal || deltaPos.sqrMagnitude < _radiusSqr)
                return deltaPos.normalized * _strength;

            return Vector3.zero;
        }
    }
}