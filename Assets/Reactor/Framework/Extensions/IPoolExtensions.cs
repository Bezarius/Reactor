﻿using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Components;
using Reactor.Entities;
using Reactor.Pools;

namespace Reactor.Extensions
{
    public static class IPoolExtensions
    {
        public static void RemoveEntitiesContaining<T>(this IPool pool)
            where T : class, IComponent
        {
            pool.Entities.Where(entity => entity.HasComponent<T>())
                .ToList()
                .ForEachRun(pool.RemoveEntity);
        }

        public static void RemoveEntitiesContaining(this IPool pool, params Type[] components)
        {
            pool.Entities.Where(entity => components.Any(x => entity.HasComponents(x)))
                .ToList()
                .ForEachRun(pool.RemoveEntity);
        }

        public static void RemoveAllEntities(this IPool pool)
        {
            var allEntities = pool.Entities.ToArray();
            foreach (var entity in allEntities)
            {
                pool.RemoveEntity(entity);
            }
        }

        public static IEnumerable<IEntity> Query(this IPool pool, IPoolQuery query)
        {
            return query.Execute(pool.Entities);
        }
    }
}