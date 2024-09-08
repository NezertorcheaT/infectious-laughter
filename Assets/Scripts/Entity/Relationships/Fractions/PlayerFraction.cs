// ReSharper disable ClassNeverInstantiated.Global

namespace Entity.Relationships.Fractions
{
    public class PlayerFraction : Fraction
    {
        public override Relation GetRelation(Fraction fraction) => fraction switch
        {
            MutantsFraction => Relation.Hostile,
            PlayerFraction => Relation.Friendly,
            _ => Relation.Natural
        };

        public override int Influence { get; set; }
    }
}