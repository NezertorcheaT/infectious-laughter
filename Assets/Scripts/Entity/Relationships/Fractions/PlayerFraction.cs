// ReSharper disable ClassNeverInstantiated.Global
namespace Entity.Relationships.Fractions
{
    public class PlayerFraction : Fraction
    {
        public override Relation GetRelation(Fraction fraction)
        {
            return fraction switch
            {
                MutantsFraction => Relation.Hostile,
                PlayerFraction => Relation.Friendly,
                _ => Relation.Natural
            };
        }
        public override Relation GetInfluence(Fraction fraction)
        {
            throw new System.NotImplementedException();
        }
    }
}