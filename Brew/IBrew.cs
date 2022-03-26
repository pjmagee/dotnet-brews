namespace Brew;

public interface IBrew
{
    void Execute()
    {
        Before();
        After();
    }

    void Before()
    {

    }

    void After()
    {

    }
}