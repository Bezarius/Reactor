using System;
using Reactor.Events;
using Reactor.Pools;

namespace Reactor.Entities
{
    public class DefaultEntityFactory : IEntityFactory
    {
        private readonly IEventSystem _eventSystem;

        public DefaultEntityFactory(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        // todo: подумать, может стоит заменить на int\long и ресайклить значения.
        // Создания Guid'a весомая операция, которая при высокой нагрузке потребляет ~2% от общей нагрузки(на момент замера)
        // так же int в качестве идентификатора лучше подходит для массивов
        // с другой стороны операция сопоставима с добавлением сущности в справочник
        public IEntity Create(IPool pool, Guid? id = null)
        {
            if (!id.HasValue)
            { id = Guid.NewGuid(); }

            return new Entity(id.Value, pool, _eventSystem);
        }
    }
}