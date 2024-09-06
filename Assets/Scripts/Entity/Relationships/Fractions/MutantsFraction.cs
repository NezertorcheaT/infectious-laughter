// ReSharper disable ClassNeverInstantiated.Global
namespace Entity.Relationships.Fractions
{
    public class MutantsFraction : Fraction
    {
        public override Relation GetRelation(Fraction fraction)
        {
            return fraction switch
            {
                MutantsFraction => Relation.Friendly,
                PlayerFraction => Relation.Hostile,
                _ => Relation.Natural
            };
        }
        public override Relation GetInfluence(Fraction fraction)
        {
            throw new System.NotImplementedException();
        }
    }
}