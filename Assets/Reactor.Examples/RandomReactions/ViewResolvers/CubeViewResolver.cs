using Reactor.Entities;
using Reactor.Unity.Systems;
using UnityEngine;

namespace Assets.Reactor.Examples.RandomReactions.ViewResolvers
{
    public class CubeViewResolver : ViewResolverSystem
    {
        private const float _spacing = 2.0f;
        private const int _perRow = 50;

        private Vector3 _nextPosition = Vector3.zero;
        private int _currentOnRow = 0;

        private readonly GameObject _coloredCubePrefab;

        public CubeViewResolver(IViewHandler viewHandler) : base(viewHandler)
        {
            _coloredCubePrefab = (GameObject)Resources.Load("colored-cube");
        }

        public override GameObject ResolveView(IEntity entity)
        {
            var view = Object.Instantiate(_coloredCubePrefab, _nextPosition, Quaternion.identity);
            IncrementRow();
            return view;
        }
        
        private void IncrementRow()
        {
            _currentOnRow++;

            if (_currentOnRow < _perRow)
            {
                _nextPosition.x += _spacing;
                return;
            }

            _currentOnRow = 0;
            _nextPosition.x = 0.0f;
            _nextPosition.z += _spacing;
        }
    }
}