// ReSharper disable ClassNeverInstantiated.Global

namespace Entity.Relationships.Fractions
{
    public class NaturalsFraction : Fraction
    {
        public override Relation GetRelation(Fraction fraction) => fraction switch
        {
            _ => Relation.Natural
        };

        public override int Influence { get; set; }
    }
}