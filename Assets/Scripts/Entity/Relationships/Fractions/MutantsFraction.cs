// ReSharper disable ClassNeverInstantiated.Global

namespace Entity.Relationships.Fractions
{
    public class MutantsFraction : Fraction
    {
        public override Relation GetRelation(Fraction fraction) => fraction switch
        {
            MutantsFraction => Relation.Friendly,
            PlayerFraction => Relation.Hostile,
            _ => Relation.Natural
        };

        public override int Influence { get; set; }
    }
}