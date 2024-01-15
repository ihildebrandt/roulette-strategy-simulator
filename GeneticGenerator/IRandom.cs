namespace GeneticGenerator;

public interface IRandom
{
    int Next();
    int NextIndividualGeneCount();
    int NextCrossOver(int aLength, int bLength);
    Gene NextGene();
    MutationType NextMutation();
}
