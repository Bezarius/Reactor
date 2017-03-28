using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Reactor.Examples.GroupFilters.Components;
using Assets.Reactor.Examples.GroupFilters.Filters;
using Assets.Reactor.Examples.GroupFilters.Groups;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Systems;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Reactor.Examples.GroupFilters.Systems
{
    public class LeaderboardSystem : IManualSystem
    {
        private LeaderboardFilter _leaderboardFilter;
        private CacheableLeaderboardFilter _cacheableLeaderboardFilter;

        private IDisposable _subscription;
        private Text _leaderBoardText;
        private Toggle _useCacheToggle;

        public IGroup TargetGroup { get { return new EmptyGroup(); } }

        public LeaderboardSystem(IPoolManager poolManager)
        {
            var accessor = poolManager.CreateGroupAccessor(new HasScoreGroup());
            _leaderboardFilter = new LeaderboardFilter(accessor);
            _cacheableLeaderboardFilter = new CacheableLeaderboardFilter(accessor);
            _leaderBoardText = GameObject.Find("Leaderboard").GetComponent<Text>();
            _useCacheToggle = GameObject.Find("UseCache").GetComponent<Toggle>();
        }

        public void StartSystem(IGroupAccessor @group)
        { _subscription = Observable.EveryUpdate().Subscribe(x => UpdateLeaderboard()); }

        public void UpdateLeaderboard()
        {
            IList<HasScoreComponent> leaderboardEntities;

            if(_useCacheToggle.isOn)
            { leaderboardEntities = _cacheableLeaderboardFilter.Filter().ToList(); }
            else
            { leaderboardEntities = _leaderboardFilter.Filter().ToList(); }

            var leaderboardString = new StringBuilder();
            for (var i = 0; i < leaderboardEntities.Count; i++)
            {
                leaderboardString.AppendLine(string.Format("{0} - {1} [{2}]", 
                    i, leaderboardEntities[i].Name, leaderboardEntities[i].Score));
            }
            _leaderBoardText.text = leaderboardString.ToString();
        }

        public void StopSystem(IGroupAccessor @group)
        { _subscription.Dispose(); }
    }
}