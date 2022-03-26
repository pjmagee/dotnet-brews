
using System;

namespace Brew.SOLID;

public class InterfaceSegregationPrinciple : IBrew
{
    #region Without

    // Genric interface with many features
    public interface IHeroBefore
    {
        void Heal();
        void Peel();
        void Assassinate();
        void Gank();
        void BasicAttack();
    }

    // Forced to implement all features, but not support all functions of the interface
    // One implementation to be replaced with an alternative may produce different behaviour
    // This goes against the liskov substitution principle. 
    public class Diablo : IHeroBefore
    {
        public void Heal()
        {
            throw new NotImplementedException();
        }

        public void Peel()
        {
            Console.WriteLine("This hero can peel enemies off team members");
        }

        public void Assassinate()
        {
            throw new NotImplementedException();
        }

        public void Gank()
        {
            Console.WriteLine("This hero can gank enemies with his dive");
        }

        public void BasicAttack()
        {
            Console.WriteLine("This hero can basic melee attack");
        }
    }

    #endregion

    #region With

    public interface IHeroAfter
    {
        void BasicAttack();
    }

    public interface IHealer : IHeroAfter
    {
        void Heal();
    }

    public interface IGanker : IHeroAfter
    {
        void Gank();
    }

    public interface IAssassin : IHeroAfter
    {
        void Assassinate();
    }

    public class LtMorales : IHealer
    {
        public void BasicAttack()
        {
            // can basic attack
        }

        public void Heal()
        {
            // can heal
        }
    }

    public class Genji : IGanker, IAssassin
    {
        public void BasicAttack()
        {
            // can basic attack
        }

        public void Assassinate()
        {
            // can assassinate
        }

        public void Gank()
        {
            // can gank
        }
    }

    #endregion

    public void Before()
    {
        new Diablo();
    }

    public void After()
    {
        new LtMorales();
        new Genji();
    }
}
