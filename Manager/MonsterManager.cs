using System.Collections.Generic;
using TheGame.Core;

namespace TheGame.Manager
{
    public class MonsterManager
    {
        private EntityManager _entityManager;
        private List<Monster> _monsters;

        public MonsterManager(EntityManager entityManager)
        {
            _entityManager = entityManager;
            _monsters = new List<Monster>();
        }
        
        public List<Monster> Monsters
        {
            get => _monsters;
        }

        public void CreateMonster(Monster monster)
        {
            _entityManager.RandomPosition(monster);
            
            _monsters.Add(monster);
            _entityManager.CreateEntity(monster);
        }
        
        public void RemoveMonster(Monster monster)
        {
            _monsters.Remove(monster);
            _entityManager.RemoveEntity(monster);
        }

        public List<Monster> GetNearbyMonsters(Entity source, float distance)
        {
            List<Monster> list = new List<Monster>();

            foreach (Monster monster in Monsters)
                if (source.GetDistanceBetweenEntity(monster) <= distance)
                    list.Add(monster);

            return list;
        }
    }
}