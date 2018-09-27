﻿using Reactor.Entities;
using Reactor.Unity.Systems;
using UnityEngine;

namespace Assets.Tests.Scenes.GroupedPerformance.ViewResolvers
{
    public class CubeViewResolver : ViewResolverSystem
    {
        private const float _spacing = 2.0f;
        private const int _perRow = 10;

        private Vector3 _nextPosition = Vector3.zero;
        private int _currentOnRow = 0;

        public CubeViewResolver(IViewHandler viewHandler) : base(viewHandler)
        {}

        public override GameObject ResolveView(IEntity entity)
        {
            var view = GameObject.CreatePrimitive(PrimitiveType.Cube);
            view.transform.position = _nextPosition;
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